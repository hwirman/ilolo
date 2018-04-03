using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGTextureBehavior : MonoBehaviour
{
    public Sprite red;
    public Sprite green;
    public Sprite blue;
    public Sprite purple;
    public Sprite orange;
    public Sprite yellow;
    SpriteRenderer sp;
    Transform player;
    float Xdist;
    float Ydist;
    float Xpos;
    float Ypos;
    Vector3 newPos;
    public float maxDist = 20;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        sp = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        sp.color = ColorCollection.access.colorDict["Tex" + ColorCollection.access.camColor];
        sp.sprite = Resources.Load<Sprite>("BGTexture/" + ColorCollection.access.camColor);
        Xdist = player.position.x - transform.position.x;
        Ydist = player.position.y - transform.position.y;
        if (Mathf.Abs(Xdist) <= maxDist && Mathf.Abs(Ydist) <= maxDist)
        {
            return;
        }
        if (Mathf.Abs(Xdist) > maxDist)
        {
            Xpos = Xdist < 0 ? (player.position.x - maxDist) : (player.position.x + maxDist);
            newPos = new Vector3(Xpos, transform.position.y, transform.position.z);
        }
        if (Mathf.Abs(Ydist) > maxDist)
        {
            Ypos = Ydist < 0 ? (player.position.y - maxDist) : (player.position.y + maxDist);
            newPos = new Vector3(transform.position.x, Ypos, transform.position.z);
        }
        transform.position = newPos;

    }
}
