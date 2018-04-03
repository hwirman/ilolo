using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionMarkBehavior : MonoBehaviour {

	// Use this for initialization
	void Start () {
		RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero,1 << LayerMask.NameToLayer("TextureMask"));
		if (hit.collider == null) {
			Destroy (gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
