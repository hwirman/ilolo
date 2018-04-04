using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ReturnButtonBehavior : MonoBehaviour
{

    // Use this for initialization
    private void OnMouseDown()
    {
        SceneManager.LoadScene("Start");
    }
}
