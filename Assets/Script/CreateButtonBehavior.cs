using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateButtonBehavior : MonoBehaviour {
	public float Amplitude;
	public float minThreshold;
	public float speed;
	public float startDelay;
	public float paintPauseTime;
	float timeCount;
	bool hasClicked = false;
	ScreenCapture screenCap;
	// Use this for initialization
	void Start () {
		GetComponent<Renderer> ().sharedMaterial.SetFloat ("_Speed", 1);
		screenCap = FindObjectOfType<ScreenCapture> ();
		Invoke ("Create", 20);
	}

	// Update is called once per frame
	void Update () {
		startDelay = Mathf.Max (0, startDelay - Time.deltaTime);

		if (startDelay == 0) {
			timeCount = Mathf.Min (paintPauseTime, timeCount + Time.deltaTime*speed);
			//GetComponent<Renderer> ().sharedMaterial.SetFloat ("_Speed", Mathf.Max (minThreshold, GetComponent<Renderer> ().sharedMaterial.GetFloat ("_Speed") - Time.deltaTime * speed));
			if(timeCount<paintPauseTime)
				GetComponent<Renderer> ().sharedMaterial.SetFloat ("_Speed", ((1-minThreshold)*Mathf.Cos(Mathf.PI * timeCount)+minThreshold+1)/2);
		}
		//GetComponent<Renderer> ().sharedMaterial.SetFloat ("_Speed", minThreshold+Mathf.Sin(Time.time)*Amplitude);
	}
	void Create(){
		if (!hasClicked) {
			hasClicked = true;
			speed = 1;
			paintPauseTime = 2;
			screenCap.OnCreateClick ();
		}
	}
	void OnMouseDown()
	{
		Create ();
	}
}
