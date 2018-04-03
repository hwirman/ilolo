using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkBubbleBehavior : MonoBehaviour {
	Rigidbody2D rig;
	Animator anim;
	public float scale;
	public float detectRadius;
	public string color;
	float angle;
	public Vector3 direction;
	public float speed;
	public float maxSpeed;
	//when this inkling was spawned by another inkling
	public bool spawningState;
	float spawningCD = 1;
	bool speedUp;
	bool speedDown;
	// Use this for initialization
	void Start () {
		rig = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
		angle = Random.Range (0, 2 * Mathf.PI);
		GetComponent<SpriteRenderer> ().color = ColorCollection.access.colorDict [color];
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		SpawningStateCheck ();
		if (!speedUp&&!speedDown) {
			CheckPlayerInRange ();
		} 
		transform.localScale = new Vector3 (scale, scale, scale);

		spawningCD = Mathf.Max (0, spawningCD - Time.fixedDeltaTime);
	}

	void SpawningStateCheck(){
		if (spawningState) {
			spawningState = false;
			speedUp = true;
		}
		if (speedUp) {
			if (speed >= maxSpeed) {
				speedUp = false;
				speedDown = true;
			}
			speed = Mathf.Min(maxSpeed,speed + 2*maxSpeed*Time.fixedDeltaTime);
			rig.velocity = direction * speed;
		}
		if (speedDown) {
			if (speed <= 0) {
				speedDown = false;
				spawningState = false;
			}
			speed = Mathf.Max(0,speed - 2*maxSpeed*Time.fixedDeltaTime);
			rig.velocity = direction * speed;
		}
	}
	void CheckPlayerInRange(){
		Collider2D player = Physics2D.OverlapCircle (transform.position, detectRadius,1 << LayerMask.NameToLayer ("Player"));
		if (player == null) {
			speed = Random.Range (1, 5);
			return;
		}
		if (scale > 1.25f && spawningCD<=0) {
			GameObject bubble = Instantiate (Resources.Load ("LargeInklings/InkBubble"), transform.position, transform.rotation) as GameObject;
			InkBubbleBehavior bubbleScript = bubble.GetComponent<InkBubbleBehavior> ();
			if (color == "red") {
				bubbleScript.color = "red";
				bubbleScript.direction = (player.transform.position - transform.position).normalized;
			} else if (color == "blue") {
				bubbleScript.color = "blue";
				bubbleScript.direction = (transform.position-player.transform.position).normalized;
			}
			scale = scale / 2;
			bubbleScript.scale = scale;
			bubbleScript.spawningState = true;
			spawningCD = 1;
		}
	}
}
