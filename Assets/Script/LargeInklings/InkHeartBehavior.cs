using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkHeartBehavior : LargeInklingBehavior
{
    bool triggered;
    protected override void Start()
    {
        base.Start();
        inkType = "InkHeart";
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
            InklingController playerScript = player.gameObject.GetComponent<InklingController>();
            GameObject wave = Instantiate(Resources.Load("Effects/AttractWave"), player.transform.position, Quaternion.identity) as GameObject;
            Color wavecolor = ColorCollection.access.colorDict[color]; wavecolor.a = 0.6f;
            wave.GetComponent<SpriteRenderer>().color = wavecolor;
            wave.transform.parent = player.transform;
            MusicManager.access.playSoundEffect("heart_split");
            SendSignal();
        }

    }
    public override void Destroy()
    {
        base.Destroy();
        triggered = false;
    }
    public void SendSignal()
    {
        GameObject splat = Instantiate(Resources.Load("Effects/HeartSplat"), transform.position, Quaternion.identity) as GameObject;
        splat.GetComponent<SpriteRenderer>().color = ColorCollection.access.colorDict[color];
        GlobalFlock.access.followPlayerCD = 8;
        GlobalFlock.access.followColor = color;
        Destroy();
    }
}
