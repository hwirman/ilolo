using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InklingController : MonoBehaviour
{
    Rigidbody2D playerRig;
    public Vector3 direction;
    public Vector3 lastDirection;
    Vector3 bounceDirection;
    public float baseSpeed = 10;
    public float speed = 10;
    public float trackSpeed = 10;
    float currentSpeed = 0;
    public string color = "";
    public Color colorRGB;
    //ColorCollection colorCollection;
    public AudioSource bellSound;

    Transform leftbody;
    Transform rightbody;
    Transform leftEmitter;
    Transform rightEmitter;
    public SpriteRenderer waveSPL;
    public SpriteRenderer waveSPR;
    public SpriteRenderer upSP;
    public SpriteRenderer downSP;
    public TrailRenderer upTrail;
    public TrailRenderer downTrail;
    SpriteRenderer leftSP;
    SpriteRenderer rightSP;
    Texture2D left;
    Texture2D right;

    //CD
    public float stayCD = 0;
    public float accelerateCD = 0;
    public float bouncebackCD = 0;

    // Use this for initialization

    void Awake()
    {


        leftbody = transform.Find("leftbody");
        rightbody = transform.Find("rightbody");
        left = PNGSave.LoadPNG(Application.persistentDataPath + "/leftBody.png");
        Sprite leftSprite = Sprite.Create(left, new Rect(0.0f, 0.0f, left.width, left.height), new Vector2(1, 0.5f));
        leftbody.GetComponent<SpriteRenderer>().sprite = leftSprite;

        right = PNGSave.LoadPNG(Application.persistentDataPath + "/rightBody.png");
        Sprite rightSprite = Sprite.Create(right, new Rect(0.0f, 0.0f, right.width, right.height), new Vector2(0, 0.5f));
        rightbody.GetComponent<SpriteRenderer>().sprite = rightSprite;
        AppyColorChange();
    }
    void Start()
    {

        //colorCollection = ColorCollection.access;
        // speed = baseSpeed;
        playerRig = GetComponent<Rigidbody2D>();
        leftSP = leftbody.GetComponent<SpriteRenderer>();
        rightSP = rightbody.GetComponent<SpriteRenderer>();
        color = PlayerPrefs.GetString("CreationColor");

    }

    void GetmoveDirection()
    {
        if (bouncebackCD > 0)
        {
            direction = -5 * bounceDirection;
        }
        else
        {
            direction = GuideBehavior.access.FDirection();
        }
        if (direction.magnitude != 0)
        {
            lastDirection = direction;
        }
    }

    void Move()
    {

        if (accelerateCD > 0)
        {
            //speed = Mathf.Min(baseSpeed * 3, Mathf.SmoothDamp(speed, trackSpeed, ref currentSpeed, 0.5f));
            //speed = trackSpeed;
            // speed = Mathf.SmoothDamp(speed, baseSpeed * 2, ref currentSpeed, 0.2f);
            speed = Mathf.Min(baseSpeed * 2f, speed + 3f * Time.fixedDeltaTime);
        }
        else if (bouncebackCD > 0)
        {
            speed = Mathf.SmoothDamp(speed, 0, ref currentSpeed, 0.5f);
        }
        else
        {
            speed = Mathf.SmoothDamp(speed, baseSpeed, ref currentSpeed, 0.5f);
        }
        //playerRig.velocity = speed * direction;
        //playerRig.MovePosition(transform.position + direction * speed * Time.fixedDeltaTime);

        if (direction.x == 0 && direction.y == 0)
        {
            speed = 0;
            stayCD = Mathf.Max(0, stayCD + Time.fixedDeltaTime);
            AnalyseManager.access.quietScore += Time.fixedDeltaTime;
        }
        else
        {
            stayCD = 0;
        }
        transform.Translate(0, speed * Time.fixedDeltaTime, 0);
    }
    void Flap()
    {
        leftbody.localEulerAngles = new Vector3(0, Mathf.Sin(Mathf.PI * Time.time) * 30 - 30, 0);
        rightbody.localEulerAngles = new Vector3(0, -Mathf.Sin(Mathf.PI * Time.time) * 30 + 30, 0);

    }
    void AppyColorChange()
    {

        //colorRGB = ColorCollection.access.colorDict[color];
        for (int y = 0; y < left.height; y++)
        {
            for (int x = 0; x < left.width; x++)
            {
                if (left.GetPixel(x, y).a > 0)
                    left.SetPixel(x, y, Color.white);
            }
        }
        left.Apply();
        for (int y = 0; y < left.height; y++)
        {
            for (int x = 0; x < left.width; x++)
            {
                if (right.GetPixel(x, y).a > 0)
                    right.SetPixel(x, y, Color.white);
            }
        }
        right.Apply();
    }

    void CDTimer()
    {
        accelerateCD = Mathf.Max(0, accelerateCD - Time.fixedDeltaTime);
        bouncebackCD = Mathf.Max(0, bouncebackCD - Time.fixedDeltaTime);
    }
    void AlignRotation()
    {
        if (lastDirection.y == 0 && lastDirection.x == 0)
        {
            transform.rotation = Quaternion.AngleAxis(0, Vector3.forward);
        }
        else
        {
            float angle = Mathf.Atan2(lastDirection.y, lastDirection.x) * Mathf.Rad2Deg - 90;

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), 5 * Time.deltaTime);
        }
    }
    void FuzzyRotation()
    {
        if (direction.x == 0 && direction.y == 0)
        {
            float angle = Mathf.Atan2(lastDirection.y, lastDirection.x) * Mathf.Rad2Deg - 90;

            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), 5 * Time.deltaTime);
        }
        else
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), 5 * Time.deltaTime);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Splat")
        {
            SplatBehavior splatScript = other.gameObject.GetComponent<SplatBehavior>();
            if (splatScript.color != color)
            {
                color = splatScript.color;
                AppyColorChange();
            }
        }
        if (other.tag == "InkAccelerator")
        {
            InkAcceleratorBehavior script = other.gameObject.GetComponent<InkAcceleratorBehavior>();
            if (script.color == color)
            {
                accelerateCD = 2;
            }
            else
            {
                bounceDirection = direction;
                bouncebackCD = 0.5f;
            }

        }
        if (other.tag == "InkShooter")
        {
            InkShooterBehavior inkshooterBehavior = other.gameObject.GetComponent<InkShooterBehavior>();
            if (inkshooterBehavior.color != color)
            {
                color = inkshooterBehavior.color;
                AppyColorChange();
            }
        }
        if (other.tag == "InkBouncer")
        {
            bounceDirection = direction;
            bouncebackCD = 0.5f;
        }
    }
    void Update()
    {
        colorRGB = ColorCollection.access.colorDict["dark" + color];
        Color normal = ColorCollection.access.colorDict[color];
        Color dark = ColorCollection.access.colorDict["dark" + color];
        Color waveColor = dark;// waveColor.a = Mathf.Sin(Mathf.PI * Time.time) * 0.15f + 0.3f;
        leftSP.color = dark;
        rightSP.color = dark;
        upSP.color = normal;
        downSP.color = dark;
        upTrail.startColor = normal;
        upTrail.endColor = normal;
        downTrail.startColor = dark;
        downTrail.endColor = dark;
        waveSPL.color = waveColor;
        waveSPR.color = waveColor;

        if (AnalyseManager.access.persona == Persona.aggressive)
        {
            baseSpeed = 25;
        }
        else
        {
            baseSpeed = 12;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        GetmoveDirection();
        Move();
        //AlignRotation();
        FuzzyRotation();
        Flap();
        CDTimer();
    }
}
