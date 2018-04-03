using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkBarBehavior : MonoBehaviour {
	public float percentage;
	public BrushTrail brush;
	Vector3 scale;
	// Use this for initialization
	void Start () {
		scale = transform.localScale;	
	}
	
	// Update is called once per frame
	void Update () {
		//percentage = GameobjectPooler.current.queueInks.Count / GameobjectPooler.current.queueInkAmount;
		percentage = (100-brush.distance)*0.01f;
		transform.localScale = new Vector3(scale.x*percentage,scale.y,scale.z);
	}
}
