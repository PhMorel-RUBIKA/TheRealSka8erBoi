using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    public Transform target;
    public CameraHelper cameraHelper;
    public float smoothSpeed = 0.1f;
    public int targetFps = 60;
    public Vector3 offset;
    [SerializeField] private float leftLimit;
    [SerializeField] private float rightLimit;
    [SerializeField] private float bottomLimit;
    [SerializeField] private float topLimit;

    private Vector2 aimoff;
    [SerializeField] private float aimMultiplier;

    private void Start()
    {
        Application.targetFrameRate = targetFps;
    }

    void Update()
    {
        aimoff.x = Input.GetAxis("RightJoy_Horizontal");
        aimoff.y = -Input.GetAxis("RightJoy_Vertical");
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed); 
        transform.position = smoothedPosition;
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x + (aimoff.x*aimMultiplier), leftLimit, rightLimit),
            Mathf.Clamp(transform.position.y + (aimoff.y*aimMultiplier), bottomLimit, topLimit),
            transform.position.z);
        //cameraHelper.MoveTo(transform.position);
    }
}
