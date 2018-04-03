using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkProjectileBehavior : MonoBehaviour {
	Rigidbody2D rig;
	public float speed;
	public Vector3 direction;
	public string color;
	// Use this for initialization
	void Start () {
		rig = GetComponent<Rigidbody2D> ();
		GetComponent<SpriteRenderer> ().color = ColorCollection.access.colorDict [color];
		Invoke ("Explode", 1);
	}
	void FixedUpdate(){
		rig.velocity = speed * direction;
		AlignRotation ();
	}
	void AlignRotation(){
		float angle = Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg-90;
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}
	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Player") {
			Explode ();
		}
	}
	void Explode(){
		GameObject splatParticle = Instantiate (Resources.Load ("Effects/SplatParticle"), transform.position, transform.rotation) as GameObject;
		splatParticle.GetComponent<SplatParticleBehavior> ().color = color;
		GameObject splat = Instantiate (Resources.Load ("Effects/Splat"), transform.position, transform.rotation) as GameObject;
		splat.GetComponent<SplatBehavior> ().color = color;
		Destroy (gameObject);
	}
}
