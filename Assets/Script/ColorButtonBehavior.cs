using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorButtonBehavior : MonoBehaviour
{
    public string color;
    // Use this for initialization
    // Update is called once per frame
    void OnMouseDown()
    {
        ColorSceneManager.access.OnColorButtonClicked(color);
    }
}
