using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalleryButtonBehavior : MonoBehaviour
{
    public bool next;
    GalleryBehavior galleryBehavior;
    // Use this for initialization
    void Start()
    {
        galleryBehavior = FindObjectOfType<GalleryBehavior>();
    }

    private void OnMouseDown()
    {
        if (next)
        {
            if (galleryBehavior.currentIndex == (PlayerPrefs.GetInt("screenshotNum") - 1))
            {
                galleryBehavior.currentIndex = 0;

            }
            else
            {
                galleryBehavior.currentIndex++;
            }
        }
        else
        {
            if (galleryBehavior.currentIndex == 0)
            {
                galleryBehavior.currentIndex = PlayerPrefs.GetInt("screenshotNum") - 1;

            }
            else
            {
                galleryBehavior.currentIndex--;
            }
        }
    }
}
