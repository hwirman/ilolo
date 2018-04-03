using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkSquareBehavior : LargeInklingBehavior
{
    bool triggered;
    protected override void Start()
    {
        base.Start();
        inkType = "InkSquare";
        //StartCoroutine(AbsorbBoids());
    }
    void FixedUpdate()
    {
        CheckPlayerInRange();
    }
    protected void CheckPlayerInRange()
    {
        Collider2D player = Physics2D.OverlapCircle(transform.position, detectRadius, 1 << LayerMask.NameToLayer("Player"));
        if (player == null)
        {
            return;
        }
        if (!triggered)
        {
            triggered = true;
            InklingController playerScript = player.gameObject.GetComponent<InklingController>();
            //anim.SetTrigger("Shrink");
            MusicManager.access.playSoundEffect("square_split");
            GameObject splat = Instantiate(Resources.Load("Effects/SquareSplat"), transform.position, Quaternion.identity) as GameObject;
            splat.GetComponent<SpriteRenderer>().color = ColorCollection.access.colorDict[color];
            if (ColorCollection.access.camColor != color)
            {
                ColorCollection.access.camColor = color;
                GameObject BGEffect = Instantiate(Resources.Load("Effects/BGEffect"), Camera.main.transform.position + 10 * Vector3.forward, Quaternion.identity) as GameObject;
                BGEffect.transform.parent = Camera.main.transform;
                BGEffect.GetComponent<SpriteRenderer>().color = ColorCollection.access.colorDict["BG" + color];
            }
            Destroy();
        }

    }
    public override void Destroy()
    {
        base.Destroy();
        triggered = false;
    }
}
