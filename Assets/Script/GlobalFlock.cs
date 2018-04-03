using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalFlock : MonoBehaviour
{
    public static GlobalFlock access;
    public GameObject player;
    public GameObject element;
    public float followPlayerCD;
    public string followColor;
    public int playgroundSize = 100;
    static int numElement = 25;
    public static GameObject[] greenElement = new GameObject[numElement];
    public static GameObject[] redElement = new GameObject[numElement];
    public static GameObject[] blueElement = new GameObject[numElement];
    public static GameObject[] purpleElement = new GameObject[numElement];
    public static GameObject[] orangeElement = new GameObject[numElement];
    public static GameObject[] yellowElement = new GameObject[numElement];
    public static Vector3 goalPos = Vector3.zero;

    // Use this for initialization
    void Awake()
    {
        access = this;
    }
    void Start()
    {
        player = GameObject.Find("Inkling");

        for (int i = 0; i < numElement; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-playgroundSize, playgroundSize), Random.Range(-playgroundSize, playgroundSize), 0);

            greenElement[i] = (GameObject)Instantiate(element, pos, Quaternion.identity);
            ElementBehavior eb = greenElement[i].GetComponent<ElementBehavior>();
            eb.player = player;
            eb.color = "green";
            eb.inklingScript = player.GetComponent<InklingController>();


        }
        for (int i = 0; i < numElement; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-playgroundSize, playgroundSize), Random.Range(-playgroundSize, playgroundSize), 0);

            redElement[i] = (GameObject)Instantiate(element, pos, Quaternion.identity);
            ElementBehavior eb = redElement[i].GetComponent<ElementBehavior>();
            eb.player = player;
            eb.color = "red";
            eb.inklingScript = player.GetComponent<InklingController>();


        }
        for (int i = 0; i < numElement; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-playgroundSize, playgroundSize), Random.Range(-playgroundSize, playgroundSize), 0);

            blueElement[i] = (GameObject)Instantiate(element, pos, Quaternion.identity);
            ElementBehavior eb = blueElement[i].GetComponent<ElementBehavior>();
            eb.player = player;
            eb.color = "blue";
            eb.inklingScript = player.GetComponent<InklingController>();

        }
        for (int i = 0; i < numElement; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-playgroundSize, playgroundSize), Random.Range(-playgroundSize, playgroundSize), 0);
            purpleElement[i] = (GameObject)Instantiate(element, pos, Quaternion.identity);
            ElementBehavior eb = purpleElement[i].GetComponent<ElementBehavior>();
            eb.player = player;
            eb.color = "purple";
            eb.inklingScript = player.GetComponent<InklingController>();
        }
        for (int i = 0; i < numElement; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-playgroundSize, playgroundSize), Random.Range(-playgroundSize, playgroundSize), 0);
            orangeElement[i] = (GameObject)Instantiate(element, pos, Quaternion.identity);
            ElementBehavior eb = orangeElement[i].GetComponent<ElementBehavior>();
            eb.player = player;
            eb.color = "orange";
            eb.inklingScript = player.GetComponent<InklingController>();
        }
        for (int i = 0; i < numElement; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-playgroundSize, playgroundSize), Random.Range(-playgroundSize, playgroundSize), 0);
            yellowElement[i] = (GameObject)Instantiate(element, pos, Quaternion.identity);
            ElementBehavior eb = yellowElement[i].GetComponent<ElementBehavior>();
            eb.player = player;
            eb.color = "yellow";
            eb.inklingScript = player.GetComponent<InklingController>();
        }
    }
    void FixedUpdate()
    {
        followPlayerCD = Mathf.Max(0, followPlayerCD - Time.fixedDeltaTime);
    }
    // Update is called once per frame
    void Update()
    {
        //		if (Random.Range (0, 10000) < 50) {
        //			goalPos = new Vector3 (Random.Range (-playgroundSize, playgroundSize), Random.Range (-playgroundSize, playgroundSize), 0);
        //		}
    }
}
