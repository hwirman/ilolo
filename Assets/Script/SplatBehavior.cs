using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplatBehavior : MonoBehaviour
{
    public string color;
    // Use this for initialization
    void Start()
    {
        GetComponent<SpriteRenderer>().color = ColorCollection.access.colorDict[color];
        Invoke("DisableTrigger", 0.2f);
    }

    void DisableTrigger()
    {
        GetComponent<CircleCollider2D>().enabled = false;
    }
}
