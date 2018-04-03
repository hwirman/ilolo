using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionButtonBehavior : MonoBehaviour {
	public GameObject OptionMenu;
	// Use this for initialization
	void Start () {
		
	}
	void OnMouseDown()
	{
		if(OptionMenu.activeInHierarchy) 
			OptionMenu.SetActive (false);
		else
			OptionMenu.SetActive (true);
	}
	// Update is called once per frame
	void Update () {
		
	}
}
