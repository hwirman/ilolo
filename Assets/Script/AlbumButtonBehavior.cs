using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class AlbumButtonBehavior : MonoBehaviour
{

    // Use this for initialization
    private void OnMouseDown()
    {
        SceneManager.LoadScene("Gallery");
    }
}
