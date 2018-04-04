using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using PDollarGestureRecognizer;

public struct Stroke
{
    public List<Vector3> positions;
    public List<float> timeStamps;
    public List<float> speeds;
    public List<float> lengths;
    public float avgSpeed;
    public float length;
}
public struct WayPoint
{
    public float timeStamp;
    public Vector3 position;
}

public class GuideBehavior : MonoBehaviour
{
    public Transform inkling;
    InklingController inklingController;
    public static GuideBehavior access;
    public List<Stroke> Strokes = new List<Stroke>();
    public Stroke newStroke = new Stroke();
    public Stroke currentFollowStroke = new Stroke();

    public List<WayPoint> WayPoints = new List<WayPoint>();
    public float fadeoutTime = 3;
    public float maxWidth;
    public Vector3 fuzzyDirection;
    List<LineRenderer> Strokelines = new List<LineRenderer>();
    List<EdgeCollider2D> strokeColliders = new List<EdgeCollider2D>();
    List<Vector2> newstrokeColliderPoints = new List<Vector2>();
    List<Vector2> currentstrokeColliderPoints = new List<Vector2>();
    public Vector3 currentTarget = Vector3.zero;
    int newFingerTouchID = 0;
    public Material material;

    GameObject newstrokeGameobject;
    LineRenderer newstrokeLine;
    EdgeCollider2D newstrokeCollider;
    LineRenderer currentstrokeLine;
    EdgeCollider2D currentstrokeCollider;

    //pattern recognition

    private List<Gesture> trainingSet = new List<Gesture>();

    private List<Point> points = new List<Point>();

    //for persona analytics
    public float avgSpeed = 0;
    public int speedCount = 0;
    public float sum = 0;
    public float totalLength = 0;

    // Use this for initialization
    void Awake()
    {
        access = this;
    }

    void Start()
    {
        inklingController = inkling.GetComponent<InklingController>();
        TextAsset[] gesturesXml = Resources.LoadAll<TextAsset>("GestureSet/LargeInklings/");
        foreach (TextAsset gestureXml in gesturesXml)
            trainingSet.Add(GestureIO.ReadGestureFromXML(gestureXml.text));

        //Load user custom gestures
        string[] filePaths = Directory.GetFiles(Application.persistentDataPath, "*.xml");
        foreach (string filePath in filePaths)
            trainingSet.Add(GestureIO.ReadGestureFromFile(filePath));

    }
    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began || (Application.isEditor && Input.GetMouseButtonDown(0)))
        {

            Vector3 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            if (Application.isEditor)
            {
                mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            }
            else
            {
                newFingerTouchID = Input.GetTouch(0).fingerId;
                mousePosition = new Vector2(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y);
            }
            Vector3 touchPoint = Camera.main.ScreenToWorldPoint(mousePosition); touchPoint.z = 0;
            newStroke = new Stroke();
            newStroke.positions = new List<Vector3>();
            newStroke.timeStamps = new List<float>();
            newStroke.speeds = new List<float>();
            newStroke.lengths = new List<float>();
            newstrokeColliderPoints = new List<Vector2>();
            newStroke.positions.Add(touchPoint);
            newStroke.timeStamps.Add(Time.time);
            newstrokeGameobject = Instantiate(Resources.Load("Stroke"), Vector3.zero, Quaternion.identity) as GameObject;
            newstrokeLine = newstrokeGameobject.GetComponent<LineRenderer>();
            newstrokeGameobject.GetComponent<StrokeBehavior>().inklingController = inklingController;
            newstrokeCollider = newstrokeGameobject.GetComponent<EdgeCollider2D>();
            Strokes.Add(newStroke);
            Strokelines.Add(newstrokeLine);
            strokeColliders.Add(newstrokeCollider);
            //for fuzzy logic
            AddWayPoint(touchPoint, Time.time);
        }
        else if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(0).fingerId == newFingerTouchID) || (Application.isEditor && Input.GetMouseButton(0)))
        {
            Vector3 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            if (Application.isEditor)
            {
                mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            }
            else
            {
                mousePosition = new Vector2(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y);
            }
            Vector3 touchPoint = Camera.main.ScreenToWorldPoint(mousePosition); touchPoint.z = 0;
            if (newStroke.positions.Count > 0 && Vector3.Distance(touchPoint, newStroke.positions[newStroke.positions.Count - 1]) > 0.5f)
            {
                newStroke.positions.Add(touchPoint);
                newStroke.timeStamps.Add(Time.time);
                newstrokeColliderPoints.Add((Vector2)touchPoint);
                newstrokeLine.positionCount = newStroke.positions.Count;
                newstrokeLine.SetPositions(newStroke.positions.ToArray());
                //for fuzzy logic
                AddWayPoint(touchPoint, Time.time);
                if (newStroke.positions.Count >= 2)
                {
                    int count = newStroke.positions.Count;
                    float length = Vector3.Distance(newStroke.positions[count - 1], newStroke.positions[count - 2]);
                    float time = newStroke.timeStamps[count - 1] - newStroke.timeStamps[count - 2];
                    float speed = length / time;
                    newStroke.speeds.Add(speed);
                    newStroke.lengths.Add(length);
                }
                if (newstrokeColliderPoints.Count >= 2)
                {
                    newstrokeCollider.points = newstrokeColliderPoints.ToArray();
                }
                if (newStroke.positions.Count > 20)
                {
                    for (int i = 0; i < newStroke.speeds.Count; i++)
                    {
                        sum += newStroke.speeds[i];
                        totalLength += newStroke.lengths[i];
                        speedCount += 1;
                    }
                    avgSpeed = sum / speedCount;
                    AnalyseManager.access.aggressiveScore = avgSpeed;
                }

            }
        }
        else if (((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended && Input.GetTouch(0).fingerId == newFingerTouchID) || Input.GetMouseButtonUp(0)))
        {
            Vector3 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            if (Application.isEditor)
            {
                mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            }
            else
            {
                mousePosition = new Vector2(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y);
            }
            Vector3 touchPoint = Camera.main.ScreenToWorldPoint(mousePosition); touchPoint.z = 0;
            if (newStroke.positions.Count < 5)
            {
                GameObject touchWave = Instantiate(Resources.Load("TouchWave"), touchPoint, Quaternion.identity) as GameObject;
                string color = inkling.GetComponent<InklingController>().color;
                touchWave.GetComponent<SpriteRenderer>().color = ColorCollection.access.colorDict[color];
                Strokes.Remove(newStroke);
                strokeColliders.Remove(newstrokeCollider);
                Strokelines.Remove(newstrokeLine);
                Destroy(newstrokeGameobject);
            }

            if (Strokes.Count > 1)
            {
                if (newStroke.positions.Count > 5)
                {
                    Vector3 center = Vector3.zero;
                    for (int i = 0; i < newStroke.positions.Count; i++)
                    {
                        center += newStroke.positions[i];
                        points.Add(new Point(newStroke.positions[i].x, -newStroke.positions[i].y, 0));
                    }
                    center = center / newStroke.positions.Count;
                    Gesture candidate = new Gesture(points.ToArray());
                    Result gestureResult = PointCloudRecognizer.Classify(candidate, trainingSet.ToArray());
                    points.Clear();
                    if (gestureResult.Score > 0.85f)
                    {
                        GameObject splatParticle = Instantiate(Resources.Load("Effects/SplatParticleAround"), center, Quaternion.identity) as GameObject;
                        SplatParticleBehavior splatParticleBehavior = splatParticle.GetComponent<SplatParticleBehavior>();
                        string color = ColorCollection.access.colorList[Random.Range(0, ColorCollection.access.colorList.Count)];
                        splatParticleBehavior.color = color;
                        GameObject largeInkling = Instantiate(Resources.Load("LargeInklings/" + gestureResult.GestureClass), center, Quaternion.identity) as GameObject;
                        LargeInklingBehavior largeInklingBehavior = largeInkling.GetComponent<LargeInklingBehavior>();
                        largeInklingBehavior.color = color;
                        largeInklingBehavior.canRespawn = false;
                        Strokes.Remove(newStroke);
                        strokeColliders.Remove(newstrokeCollider);
                        Strokelines.Remove(newstrokeLine);
                        Destroy(newstrokeGameobject);
                    }
                }
            }

        }
        FuzzyDirection();
        if (Strokes.Count > 0)
        {

            // currentFollowStroke = Strokes[0];
            // currentTarget = currentFollowStroke.positions[0];
            // currentstrokeLine = Strokelines[0];
            // currentstrokeCollider = strokeColliders[0];
            // currentstrokeColliderPoints = currentstrokeCollider.points.ToList();
            // updateStrokeStatus();
        }
        else if (Strokes.Count == 0)
        {
            if (Vector3.Distance(inkling.position, currentTarget) <= 1)
            {
                currentTarget = Vector3.zero;
            }
        }
    }

    void LateUpdate()
    {
        AutoClearStroke();
    }
    public void updateStrokeStatus()
    {
        if (Vector3.Distance(inkling.position, currentTarget) <= 1)
        {
            inklingController.accelerateCD = 0.5f;
            currentFollowStroke.positions.RemoveAt(0);
            currentFollowStroke.timeStamps.RemoveAt(0);
            if (currentFollowStroke.speeds.Count >= 1)
            {
                //inklingController.trackSpeed = currentFollowStroke.speeds[0];
                currentFollowStroke.speeds.RemoveAt(0);
                currentFollowStroke.lengths.RemoveAt(0);
            }
            //
            if (currentstrokeColliderPoints.Count > 2)
            {
                currentstrokeColliderPoints.RemoveAt(0);
                currentstrokeCollider.points = currentstrokeColliderPoints.ToArray();
            }
            currentstrokeLine.SetPositions(currentFollowStroke.positions.ToArray());

            if (currentFollowStroke.positions.Count > 0)
            {
                currentTarget = currentFollowStroke.positions[0];
            }
            else
            {
                Strokes.RemoveAt(0);
                Strokelines.RemoveAt(0);
                strokeColliders.RemoveAt(0);
                currentstrokeColliderPoints.Clear();
                Destroy(currentstrokeLine.gameObject);
                if (Strokes.Count > 0)
                {
                    currentFollowStroke = Strokes[0];
                    currentstrokeLine = Strokelines[0];
                    currentstrokeCollider = strokeColliders[0];
                }
                else
                {
                    currentFollowStroke = new Stroke();
                    currentstrokeColliderPoints = new List<Vector2>();
                    currentstrokeLine = new LineRenderer();
                    currentstrokeCollider = new EdgeCollider2D();
                    currentTarget = Vector3.zero;
                }
            }

        }
    }

    void AutoClearStroke()
    {
        if (Strokes.Count > 0)
        {
            int strokeCount = Strokes.Count;
            for (int i = 0; i < strokeCount; i++)
            {
                int count = Strokes[i].timeStamps.Count;
                //update stroke alpha and width
                if (count > 1)
                {
                    // float sum = 0;
                    // foreach (float length in Strokes[i].lengths)
                    // {
                    //     sum += length;
                    // }
                    // float currentlength = 0;
                    List<GradientAlphaKey> alphaKeys = new List<GradientAlphaKey>();
                    alphaKeys.Add(new GradientAlphaKey(0, 0));
                    alphaKeys.Add(new GradientAlphaKey(1 - (Time.time - Strokes[i].timeStamps[0]) / fadeoutTime, 0.02f));
                    alphaKeys.Add(new GradientAlphaKey(1.8f - (Time.time - Strokes[i].timeStamps[count - 1]) / fadeoutTime, 0.95f));
                    alphaKeys.Add(new GradientAlphaKey(0, 1));
                    // for (int k = 1; k < count; k++)
                    // {
                    //     currentlength += Strokes[i].lengths[k - 1];
                    //     float alpha = Mathf.Min(1, (Time.time - Strokes[i].timeStamps[k]) / fadeoutTime);
                    //     float time = Mathf.Min(1, currentlength / sum);
                    //     alphaKeys.Add(new GradientAlphaKey(alpha, time));
                    // }

                    Gradient gradient = new Gradient();
                    GradientColorKey[] color = Strokelines[i].colorGradient.colorKeys;
                    GradientAlphaKey[] alphas = alphaKeys.ToArray();
                    gradient.SetKeys(color, alphas);
                    AnimationCurve curve = new AnimationCurve();
                    curve.AddKey(0.0f, 2 + ((Time.time - Strokes[i].timeStamps[0]) / fadeoutTime) * (maxWidth - 1));
                    curve.AddKey(1.0f, 2 + ((Time.time - Strokes[i].timeStamps[count - 1]) / fadeoutTime) * (maxWidth - 1));
                    Strokelines[i].colorGradient = gradient;
                    Strokelines[i].widthCurve = curve;
                }
                if (Time.time - Strokes[i].timeStamps[0] > fadeoutTime)
                {

                    Strokes[i].positions.RemoveAt(0);
                    Strokes[i].timeStamps.RemoveAt(0);
                    if (Strokes[i].speeds.Count >= 1)
                    {
                        Strokes[i].speeds.RemoveAt(0);
                        Strokes[i].lengths.RemoveAt(0);
                    }
                    //
                    if (strokeColliders[i].points.Length > 2)
                    {
                        List<Vector2> plist = strokeColliders[i].points.ToList();
                        plist.RemoveAt(0);
                        strokeColliders[i].points = plist.ToArray();
                    }
                    Strokelines[i].SetPositions(Strokes[i].positions.ToArray());

                    if (Strokes[i].positions.Count == 0)
                    {
                        Destroy(Strokelines[i].gameObject);
                        Strokes.Remove(Strokes[i]);
                        Strokelines.Remove(Strokelines[i]);
                        strokeColliders.Remove(strokeColliders[i]);
                        i = i - 1;
                        if (Strokes.Count > 0)
                        {

                        }
                        else
                        {
                            currentFollowStroke = new Stroke();
                            currentstrokeColliderPoints = new List<Vector2>();
                            currentstrokeLine = new LineRenderer();
                            currentstrokeCollider = new EdgeCollider2D();
                            currentTarget = Vector3.zero;
                            break;
                        }
                    }
                }
                strokeCount = Strokes.Count;
            }
        }
    }
    void FuzzyDirection()
    {
        int count = WayPoints.Count;
        if (count > 0)
        {
            Vector3 sumDir = Vector3.zero;
            for (int i = 0; i < count; i++)
            {
                float dist = Vector3.Distance(WayPoints[i].position, inkling.position);
                float time = Time.time - WayPoints[i].timeStamp;
                if (time > fadeoutTime || dist <= 3)
                {
                    WayPoints.RemoveAt(i);
                    i -= 1;
                    count -= 1;
                    continue;
                }
                else
                {
                    Vector3 dir = (WayPoints[i].position - inkling.position).normalized;
                    float tWeight = 10 * time / fadeoutTime;
                    float dWeight = Mathf.Max(0, 12 - dist);
                    sumDir += ((tWeight + dWeight) / 2) * dir;
                }

            }
            fuzzyDirection = sumDir.normalized;
        }
    }
    void AddWayPoint(Vector3 position, float timeStamp)
    {
        WayPoint waypoint = new WayPoint();
        waypoint.position = position;
        waypoint.timeStamp = timeStamp;
        WayPoints.Add(waypoint);
    }
    public Vector3 direction()
    {
        if (currentTarget == Vector3.zero)
        {
            return Vector3.zero;
        }
        Vector3 direction = (currentTarget - inkling.position).normalized;
        return direction;
    }

    public Vector3 FDirection()
    {
        return fuzzyDirection;
    }
}
