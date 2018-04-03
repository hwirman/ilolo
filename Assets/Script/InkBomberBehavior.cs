using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkBomberBehavior : LargeInklingBehavior
{
    float size;
    float boidNum = 0;
    bool exploded;
    public float sizeIncrement = 0.5f;
    protected override void Start()
    {
        base.Start();
        inkType = "InkBomber";
        size = transform.localScale.x;
        //StartCoroutine(AbsorbBoids());
    }
    void FixedUpdate()
    {
        CheckPlayerInRange();
        transform.localScale = new Vector3(size, size, size);
        // if (boidNum > 2)
        // {
        //     anim.SetTrigger("Explode");
        // }
    }
    protected void CheckPlayerInRange()
    {
        Collider2D player = Physics2D.OverlapCircle(transform.position, detectRadius, 1 << LayerMask.NameToLayer("Player"));
        if (player == null)
        {
            return;
        }
        if (!exploded)
        {
            exploded = true;
            InklingController playerScript = player.gameObject.GetComponent<InklingController>();
            playerScript.color = color;
            anim.SetTrigger("Explode");
        }
    }
    // IEnumerator AbsorbBoids()
    // {
    //     Collider2D[] boids = Physics2D.OverlapCircleAll(transform.position, detectRadius, 1 << LayerMask.NameToLayer("Boid"));
    //     if (boids.Length != 0)
    //     {
    //         float numInRange = 0;
    //         foreach (Collider2D boid in boids)
    //         {
    //             ElementBehavior boidScript = boid.GetComponent<ElementBehavior>();
    //             if (boidScript.color == color)
    //             {
    //                 boidScript.Destroy();
    //                 numInRange += 1;
    //             }
    //         }
    //         if (numInRange > 0)
    //         {
    //             float grownSize = size + numInRange * sizeIncrement;
    //             while (size < grownSize * 1.2f)
    //             {
    //                 size += sizeIncrement;
    //                 yield return new WaitForFixedUpdate();
    //             }
    //             while (size > grownSize)
    //             {
    //                 size -= sizeIncrement;
    //                 yield return new WaitForFixedUpdate();
    //             }
    //         }
    //         boidNum += numInRange;
    //     }

    //     yield return new WaitForSeconds(0.2f);
    //     StartCoroutine(AbsorbBoids());

    // }
    public override void Destroy()
    {
        base.Destroy();
        boidNum = 0;
        exploded = false;
        anim.SetTrigger("BackToIdle");
    }
    public void ReleaseInk()
    {
        MusicManager.access.playSoundEffect("InkBomber", transform.position, 1);
        GameObject splat = Instantiate(Resources.Load("Effects/LargeSplat"), transform.position, transform.rotation) as GameObject;
        //splat.transform.localScale = new Vector3(10, 10, 10);
        splat.GetComponent<SplatBehavior>().color = color;
    }
}
