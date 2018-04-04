using UnityEngine;
using System.Collections;

public class AnalyseManager : MonoBehaviour
{

    public static AnalyseManager access;
    public GameObject player;
    public InklingController inklingcontroller;
    public GuideBehavior guideBehavior;
    //dynamic gameplay parameter
    public Persona persona = Persona.normal;
    bool quietGameplayTrigger;
    bool aggressiveGameplayTrigger;
    bool socialGameplayTrigger;
    public float detectionFrequency = 10;
    public float quietScore = 0;
    public float aggressiveScore = 0;

    float spawnBoidTextCD = 0;
    static int number = 0;
    public static int numberPicker()
    {
        number += 1;
        return number;
    }
    void Awake()
    {
        access = this;
    }

    void Start()
    {
        player = GameObject.Find("Inkling");
        inklingcontroller = player.GetComponent<InklingController>();
        guideBehavior = FindObjectOfType<GuideBehavior>();
        StartCoroutine(DetectPersona());
        InvokeRepeating("TakePicture", 5, 20);
    }
    IEnumerator DetectPersona()
    {
        if (quietScore > 6 * detectionFrequency / 10)
        {
            persona = Persona.quiet;
            detectionFrequency = 10;
        }
        else if (aggressiveScore > 100 && guideBehavior.totalLength > 50000)
        {
            persona = Persona.aggressive;
            detectionFrequency = 20;
        }
        else
        {
            persona = Persona.normal;
            detectionFrequency = 10;
        }
        quietScore = 0;
        aggressiveScore = 0;
        guideBehavior.sum = 0;
        guideBehavior.totalLength = 0;
        guideBehavior.speedCount = 0;
        yield return new WaitForSeconds(detectionFrequency);
        StartCoroutine(DetectPersona());
    }

    void Update()
    {
        spawnBoidTextCD = Mathf.Max(0, spawnBoidTextCD - Time.deltaTime);
        if (persona == Persona.quiet)
        {
            if (spawnBoidTextCD <= 0 && inklingcontroller.stayCD > 0)
            {
                float angle = Random.Range(0, 2 * Mathf.PI);
                Vector3 randomDirection = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0);
                Vector3 spawnPos = player.transform.position + 15 * randomDirection;

                Instantiate(Resources.Load("BoidText/BoidText" + Random.Range(1, 5).ToString()), spawnPos, Quaternion.identity);
                spawnBoidTextCD = 15;
            }
        }
    }

    void TakePicture()
    {
        int index = PlayerPrefs.GetInt("screenshotIndex");
        if (index < 6)
        {
            index++;
            PlayerPrefs.SetInt("screenshotIndex", index);
            PlayerPrefs.SetInt("screenshotNum", Mathf.Max(PlayerPrefs.GetInt("screenshotNum"), index));
        }
        else
        {
            index = 1;
            PlayerPrefs.SetInt("screenshotIndex", index);
        }
        UnityEngine.ScreenCapture.CaptureScreenshot(index.ToString() + "screenshot.png");

    }

}
