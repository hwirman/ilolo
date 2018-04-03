using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
public class StartButtonBehavior : MonoBehaviour {
	MenuManager menuManager;
	// Use this for initialization
	void Start () {
		menuManager = FindObjectOfType<MenuManager> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnMouseDown()
	{
		menuManager.OnStartButtonClick ();
		GetComponent<AnalyticsTracker> ().TriggerEvent ();
	}
}
