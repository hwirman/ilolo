using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchWaveBehavior : MonoBehaviour
{
    //public float size = 0.5f;
    float maxprevSize = 0.25f;

    // Use this for initialization
    void Start()
    {
        Invoke("DisableTrigger", 0.2f);
        //maxprevSize = size;
        // Collider2D[] waves = Physics2D.OverlapCircleAll(transform.position, 1.0f, 1 << LayerMask.NameToLayer("Wave"));
        // if (waves.Length > 1)
        // {
        //     foreach (Collider2D wave in waves)
        //     {
        //         TouchWaveBehavior wavebehavior = wave.GetComponent<TouchWaveBehavior>();
        //         if (wavebehavior != this)
        //         {
        //             maxprevSize = Mathf.Max(maxprevSize, wavebehavior.size);
        //         }
        //     }
        // }
        // size = Mathf.Min(maxprevSize * 2, 4);
        // transform.localScale = new Vector3(size, size, size);
    }

    void DisableTrigger()
    {
        GetComponent<CircleCollider2D>().enabled = false;
    }


}
