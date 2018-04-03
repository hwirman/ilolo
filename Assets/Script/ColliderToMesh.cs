using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(PolygonCollider2D))]
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[ExecuteInEditMode]
public class ColliderToMesh : MonoBehaviour
{
    PolygonCollider2D pCollider;
    Vector2[] originalPoints;
    float[] originalRadius;
    Vector2 center;
    public string color = "white";
    MeshFilter mf;
    public float offsetRange = 1;
    public float perlinScale = 1;
    Vector2[] points;
    Vector3[] linePoints;
    LineRenderer lr;
    public GameObject lineEdge;
    EdgeCollider2D linecollider;

    bool boidInside = false;
    bool detectBoid = true;
    public string spawnPath;
    void Start()
    {
        pCollider = GetComponent<PolygonCollider2D>();
        lr = GetComponent<LineRenderer>();
        mf = GetComponent<MeshFilter>();
        originalPoints = pCollider.points;
        originalRadius = new float[originalPoints.Length];
        points = new Vector2[originalPoints.Length];
        linePoints = new Vector3[originalPoints.Length];
        center = pCollider.bounds.center;
        for (int i = 0; i < originalPoints.Length; i++)
        {
            originalRadius[i] = Vector2.Distance(originalPoints[i], center);
        }
        StartCoroutine(GenerateMesh());
    }

    void Update()
    {
        if (Vector3.Distance(pCollider.bounds.center, GlobalFlock.access.player.transform.position) >= Mathf.Sqrt(2) * GlobalFlock.access.playgroundSize)
        {
            Destroy(gameObject);
        }
    }
    IEnumerator GenerateMesh()
    {
        yield return new WaitForSeconds(2 * Time.fixedDeltaTime);
        if (!boidInside && spawnPath != "")
        {
            detectBoid = false;
            GameObject largeinkling = Instantiate(Resources.Load(spawnPath), pCollider.bounds.center, Quaternion.identity) as GameObject;
            largeinkling.GetComponent<LargeInklingBehavior>().color = color;
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (detectBoid && other.tag == "Boid" && !boidInside)
        {
            boidInside = true;
            lineEdge = new GameObject();
            lineEdge.transform.parent = this.gameObject.transform;
            lineEdge.layer = 14;
            lineEdge.AddComponent<EdgeCollider2D>();
            linecollider = lineEdge.GetComponent<EdgeCollider2D>();
            linecollider.isTrigger = true;
            linecollider.points = originalPoints;
            InvokeRepeating("UpdateEdgeCollider", 0, 0.05f);
        }
        if (other.tag == "Player")
        {
            InklingController inklingController = other.gameObject.GetComponent<InklingController>();
            if (color != inklingController.color)
            {
                Destroy(gameObject);
            }
        }
    }
    void UpdateEdgeCollider()
    {
        offsetPolygonPoints();
        fillOutline();
    }
    void UpdatePolygon()
    {
        offsetPolygonPoints();
        fillPolygon();
    }

    void offsetPolygonPoints()
    {
        //		float xoff = 0;
        //		for (int i = 0; i < originalPoints.Length; i++) {
        //			float height = offsetRange * Mathf.PerlinNoise(xoff, Time.time * perlinScale);
        //			float width = offsetRange * Mathf.PerlinNoise(Time.time * perlinScale, xoff);
        //			Vector2 pos = Vector2.zero;
        //			pos.y = height-offsetRange/2;
        //			pos.x = width-offsetRange/2;
        //			points [i] = originalPoints [i] + pos;
        //			xoff += 0.1f;
        //		}
        for (int i = 0; i < originalPoints.Length; i++)
        {
            float angle = Mathf.Deg2Rad * (Vector2.SignedAngle(originalPoints[i] - center, Vector2.up));
            float offset = Map(-0.2f, 0.2f, -1, 1, Mathf.Sin(angle * 5 + Time.time * 10));
            float r = originalRadius[i] + offset;
            points[i] = center + new Vector2(r * Mathf.Sin(angle), r * Mathf.Cos(angle));
            linePoints[i] = (Vector3)points[i];
        }
        pCollider.points = points;
    }
    void fillPolygon()
    {
        int pointCount = 0;
        pointCount = originalPoints.Length;
        mf = GetComponent<MeshFilter>();
        Mesh mesh = new Mesh();
        Vector2[] points = pCollider.points;
        Vector3[] vertices = new Vector3[pointCount];
        Vector2[] uv = new Vector2[pointCount];
        for (int j = 0; j < pointCount; j++)
        {
            Vector2 actual = points[j];
            vertices[j] = new Vector3(actual.x, actual.y, -1);
            uv[j] = actual;
        }
        Triangulator tr = new Triangulator(points);
        int[] triangles = tr.Triangulate();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        mf.mesh = mesh;
        UpdateColor();
        fillOutline();
    }
    void fillOutline()
    {
        lr.positionCount = originalPoints.Length;
        lr.SetPositions(linePoints);
        lr.material.color = ColorCollection.access.colorDict["dark" + color];
    }
    void UpdateColor()
    {
        Vector3[] vertices = mf.mesh.vertices;
        Color[] colors = new Color[vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            colors[i] = ColorCollection.access.colorDict[color];
        }
        mf.mesh.colors = colors;
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
