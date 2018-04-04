using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalleryBehavior : MonoBehaviour
{
    public List<Sprite> picSprites = new List<Sprite>();
    SpriteRenderer sp;
    public int currentIndex = 0;
    // Use this for initialization
    // void OnGUI() //For testing
    // {
    //     GUI.TextArea(new Rect(100 * 0, 0, 100, 30), PlayerPrefs.GetInt("screenshotNum").ToString());
    // }
    void Start()
    {
        int num = PlayerPrefs.GetInt("screenshotNum");
        sp = GetComponent<SpriteRenderer>();
        if (num == 0)
        {
            return;
        }
        for (int i = 0; i < num; i++)
        {
            Texture2D pic = PNGSave.LoadPNG(Application.persistentDataPath + "/" + (i + 1).ToString() + "screenshot.png");
            Sprite picSprite = Sprite.Create(pic, new Rect(0.0f, 0.0f, pic.width, pic.height), new Vector2(0.5f, 0.5f), 100f);
            picSprites.Add(picSprite);
        }
    }

    // Update is called once per frame
    void Update()
    {
        sp.sprite = picSprites[currentIndex];
    }
}
