using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskMarkerPlacer : MonoBehaviour
{
    //public PolygonCollider2D[] polygons;
    PolygonCollider2D pc;
    public int gridSizeX = 0;
    public int gridSizeY = 0;
    public float increment = 0.5f;
    public GameObject[,] DotGrid;
    public Vector3[,] DotPosition;
    public string color;
    GameObject player;

    public float steerCD = 8;
    InklingController inklingscript;
    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("Inkling");
        inklingscript = player.GetComponent<InklingController>();
        pc = GetComponent<PolygonCollider2D>();
        gridSizeX = (int)(Mathf.Ceil(pc.bounds.size.y) / increment);
        gridSizeY = (int)(Mathf.Ceil(pc.bounds.size.x) / increment);
        DotGrid = new GameObject[gridSizeX, gridSizeY];
        DotPosition = new Vector3[gridSizeX, gridSizeY];
        // color = ColorCollection.access.colorList[Random.Range(0, ColorCollection.access.colorList.Count)];
        //SpawnPosition();
        SetPositions();
    }
    void SetPositions()
    {
        Collider2D[] boids = Physics2D.OverlapCircleAll(transform.position, 100, 1 << LayerMask.NameToLayer("Boid"));
        foreach (Collider2D b in boids)
        {
            GameObject boid = b.gameObject;
            ElementBehavior eb = boid.GetComponent<ElementBehavior>();
            eb.steerCD = 0;
        }
        float x = pc.bounds.min.x;
        float y = pc.bounds.min.y;
        for (int i = 0; i < gridSizeX; i++)
        {
            for (int j = 0; j < gridSizeY; j++)
            {
                DotPosition[i, j] = new Vector3(x, y, 0);
                RaycastHit2D hit = Physics2D.Raycast(DotPosition[i, j], Vector2.zero, 1 << LayerMask.NameToLayer("TextureMask"));
                if (hit.collider != null)
                {
                    if (boids.Length > 0)
                    {

                        foreach (Collider2D b in boids)
                        {
                            GameObject boid = b.gameObject;
                            ElementBehavior eb = boid.GetComponent<ElementBehavior>();
                            if (eb.steerCD == 0)
                            {
                                eb.steerCD = steerCD;
                                eb.goalPos = DotPosition[i, j];
                                break;
                            }
                            else
                            {
                                continue;
                            }
                        }
                    }
                }
                x += increment;
            }
            y += increment;
            x = pc.bounds.min.x;
        }
        Destroy(gameObject);
    }
    void SpawnPosition()
    {
        float x = pc.bounds.min.x;
        float y = pc.bounds.min.y;
        for (int i = 0; i < gridSizeX; i++)
        {
            for (int j = 0; j < gridSizeY; j++)
            {
                DotPosition[i, j] = new Vector3(x, y, 0);
                RaycastHit2D hit = Physics2D.Raycast(DotPosition[i, j], Vector2.zero, 1 << LayerMask.NameToLayer("TextureMask"));
                if (hit.collider != null)
                {
                    float angle = Random.Range(0, 2 * Mathf.PI);
                    float r = Random.Range(20, 30);
                    Vector3 spawnPos = new Vector3(r * Mathf.Sin(angle), r * Mathf.Cos(angle), 0) + pc.bounds.center;
                    GameObject boid = (GameObject)Instantiate(Resources.Load("Elements/BoidElement"), spawnPos, Quaternion.identity);
                    ElementBehavior eb = boid.GetComponent<ElementBehavior>();
                    eb.player = player;
                    eb.inklingScript = inklingscript;
                    eb.color = color;
                    eb.steerCD = steerCD;
                    eb.goalPos = DotPosition[i, j];
                }
                x += increment;
            }
            y += increment;
            x = pc.bounds.min.x;
        }
    }
    //	void OnDrawGizmos() {
    //		pc = GetComponent<PolygonCollider2D> ();
    //		Gizmos.color = Color.yellow;
    //		Gizmos.DrawSphere(pc.bounds.min, 0.1f);
    //		Gizmos.DrawSphere(pc.bounds.max, 0.1f);
    //	}
}
