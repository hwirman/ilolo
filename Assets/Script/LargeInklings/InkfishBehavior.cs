using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkfishBehavior : LargeInklingBehavior
{
    float scale;

    public float maxScale = 5;
    float boidNum = 0;
    public float scaleIncrement = 0.5f;
    float scaleX;
    float scaleY;
    public float speed;
    public float maxSpeed;
    float angle;
    Vector3 direction;
    bool speedUp;
    bool speedDown;
    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        inkType = "InkFish";
        scale = maxScale;
        scaleX = scale;
        scaleY = scale;
        angle = Random.Range(0, 2 * Mathf.PI);
        direction = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0);
        StartCoroutine(AbsorbBoids());
    }
    void FixedUpdate()
    {
        if (!speedUp && !speedDown)
        {
            CheckPlayerInRange();
        }
        if (speedUp)
        {
            if (speed >= maxSpeed)
            {
                speedUp = false;
                speedDown = true;
            }
            speed = Mathf.Min(maxSpeed, speed + 2 * maxSpeed * Time.fixedDeltaTime);

        }
        if (speedDown)
        {
            if (speed <= 0)
            {
                speedDown = false;
            }
            speed = Mathf.Max(0, speed - 2 * maxSpeed * Time.fixedDeltaTime);

        }

        rig.velocity = direction * speed;
        AlignRotation();
        AlignSizeWithSpeed();
    }
    void AlignSizeWithSpeed()
    {
        if (speedUp || speedDown)
        {
            scaleX = scale - scale * 0.2f * Mathf.Cos(1.5f * Mathf.PI * (speed / maxSpeed) + Mathf.PI / 2);
            scaleY = scale + scale * 0.2f * Mathf.Cos(1.5f * Mathf.PI * (speed / maxSpeed) + Mathf.PI / 2);
        }
        else
        {
            scaleX = scale;
            scaleY = scale;
        }
        transform.localScale = new Vector3(scaleX, scaleY, scale);
    }
    void AlignRotation()
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    protected void CheckPlayerInRange()
    {
        Collider2D player = Physics2D.OverlapCircle(transform.position, detectRadius, 1 << LayerMask.NameToLayer("Player"));
        Collider2D touchWave = null;//Physics2D.OverlapCircle(transform.position, detectRadius, 1 << LayerMask.NameToLayer("Wave"));
        if (player == null && touchWave == null)
        {
            speed = 3;
            return;
        }
        InklingController inklingScript = GlobalFlock.access.player.GetComponent<InklingController>();

        // if (inklingScript.color != color)
        // {
        if (player != null)
        {
            EscapeFrom(player.transform.position);
        }
        else
        {
            EscapeFrom(touchWave.transform.position);
        }
        Invoke("ReleaseInk", 0.33f);
        // }
    }
    IEnumerator AbsorbBoids()
    {

        Collider2D[] boids = Physics2D.OverlapCircleAll(transform.position, detectRadius, 1 << LayerMask.NameToLayer("Boid"));
        if (boids.Length != 0)
        {
            float numInRange = 0;
            foreach (Collider2D boid in boids)
            {
                ElementBehavior boidScript = boid.GetComponent<ElementBehavior>();
                if (boidScript.color == color && scale < maxScale)
                {
                    boidScript.Destroy();
                    numInRange += 1;
                }
            }
            if (numInRange > 0)
            {
                float grownSize = Mathf.Min(maxScale, scale + numInRange * scaleIncrement);
                while (scale < grownSize * 1.2f)
                {
                    scale += scaleIncrement;
                    yield return new WaitForFixedUpdate();
                }
                while (scale > grownSize)
                {
                    scale -= scaleIncrement;
                    yield return new WaitForFixedUpdate();
                }
            }
            boidNum += numInRange;
        }

        yield return new WaitForSeconds(0.2f);
        StartCoroutine(AbsorbBoids());

    }
    void ReleaseInk()
    {
        MusicManager.access.playSoundEffect("Inkfish", transform.position, 1);
        GameObject splatParticle = Instantiate(Resources.Load("Effects/SplatParticle"), transform.position, transform.rotation) as GameObject;
        splatParticle.GetComponent<SplatParticleBehavior>().color = color;
        GameObject splat = Instantiate(Resources.Load("Effects/Splat"), transform.position, transform.rotation) as GameObject;
        splat.GetComponent<SplatBehavior>().color = color;
        scale = Mathf.Max(1, scale - 1);
        detectRadius = scale;
    }
    void EscapeFrom(Vector3 playerPos)
    {
        speedUp = true;
        direction = (transform.position - playerPos).normalized;
    }
}
