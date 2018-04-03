using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkAcceleratorBehavior : LargeInklingBehavior
{
    protected override void Start()
    {
        base.Start();
        inkType = "InkAccelerator";
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            InklingController inklingController = other.gameObject.GetComponent<InklingController>();
            if (inklingController.color == color)
            {
                anim.SetTrigger("Spark");
            }
        }
    }
}
