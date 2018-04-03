using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrushBehavior : MonoBehaviour
{
    List<Vector3> positions = new List<Vector3>();
    Vector3 lastPosition;
    LineRenderer lineRenderer;
    GameObject lineStroke;
    // Use this for initialization
    void Start()
    {
        lineStroke = Instantiate(Resources.Load("DrawStroke"), Vector3.zero, Quaternion.identity) as GameObject;
        lineRenderer = lineStroke.GetComponent<LineRenderer>();
        Color strokeColor = ColorCollection.access.colorDict[PlayerPrefs.GetString("CreationColor")];
        lineRenderer.startColor = strokeColor;
        lineRenderer.endColor = strokeColor;
        lastPosition = transform.position; lastPosition.z = 0;
        //positions.Add(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(lastPosition, transform.position) > 0.05f)
        {
            lastPosition = transform.position; lastPosition.z = 0;
            positions.Add(lastPosition);

        }
        lineRenderer.positionCount = positions.Count;
        lineRenderer.SetPositions(positions.ToArray());
    }
}
