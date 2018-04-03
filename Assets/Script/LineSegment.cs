using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineSegment : MonoBehaviour {
	LineRenderer linerenderer;
	public float maxWidth;
	public float width;
	public float expandSpeed;
	// Use this for initialization
	void Start () {
		linerenderer = GetComponent<LineRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		width += expandSpeed * Time.deltaTime;
		width = Mathf.Min (width, maxWidth);
		//linerenderer.SetWidth (width,width);

		linerenderer.widthMultiplier = width;
	}
}
