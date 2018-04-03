using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ColorSceneManager : MonoBehaviour
{
    public static ColorSceneManager access;
    bool colorSelected;
    private void Awake()
    {
        access = this;
    }
    // Use this for initialization
    public void OnColorButtonClicked(string color)
    {
        if (!colorSelected)
        {
            colorSelected = true;
            SceneManager.LoadScene("CharacterCreation");
            PlayerPrefs.SetString("CreationColor", color);
        }
    }
}
