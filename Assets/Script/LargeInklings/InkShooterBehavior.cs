using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkShooterBehavior : LargeInklingBehavior
{
    float angle;
    public Vector3 direction;
    float shootCD;
    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        inkType = "InkShooter";
        angle = Random.Range(0, 2 * Mathf.PI);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckPlayerInRange();
        shootCD = Mathf.Max(0, shootCD - Time.fixedDeltaTime);
    }

    protected void CheckPlayerInRange()
    {
        Collider2D player = Physics2D.OverlapCircle(transform.position, detectRadius, 1 << LayerMask.NameToLayer("Player"));
        if (player == null)
        {
            return;
        }
        InklingController playerScript = player.gameObject.GetComponent<InklingController>();
        if (playerScript.color == color)
        {
            return;
        }
        if (shootCD <= 0)
        {
            GameObject inkProjectile = Instantiate(Resources.Load("InkProjectile"), transform.position, Quaternion.identity) as GameObject;
            InkProjectileBehavior projectileScript = inkProjectile.GetComponent<InkProjectileBehavior>();
            projectileScript.direction = (player.transform.position - transform.position).normalized;
            projectileScript.color = color;
            shootCD = 1f;
        }

    }
}
