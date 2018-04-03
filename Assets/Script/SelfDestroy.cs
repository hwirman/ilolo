using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    public float time = 1;
    // Use this for initialization
    void Start()
    {
        float size = transform.localScale.x;
        if (GameManager.access.persona == Persona.aggressive)
        {
            transform.localScale = new Vector3(size * 2f, size * 2f, size * 2f);
        }
        Destroy(gameObject, time);
    }
}
