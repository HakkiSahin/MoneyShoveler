using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public Vector3 offset = new Vector3(8, 0, -13f);
    private float smoothTime = 0.125f;
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        target = GameObject.FindWithTag("Car").transform;
        Vector3 targetPos = target.position + offset;

        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
    }
}