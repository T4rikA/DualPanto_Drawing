using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PantoDrawing
{
    public class LineDraw : MonoBehaviour
    {

        public GameObject linePrefab;
        public GameObject currentLine;

        public bool canDraw;

        UpperHandle upperHandle;
        LowerHandle lowerHandle;

        float lowerRotation;

        //currently adding points to a line
        bool drawing;
        public bool mouse = false;

        public LineRenderer lineRenderer;

        public List<Vector3> fingerPositions;

        //Dictonary to store all lines e.g. eye -> contains a list of lines user named eye
        public Dictionary<string, List<LineRenderer>> lines = new Dictionary<string, List<LineRenderer>>();

        // Start is called before the first frame update
        void Start()
        {
            upperHandle = GameObject.Find("Panto").GetComponent<UpperHandle>();
            lowerHandle = GameObject.Find("Panto").GetComponent<LowerHandle>();
            lowerRotation = lowerHandle.GetRotation();
        }

        // Update is called once per frame
        void Update()
        {
            if(!canDraw)
                return;
            if(Mathf.Abs(lowerRotation - lowerHandle.GetRotation()) > 180 && !mouse || Input.GetMouseButtonDown(0) && mouse) 
            {
                lowerRotation = lowerHandle.GetRotation();
                CreateLine();
                drawing = true;
            }
            if(drawing && Mathf.Abs(lowerRotation - lowerHandle.GetRotation()) < 20 && !mouse || Input.GetMouseButton(0) && mouse){
                Vector3 tempFingerPos = upperHandle.HandlePosition(transform.position);
                tempFingerPos.y = .1f;
                Vector3 latestPos = fingerPositions[fingerPositions.Count - 1];
                latestPos.y = .1f;
                if(Vector3.Distance(tempFingerPos, latestPos) > .1f)
                {
                    UpdateLine(lineRenderer, tempFingerPos);
                }
            }else{
                drawing = false;
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
            lowerRotation = lowerHandle.GetRotation();
            return lineRenderer;
        }


        public void DrawCircle(string name) {
            LineRenderer line = CreateLine();
            line.name = name;
        }

        void CreateCircle(LineRenderer line){
            //WIP
            line.loop = true;
            line.Simplify(2);
        }

        void UpdateLine(LineRenderer line, Vector3 newFingerPos)
        {
            fingerPositions.Add(newFingerPos);
            line.positionCount++;
            line.SetPosition(line.positionCount - 1, newFingerPos);
        }

        public async void TraceLine(string name)
        {
            LineRenderer line = GameObject.Find(name).GetComponent<LineRenderer>();
            Vector3[] linePos = new Vector3[line.positionCount];
            line.GetPositions(linePos);
            for (int i = 0; i < line.positionCount; i += 2)
            {
                await lowerHandle.MoveToPosition(linePos[i], .1f);
            }
            Debug.Log(linePos[0]);
        }

        public async void FindStartingPoint(string name)
        {
            LineRenderer line = GameObject.Find(name).GetComponent<LineRenderer>();
            Vector3[] linePos = new Vector3[line.positionCount];
            line.GetPositions(linePos);
            await upperHandle.MoveToPosition(linePos[0], .2f);
        }

        public void CombineLines(string name, string addedObject, bool inverted = false)
        {        
            LineRenderer lineTwo = GameObject.Find(addedObject).GetComponent<LineRenderer>();
            Vector3[] lineTwoPos = new Vector3[lineTwo.positionCount];
            lineTwo.GetPositions(lineTwoPos);

            LineRenderer lineOne = GameObject.Find(name).GetComponent<LineRenderer>();

            if (!inverted){
               for(int i = 0; i < lineTwo.positionCount; i++){
                lineOne.positionCount++;
                lineOne.SetPosition(lineOne.positionCount - 1, lineTwoPos[i]);
                } 
            } else {
                for(int i = lineTwo.positionCount-1; i >= 0; i--){
                lineOne.positionCount++;
                lineOne.SetPosition(lineOne.positionCount - 1, lineTwoPos[i]);
                } 
            }
            
            
        }
    }
}