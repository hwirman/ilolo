using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameRate : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		//Screen.SetResolution (1136, 640, true, 0);
			QualitySettings.vSyncCount = 0;
			float w = Screen.width;
			float h = Screen.height;
			int height = Mathf.RoundToInt (1280 * h / w);
			Screen.SetResolution (1280, height, true, 0);
			Application.targetFrameRate = 60;
		}
}
