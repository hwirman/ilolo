using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Analytics;

public class MenuManager : MonoBehaviour
{
    public Animator titleAnim;
    public bool fadeToScene;
    float pressStartTime = 0;
    void Awake()
    {
        Application.targetFrameRate = 60;
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        pressStartTime += Time.deltaTime;
    }

    public void OnStartButtonClick()
    {
        titleAnim.SetTrigger("fadeout");
        fadeToScene = true;
        Invoke("LoadSceneDelayed", 1);
    }

    void LoadSceneDelayed()
    {
        SceneManager.LoadScene("ColorChoosing");
    }
}
