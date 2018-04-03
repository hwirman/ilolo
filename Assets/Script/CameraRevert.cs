using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRevert : MonoBehaviour {
	
	void OnPreCull () {
		GetComponent<Camera>().ResetWorldToCameraMatrix ();
		GetComponent<Camera>().ResetProjectionMatrix ();
		GetComponent<Camera>().projectionMatrix = GetComponent<Camera>().projectionMatrix * Matrix4x4.Scale(new Vector3 (-1, 1, 1));;
	}

	void OnPreRender () {
		GL.SetRevertBackfacing (true);
	}

	void OnPostRender () {
		GL.SetRevertBackfacing (false);
	}
}
