using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class CameraFollowTarget : MonoBehaviour
{
    public Transform target;

    public float smoothTime = 0.5f;
    public float basesize;
    private Vector3 m_f3FollowVelocity;
    public float left_edge;
    public float right_edge;
    public float up_edge;
    public float bottom_edge;
    private void FixedUpdate()
    {
        if (target == null)
        {
            target = GameObject.FindWithTag("Player").transform;
        }
        Vector3 f3Position = transform.position;

        Vector2 f2TargetPosition = new Vector2(target.position.x, target.position.y);
        Vector2 f2Position = f3Position;

        Vector3 f3SmoothedPosition = Vector3.SmoothDamp(f2Position, f2TargetPosition, ref m_f3FollowVelocity, smoothTime);

        f3SmoothedPosition.z = f3Position.z;
        float finalpositionX;
        float finalpositionY;
        if (f3SmoothedPosition.x > left_edge && f3SmoothedPosition.x < right_edge)
        {
            finalpositionX = f3SmoothedPosition.x;
        }
        else
        {
            finalpositionX = transform.position.x;
        }
        if (f3SmoothedPosition.y > bottom_edge && f3SmoothedPosition.y < up_edge)
        {
            finalpositionY = f3SmoothedPosition.y;
        }
        else
        {
            finalpositionY = transform.position.y;
        }
        transform.position = new Vector3(finalpositionX, finalpositionY, transform.position.z);
        //Camera.main.orthographicSize = Mathf.Min(basesize + transform.position.y,7);
    }

    void Update()
    {
        //GetComponent<Camera>().backgroundColor = ColorCollection.access.colorDict["BG" + ColorCollection.access.camColor];
    }
}
