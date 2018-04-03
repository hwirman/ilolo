using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGEffectBehavior : MonoBehaviour
{

    // Use this for initialization
    public float time = 1;
    // Use this for initialization
    void Start()
    {
        Destroy(gameObject, time);
    }

    public void ChangeBGColor()
    {
        Camera.main.transform.GetComponent<Camera>().backgroundColor = ColorCollection.access.colorDict["BG" + ColorCollection.access.camColor];
        MusicManager.access.ChangeBGM(ColorCollection.access.camColor);
    }
}
