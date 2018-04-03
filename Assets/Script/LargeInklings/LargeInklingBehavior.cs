using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LargeInklingBehavior : MonoBehaviour
{
    public string color;
    protected Rigidbody2D rig;
    protected Animator anim;
    public float detectRadius;
    public string inkType = "";
    public bool canRespawn = true;
    bool analyticsDataSent = false;
    // Use this for initialization
    protected virtual void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        GetComponent<SpriteRenderer>().color = ColorCollection.access.colorDict[color];
    }

    protected virtual void Update()
    {
        if (Vector3.Distance(transform.position, GlobalFlock.access.player.transform.position) >= 3 * GlobalFlock.access.playgroundSize)
        {
            Destroy();
        }
    }
    public virtual void Destroy()
    {
        if (canRespawn)
        {
            float r = Random.Range(GlobalFlock.access.playgroundSize, 3 * GlobalFlock.access.playgroundSize);
            float angle = Random.Range(0, 2 * Mathf.PI);
            transform.position = new Vector3(GlobalFlock.access.player.transform.position.x + r * Mathf.Sin(angle), GlobalFlock.access.player.transform.position.y + r * Mathf.Cos(angle), 0);
            Collider2D[] others = Physics2D.OverlapCircleAll(transform.position, 30, 1 << LayerMask.NameToLayer("LargeInkling"));
            if (others.Length > 1)
            {
                //Debug.Log("Yes");
                Destroy();
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
