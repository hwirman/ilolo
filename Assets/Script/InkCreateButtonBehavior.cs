using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkCreateButtonBehavior : MonoBehaviour
{
    bool hasClicked;
    ScreenCapture screenCap;
    // Use this for initialization
    void Start()
    {
        screenCap = FindObjectOfType<ScreenCapture>();
        Invoke("Create", 20);
    }
    void Create()
    {
        if (!hasClicked)
        {
            hasClicked = true;
            GetComponent<Animator>().SetTrigger("Clicked");
            screenCap.OnCreateClick();
        }
    }

    void OnMouseDown()
    {
        Create();
    }
}
