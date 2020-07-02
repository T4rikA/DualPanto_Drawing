using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paintable : MonoBehaviour
{

    public GameObject linePrefab;
    public GameObject currentLine;


    UpperHandle upperHandle;
    LowerHandle lowerHandle;

    public LineRenderer lineRenderer;

    public List<Vector2> fingerPositions;
    // Start is called before the first frame update
    void Start()
    {
        upperHandle = GameObject.Find("Panto").GetComponent<UpperHandle>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            CreateLine();
        }
        
        if(Input.GetMouseButton(0))
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
        newPos.y = .1f;
        fingerPositions.Add(newPos);
        fingerPositions.Add(newPos);
        lineRenderer.SetPosition(0, fingerPositions[0]);
        lineRenderer.SetPosition(1, fingerPositions[1]);
    }

    void UpdateLine(Vector3 newFingerPos)
    {
        fingerPositions.Add(newFingerPos);
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, newFingerPos);
    }
}
