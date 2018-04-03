using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplatParticleBehavior : MonoBehaviour {
	public string color;
	// Use this for initialization
	void Start () {
		ParticleSystem.MainModule settings = GetComponent<ParticleSystem>().main;
		settings.startColor = ColorCollection.access.colorDict [color];
	}
}
