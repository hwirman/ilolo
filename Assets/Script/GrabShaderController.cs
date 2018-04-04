using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabShaderController : MonoBehaviour
{
    public MenuManager menu;
    //public float Amplitude;
    public float minThreshold;
    public float speed;
    public float startDelay;
    public float paintPauseTime;
    float timeCount;
    // Use this for initialization
    void Start()
    {
        GetComponent<Renderer>().sharedMaterial.SetFloat("_Speed", 1);
        menu = FindObjectOfType<MenuManager>();
    }

    // Update is called once per frame
    void Update()
    {
        startDelay = Mathf.Max(0, startDelay - Time.deltaTime);

        if (startDelay == 0)
        {
            timeCount = Mathf.Min(paintPauseTime, timeCount + Time.deltaTime * speed);
            //GetComponent<Renderer> ().sharedMaterial.SetFloat ("_Speed", Mathf.Max (minThreshold, GetComponent<Renderer> ().sharedMaterial.GetFloat ("_Speed") - Time.deltaTime * speed));
            if (timeCount < paintPauseTime)
                GetComponent<Renderer>().sharedMaterial.SetFloat("_Speed", ((1 - minThreshold) * Mathf.Cos(Mathf.PI * timeCount) + minThreshold + 1) / 2);
        }

        if (menu.fadeToScene == true)
        {
            speed = 1;
            paintPauseTime = 2;
        }
        //GetComponent<Renderer> ().sharedMaterial.SetFloat ("_Speed", minThreshold+Mathf.Sin(Time.time)*Amplitude);
    }
}
