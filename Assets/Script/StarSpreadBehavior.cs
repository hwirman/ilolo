using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarSpreadBehavior : MonoBehaviour
{
    public float maxsize = 20;
    public float spreadSpeed = 2;
    float size;
    // Use this for initialization
    void Start()
    {
        size = transform.localScale.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        size = Mathf.Min(maxsize, size + Time.fixedDeltaTime * spreadSpeed);
        transform.localScale = new Vector3(size, size, size);
        if (size >= maxsize)
        {
            Destroy(gameObject);
        }
    }
}
