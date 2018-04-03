using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarFlowerBehavior : MonoBehaviour
{
    public Vector3 direction;
    GameObject player;
    void Start()
    {
        var euler = transform.eulerAngles;
        euler.z = Random.Range(0.0f, 360.0f);
        transform.eulerAngles = euler;
        player = GlobalFlock.access.player;
    }
    // Update is called once per frame
    void Update()
    {
        var euler = transform.eulerAngles;
        euler.z += 90 * Time.deltaTime;
        transform.eulerAngles = euler;
        transform.position += Random.Range(5, 10) * direction * Time.deltaTime;
        if (Vector3.Distance(transform.position, player.transform.position) >= Mathf.Sqrt(2) * GlobalFlock.access.playgroundSize)
        {
            Destroy(gameObject);
        }

    }
}
