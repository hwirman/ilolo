using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static float cycle = 1.5f;
    public static float timer;
    public static MusicManager access;
    public AudioSource bgm;
    // Use this for initialization
    void Start()
    {
        access = this;
    }

    // Update is called once per frame
    void Update()
    {
        timer = Mathf.Repeat(Time.time, cycle);
    }
    public void ChangeBGM(string color)
    {
        AudioClip audio = Resources.Load("Sound/" + "bgm_" + color, typeof(AudioClip)) as AudioClip;
        bgm.GetComponent<AudioSource>().clip = audio;
        bgm.GetComponent<AudioSource>().Play();
    }
    public void playSoundEffect(string soundName, Vector3 position = default(Vector3), float volumn = 0.5f, float delay = 0)
    {
        if (position == null)
        {

            position = Camera.main.transform.position;
        }
        GameObject audiosource = Instantiate(Resources.Load("audiosource"), position, Quaternion.identity) as GameObject;
        audiosource.transform.position = position;
        AudioClip audio = Resources.Load("Sound/" + soundName, typeof(AudioClip)) as AudioClip;
        Destroy(audiosource, audio.length + delay);
        audiosource.SetActive(true);
        audiosource.GetComponent<AudioSource>().clip = audio;
        audiosource.GetComponent<AudioSource>().volume = volumn;
        audiosource.GetComponent<AudioSource>().PlayDelayed(delay);
    }
}
