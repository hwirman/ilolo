using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BrushType { Brush1, Brush2, Brush3, Brush4, Brush5, Brush6, Brush7, Brush8, Brush9, Brush10, Brush11, Brush12, Brush13, Brush14, Brush15 };
public class BrushTrail : MonoBehaviour
{
    public BrushType brushtype;
    public GameObject creationButton;
    public int brushCount = 0;
    public bool halfCanvas;
    public float distance;
    public float maxDistance = 100;
    Plane targetPlane;
    GameObject currentTrail;
    GameObject mirroredTrail;
    Vector3 startPos = Vector3.zero;

    //for analytics
    bool firstTouch = false;
    float firstDrawTouch = 0;

    bool spawnedBrushDot;
    // Use this for initialization
    void Start()
    {
        targetPlane = new Plane(Camera.main.transform.forward * -1, this.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        firstDrawTouch += Time.deltaTime;
        if (distance >= maxDistance)
        {
            return;
        }
        if (((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetMouseButtonDown(0)))
        {
            if (firstTouch == false)
            {
                firstTouch = true;
            }

            startPos = transform.position;
            currentTrail = (GameObject)Instantiate(Resources.Load("Brush"), transform.position, Quaternion.identity);
            //currentTrail.GetComponent<TimedTrailRenderer>().material = Resources.Load("Brushes/" + brushtype.ToString()) as Material;
            mirroredTrail = (GameObject)Instantiate(Resources.Load("Brush"), transform.position, Quaternion.identity);
            // mirroredTrail.GetComponent<TimedTrailRenderer>().material = currentTrail.GetComponent<TimedTrailRenderer>().material;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float rayDistance;
            if (targetPlane.Raycast(ray, out rayDistance))
            {
                if ((halfCanvas && ray.GetPoint(rayDistance).x <= 0) || !halfCanvas)
                {
                    startPos = ray.GetPoint(rayDistance);
                    currentTrail.transform.position = startPos;

                }
            }
        }
        else if (((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) || Input.GetMouseButton(0)))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float rayDistance;
            if (targetPlane.Raycast(ray, out rayDistance))
            {
                if ((halfCanvas && ray.GetPoint(rayDistance).x <= 0) || !halfCanvas)
                {
                    Vector3 temp = currentTrail.transform.position;

                    currentTrail.transform.position = ray.GetPoint(rayDistance);
                    distance = Mathf.Min(maxDistance, distance + Vector3.Distance(temp, currentTrail.transform.position));
                    mirroredTrail.transform.position = new Vector3(-currentTrail.transform.position.x, currentTrail.transform.position.y, currentTrail.transform.position.z);
                    if (!spawnedBrushDot)
                    {
                        spawnedBrushDot = true;
                        //Instantiate(Resources.Load("Effects/BrushDot"), currentTrail.transform.position, Quaternion.identity);
                        //Instantiate(Resources.Load("Effects/BrushDot"), mirroredTrail.transform.position, Quaternion.identity);
                    }
                }
            }
        }
        else if (((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended) || Input.GetMouseButtonUp(0)))
        {

            if (Vector3.Distance(currentTrail.transform.position, startPos) < 0.1f)
            {
                spawnedBrushDot = false;
                Destroy(currentTrail);
                Destroy(mirroredTrail);
            }
            else
            {
                //Instantiate(Resources.Load("Effects/BrushDot"), currentTrail.transform.position, Quaternion.identity);
                //Instantiate(Resources.Load("Effects/BrushDot"), mirroredTrail.transform.position, Quaternion.identity);
                spawnedBrushDot = false;
                brushCount++;
            }
        }
        if (distance > maxDistance / 6)
        {
            creationButton.SetActive(true);
        }
    }
}
