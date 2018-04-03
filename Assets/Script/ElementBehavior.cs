using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ElementBehavior : MonoBehaviour
{
    public ColorCollection colorCollection;
    public string color;
    public Color colorRGB;
    public Rigidbody2D rig;
    public GameObject player;
    public InklingController inklingScript;
    public SpriteRenderer inSp;
    public SpriteRenderer outSp;

    public GameObject waveAnimation;
    protected Animator anim;

    public Sprite originalSprite;
    public TrailRenderer trail;
    public TrailRenderer outline;
    private Vector3 randomDirection;
    //flocking
    public float flockingSpeed;
    float rotationSpeed = 4.0f;
    Vector3 averageHeading;
    Vector3 averagePosition;
    Vector3 direction = Vector3.right;
    float coheDistance = 4.0f;
    bool turning = false;
    bool Touched = false;
    public Vector3 head;
    public bool alwaysFollow;
    //detect range for flocking
    public float boidRadius;

    public float maxFlockingSpeed = 12;
    //steering behavior
    public bool hasGoal;
    public float steerCD = 0;
    public Vector3 goalPos;
    protected virtual void Awake()
    {
        colorCollection = FindObjectOfType<ColorCollection>();
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    // Use this for initialization
    protected virtual void Start()
    {
        inSp.color = colorCollection.colorDict[color];
        trail.startColor = colorCollection.colorDict[color];
        trail.endColor = colorCollection.colorDict[color];
        outSp.color = colorCollection.colorDict["dark" + color];
        outline.startColor = colorCollection.colorDict["dark" + color];
        outline.endColor = colorCollection.colorDict["dark" + color];
        flockingSpeed = Random.Range(1, 5);
        //generate random direction
        float angle = Random.Range(0, 2 * Mathf.PI);
        randomDirection = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0);
        InvokeRepeating("flock", 0, 0.2f);
    }

    // Update is called once per frame
    protected virtual void FixedUpdate()
    {

        if (Vector3.Distance(transform.position, player.transform.position) >= Mathf.Sqrt(2) * GlobalFlock.access.playgroundSize)
        {
            Destroy();
        }

        AlignRotation();
        //rig.velocity = direction * flockingSpeed;
        //rig.MovePosition(transform.position + transform.up * Time.deltaTime * flockingSpeed);
        transform.Translate(0, Time.deltaTime * flockingSpeed, 0);
        CDCoolDown();
        if (GameManager.access.persona == Persona.aggressive)
        {
            maxFlockingSpeed = 25;
        }
        else
        {
            maxFlockingSpeed = 12;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "TouchInk" && !Touched)
        {
            Touched = true;
            GameObject touchWave = Instantiate(Resources.Load("TouchWave"), transform.position, Quaternion.identity) as GameObject;
            touchWave.GetComponent<SpriteRenderer>().color = ColorCollection.access.colorDict[color];
            other.gameObject.GetComponent<CircleCollider2D>().enabled = false;
            touchWave.GetComponent<SortingGroup>().sortingOrder = other.GetComponent<SortingGroup>().sortingOrder + 1;
            MusicManager.access.playSoundEffect("splash_0" + Random.Range(1, 5).ToString());
            Destroy();
        }
        if (other.tag == "StarSpread" && !Touched)
        {
            Touched = true;
            GameObject splat = Instantiate(Resources.Load("Effects/BoidSplat"), transform.position, Quaternion.identity) as GameObject;
            splat.GetComponent<SpriteRenderer>().color = ColorCollection.access.colorDict["dark" + color];
            GameObject flower = Instantiate(Resources.Load("Effects/StarFlower"), transform.position, Quaternion.identity) as GameObject;
            Color fcolor = ColorCollection.access.colorDict[color];
            flower.GetComponent<SpriteRenderer>().color = fcolor;
            flower.GetComponent<StarFlowerBehavior>().direction = (transform.position - other.transform.position).normalized;
            MusicManager.access.playSoundEffect("star_plant_growing");
            Destroy();
        }
    }
    void AlignRotation()
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), rotationSpeed * Time.deltaTime);
        head = transform.up.normalized;
        //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    void CDCoolDown()
    {
        float playerDist = Vector3.Distance(player.transform.position, this.transform.position);
        // if (playerDist < 5)
        // {
        //     steerCD = 0;
        // }
        steerCD = Mathf.Max(0, steerCD - Time.fixedDeltaTime);
    }
    public void Destroy()
    {
        steerCD = 0;
        Touched = false;
        float r = Random.Range(GlobalFlock.access.playgroundSize, Mathf.Sqrt(2) * GlobalFlock.access.playgroundSize);
        float angle = Random.Range(0, 2 * Mathf.PI);
        trail.Clear();
        outline.Clear();
        transform.position = new Vector3(player.transform.position.x + r * Mathf.Sin(angle), player.transform.position.y + r * Mathf.Cos(angle), 0);
    }
    void steer()
    {
        direction = (goalPos - transform.position).normalized;
        float dis = Vector3.Distance(goalPos, transform.position);
        if (dis > 5)
        {
            flockingSpeed = Map(10, 20, 0, 100, dis);
        }
        else
        {
            flockingSpeed = Map(1, 10, 0, 5, dis);
        }
    }
    void flock()
    {
        if (steerCD > 0)
        {
            steer();
            return;
        }
        Collider2D[] others = Physics2D.OverlapCircleAll(transform.position, boidRadius, 1 << LayerMask.NameToLayer("Boid"));
        Collider2D[] waves = Physics2D.OverlapCircleAll(transform.position, 30.0f, 1 << LayerMask.NameToLayer("Wave"));
        Collider2D[] largeinklings = Physics2D.OverlapCircleAll(transform.position, 30.0f, 1 << LayerMask.NameToLayer("LargeInkling"));
        //RaycastHit2D hit = Physics2D.Raycast(transform.position, head, 5.0f, 1 << LayerMask.NameToLayer("Block"));
        //RaycastHit2D hitR = Physics2D.Raycast(transform.position, transform.right.normalized, 3.0f, 1 << LayerMask.NameToLayer("Block"));
        //RaycastHit2D hitL = Physics2D.Raycast(transform.position, -transform.right.normalized, 3.0f, 1 << LayerMask.NameToLayer("Block"));
        Vector3 center = Vector3.zero;
        Vector3 avoid = Vector3.zero;
        Vector3 heading = Vector3.zero;
        Vector3 playerBlock = Vector3.zero;
        Vector3 waveBlock = Vector3.zero;
        Vector3 largelinklingBlock = Vector3.zero;
        float gSpeed = 0;
        float pSpeed = 0;
        float bSpeed = 0;
        float fSpeed = 0;
        int groupSize = 0;
        //		foreach (Collider2D wave in waves) {
        //			float dist = Vector3.Distance (wave.transform.position, this.transform.position);
        //			if (inklingScript.color == color) {
        //				if (dist < 20.0f) {
        //					waveBlock = (wave.transform.position - transform.position) / dist;
        //					bSpeed = 10.0f / (dist / 2);
        //				}
        //				} else {
        //					if(dist < 10.0f){
        //						waveBlock = (transform.position - wave.transform.position) / dist;
        //						bSpeed = 10.0f/(dist);
        //					}
        //				}
        //		}
        foreach (Collider2D largeinklingCollider in largeinklings)
        {
            GameObject largeinkling = largeinklingCollider.gameObject;
            LargeInklingBehavior largeInklingBehavior = largeinkling.GetComponent<LargeInklingBehavior>();
            if (largeInklingBehavior.color == color)
            {
                float dist = Vector3.Distance(largeinkling.transform.position, this.transform.position);
                if (dist < 20)
                {
                    largelinklingBlock = (largeinkling.transform.position - transform.position) / dist;
                    if (dist <= 5)
                    {
                        largelinklingBlock = Vector3.zero;
                    }
                }

            }
        }
        foreach (Collider2D other in others)
        {
            GameObject anotherBoid = other.gameObject;
            ElementBehavior boidScript = anotherBoid.GetComponent<ElementBehavior>();
            if (boidScript.color != color || anotherBoid == this.gameObject)
            {
                continue;
            }
            float dist = Vector3.Distance(anotherBoid.transform.position, this.transform.position);


            center += anotherBoid.transform.position;
            groupSize++;

            if (dist < 2.0f)
            {
                avoid = avoid + (transform.position - anotherBoid.transform.position) / dist;
            }
            if (dist < 3.0f)
            {
                heading = heading + boidScript.head / dist;
            }
            gSpeed = gSpeed + boidScript.flockingSpeed;
        }
        if (center == Vector3.zero)
        {
            center = randomDirection;
            gSpeed = 5;
        }

        float playerDist = Vector3.Distance(player.transform.position, this.transform.position);
        if (GlobalFlock.access.followPlayerCD > 0 && GlobalFlock.access.followColor == color && playerDist > 10)
        {
            waveAnimation.SetActive(true);
            waveAnimation.GetComponent<SpriteRenderer>().color = colorCollection.colorDict[color];
            playerBlock = (player.transform.position - transform.position) / playerDist;
            pSpeed = 24;
        }
        else
        {
            waveAnimation.SetActive(false);
            if (inklingScript.color == color)
            {
                if (playerDist < 30f)
                {
                    playerBlock = (player.transform.position - transform.position) / playerDist;
                    pSpeed = 24;
                    if (playerDist < 8)
                    {
                        playerBlock = Vector3.zero;
                        pSpeed = 5;
                    }
                }
            }
            else
            {
                if (playerDist < 10.0f)
                {
                    playerBlock = (transform.position - player.transform.position) / playerDist;
                    pSpeed = 10 / (playerDist);
                }
            }
        }
        if (groupSize > 0)
        {
            center = center / groupSize - transform.position;
            gSpeed = gSpeed / groupSize;

        }
        // if (hit.collider != null)
        // {
        //     float dist = Vector3.Distance(hit.point, this.transform.position);
        //     waveBlock += (transform.position - (Vector3)hit.point);
        //     pSpeed = 3;
        //     //Debug.DrawLine(transform.position, (Vector3)hit.point, Color.red);
        // }
        // if (hitR.collider != null)
        // {
        //     float dist = Vector3.Distance(hitR.point, this.transform.position);
        //     waveBlock += (transform.position - (Vector3)hitR.point);
        //     pSpeed = 3;
        //     //Debug.DrawLine(transform.position, (Vector3)hitR.point, Color.red);
        // }
        // if (hitL.collider != null)
        // {
        //     float dist = Vector3.Distance(hitL.point, this.transform.position);
        //     waveBlock += (transform.position - (Vector3)hitL.point);
        //     pSpeed = 3;
        // }
        flockingSpeed = Mathf.Min(maxFlockingSpeed, (gSpeed + pSpeed) / 2);
        flockingSpeed = Mathf.Max(flockingSpeed, 5);
        direction = (center.normalized + avoid + heading + playerBlock * 8 + largelinklingBlock * 5 + waveBlock * 0).normalized;
    }
    public float Map(float from, float to, float from2, float to2, float value)
    {
        if (value <= from2)
        {
            return from;
        }
        else if (value >= to2)
        {
            return to;
        }
        else
        {
            return (to - from) * ((value - from2) / (to2 - from2)) + from;
        }
    }
}
