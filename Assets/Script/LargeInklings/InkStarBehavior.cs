using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkStarBehavior : LargeInklingBehavior
{
    bool triggered;
    Vector3 direction;
    public float speed;
    protected override void Start()
    {
        base.Start();
        inkType = "InkStar";
        float angle = Random.Range(0, 2 * Mathf.PI);
        direction = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0);
        speed = Random.Range(3, 5);
    }
    void FixedUpdate()
    {
        CheckPlayerInRange();
        if (!triggered)
        {
            transform.position += direction * speed * Time.fixedDeltaTime;
        }
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
            InklingController playerScript = player.gameObject.GetComponent<InklingController>();
            GameObject spread = Instantiate(Resources.Load("Effects/StarSpread"), transform.position, Quaternion.identity) as GameObject;
            spread.GetComponent<SpriteRenderer>().color = ColorCollection.access.colorDict[color];
            GameObject splat = Instantiate(Resources.Load("Effects/StarSplat"), transform.position, Quaternion.identity) as GameObject;
            splat.GetComponent<SpriteRenderer>().color = ColorCollection.access.colorDict[color];
            MusicManager.access.playSoundEffect("star_split");
            Destroy();
        }

    }
    public override void Destroy()
    {
        base.Destroy();
        triggered = false;
    }
}
