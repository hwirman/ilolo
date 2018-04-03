using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkCamera : LargeInklingBehavior
{
    bool captured = false;
    protected override void Start()
    {
        base.Start();
        inkType = "InkCamera";
        InvokeRepeating("TakePicture", 2, 0.05f);
    }
    public override void Destroy()
    {
        base.Destroy();
        captured = false;
    }
    void TakePicture()
    {
        Collider2D player = Physics2D.OverlapCircle(transform.position, detectRadius, 1 << LayerMask.NameToLayer("Player"));
        if (player != null && !captured)
        {
            captured = true;
            InklingController inklingController = player.gameObject.GetComponent<InklingController>();
            anim.SetTrigger("TakeShot");
            UnityEngine.ScreenCapture.CaptureScreenshot("screenshot.png");
            Invoke("Destroy", 1);
        }
    }
}

