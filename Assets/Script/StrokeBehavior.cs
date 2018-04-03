using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrokeBehavior : MonoBehaviour
{
    public InklingController inklingController;
    LineRenderer lineRenderer;


    // Use this for initialization
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        lineRenderer.material.color = ColorCollection.access.colorDict[inklingController.color];
    }
}
