using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DualPantoFramework;
using System.Threading.Tasks;
using SpeechIO;

namespace PantoDrawing
{
    public class LineDraw : MonoBehaviour
    {

        public GameObject linePrefab;
        public GameObject currentLine;
        public bool canDraw = false;

        UpperHandle upperHandle;
        LowerHandle lowerHandle;

        float upperRotation;
        float angle;
        public int lineCount = 0;
        //currently adding points to a line
        bool drawing = false;
        bool mouse = true;

        public LineRenderer lineRenderer;

        public List<Vector3> fingerPositions;

        public Dictionary<string, LineRenderer> lines = new Dictionary<string, LineRenderer>();

        // Start is called before the first frame update
        void Start()
        {
            upperHandle = GameObject.Find("Panto").GetComponent<UpperHandle>();
            lowerHandle = GameObject.Find("Panto").GetComponent<LowerHandle>();
            upperRotation = lowerHandle.GetRotation();
        }

        // Update is called once per frame
        void Update()
        {
            if(!canDraw)
                return;
            angle = upperHandle.GetRotation() - upperRotation;
            angle = Mathf.Abs((angle + 180) % 360 - 180);
            if((angle > 80f  && !mouse && !drawing) || (Input.GetMouseButtonDown(0) && mouse)) 
            {
                upperRotation = upperHandle.GetRotation();
                angle = upperHandle.GetRotation() - upperRotation;
                angle = Mathf.Abs((angle + 180) % 360 - 180);
                CreateLine();
                drawing = true;
            }
            if((angle < 30f && !mouse && drawing) || (Input.GetMouseButton(0) && mouse)){
                Vector3 tempFingerPos = upperHandle.HandlePosition(transform.position);
                tempFingerPos.y = .1f;
                Vector3 latestPos = fingerPositions[fingerPositions.Count - 1];
                latestPos.y = .1f;
                if(Vector3.Distance(tempFingerPos, latestPos) > .1f)
                {
                    UpdateLine(lineRenderer, tempFingerPos);
                }
            }else{
                if(drawing)
                {
                    lines.Add("line"+lineCount, lineRenderer);
                    lineRenderer.name = "line"+lineCount;
                    GameObject.Find("Panto").GetComponent<GameManager>().keywords.Add("line"+lineCount, () =>
                    {
                        TraceLine(lines["line"+lineCount]);
                    });
                    lineCount++;
                    drawing = false;
                }
            }
        }

        LineRenderer CreateLine()
        {   
            currentLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
            lineRenderer = currentLine.GetComponent<LineRenderer>();
            fingerPositions.Clear();
            Vector3 newPos = upperHandle.HandlePosition(transform.position);
            newPos.y = .1f;
            fingerPositions.Add(newPos);
            fingerPositions.Add(newPos);
            lineRenderer.SetPosition(0, fingerPositions[0]);
            lineRenderer.SetPosition(1, fingerPositions[1]);
            return lineRenderer;
        }

        public void CreateCircle(){
            LineRenderer line = lines["line"+(lineCount-1)];
            Vector3 center = GetCircleCenter(line);
            Vector3 radius = GetCircleRadius(line, center);
            CreateCirclePoints(line, center, (radius.x + radius.z) / 2);
        }

        void CreateCirclePoints (LineRenderer line, Vector3 center, float radius)
        {
            float x;
            float y = .1f;
            float z;
        
            float angle = 20f;
        
            for (int i = 0; i < line.positionCount; i++)
            {
                x = Mathf.Sin (Mathf.Deg2Rad * angle) * radius;
                z = Mathf.Cos (Mathf.Deg2Rad * angle) * radius;

                line.SetPosition (i,new Vector3(x+center.x,y,z+center.z));
                    
                angle += (360f / line.positionCount);
            }
        }

        Vector3 GetCircleCenter(LineRenderer line)
        {
            float sumX = 0, sumZ = 0;
            Vector3[] linePos = new Vector3[line.positionCount];
            line.GetPositions(linePos);
            for (int i = 0; i < line.positionCount; i++){
                sumX += linePos[i].x;
                sumZ += linePos[i].z;
            }
            return new Vector3(sumX/line.positionCount, .1f, sumZ/line.positionCount);
        }

        Vector3 GetCircleRadius(LineRenderer line, Vector3 center)
        {
            float sumX = 0, sumZ = 0;
            Vector3[] linePos = new Vector3[line.positionCount];
            line.GetPositions(linePos);
            for (int i = 0; i < line.positionCount; i++){
                sumX += Mathf.Abs(linePos[i].x-center.x);
                sumZ += Mathf.Abs(linePos[i].z-center.z);
            }
            return new Vector3(sumX/line.positionCount, .1f, sumZ/line.positionCount);
        }

        void UpdateLine(LineRenderer line, Vector3 newFingerPos)
        {
            fingerPositions.Add(newFingerPos);
            line.positionCount++;
            line.SetPosition(line.positionCount - 1, newFingerPos);
        }

        public async Task TraceLine(LineRenderer line)
        {
            Vector3[] linePos = new Vector3[line.positionCount];
            line.GetPositions(linePos);
            for (int i = 0; i < line.positionCount; i += 2)
            {
                await lowerHandle.MoveToPosition(linePos[i], .1f);
            }
            Debug.Log(linePos[0]);
        }

        public async void FindStartingPoint(LineRenderer line)
        {
            Vector3[] linePos = new Vector3[line.positionCount];
            line.GetPositions(linePos);
            await lowerHandle.MoveToPosition(linePos[0], .2f);
        }

        public async Task ShowLines(){
            foreach (KeyValuePair<string, LineRenderer> line in lines)
            {
                await TraceLine(line.Value);
            }
        }

        public void CombineLines(LineRenderer line1, LineRenderer line2, bool inverted = false)
        {
            Vector3[] line2Pos = new Vector3[line2.positionCount];
            line2.GetPositions(line2Pos);

            if (!inverted){
               for(int i = 0; i < line2.positionCount; i++){
                line1.positionCount++;
                line1.SetPosition(line1.positionCount - 1, line2Pos[i]);
                } 
            } else {
                for(int i = line2.positionCount-1; i >= 0; i--){
                line1.positionCount++;
                line1.SetPosition(line1.positionCount - 1, line2Pos[i]);
                } 
            }
        }
    }
}