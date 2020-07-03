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

        private float lowerRotation;

        public LineRenderer lineRenderer;

        public List<Vector3> fingerPositions;
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
            Debug.Log(lowerRotation);
            if(Input.GetMouseButtonDown(0) || Mathf.Abs(lowerRotation - lowerHandle.GetRotation()) > 70)
            {
                CreateLine();
            }
            lowerRotation = lowerHandle.GetRotation();
            Debug.Log(lowerRotation);
            if(Input.GetMouseButton(0) || Mathf.Abs(lowerRotation - lowerHandle.GetRotation()) > 70)
            {
                Vector3 tempFingerPos = upperHandle.HandlePosition(transform.position);
                tempFingerPos.y = .1f;
                //Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 latestPos = fingerPositions[fingerPositions.Count - 1];
                latestPos.y = .1f;
                if(Vector3.Distance(tempFingerPos, latestPos) > .1f)
                {
                    UpdateLine(tempFingerPos);
                }
            }
        }

        void CreateLine()
        {   
            currentLine = Instantiate(linePrefab, Vector3.zero, Quaternion.identity);
            lineRenderer = currentLine.GetComponent<LineRenderer>();
            fingerPositions.Clear();
            Vector3 newPos = upperHandle.HandlePosition(transform.position);
            Debug.Log(newPos);
            newPos.y = .1f;
            fingerPositions.Add(newPos);
            fingerPositions.Add(newPos);
            lineRenderer.SetPosition(0, fingerPositions[0]);
            lineRenderer.SetPosition(1, fingerPositions[1]);
        }

        void CreateCircle(LineRenderer line){
            //WIP
            line.loop = true;
            line.Simplify(1);
        }

        void UpdateLine(Vector3 newFingerPos)
        {
            fingerPositions.Add(newFingerPos);
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, newFingerPos);
        }

        public async void TraceLine(string name)
        {
            LineRenderer line = GameObject.Find(name).GetComponent<LineRenderer>();
            Vector3[] linePos = new Vector3[line.positionCount];
            line.GetPositions(linePos);
            for (int i = 0; i < line.positionCount; i += 2)
            {
                await lowerHandle.MoveToPosition(linePos[i], .2f);
            }
            Debug.Log(linePos[0]);
        }
    }
}