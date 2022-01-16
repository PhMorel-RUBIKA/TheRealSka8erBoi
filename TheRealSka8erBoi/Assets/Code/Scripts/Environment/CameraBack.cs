using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBack : MonoBehaviour
{
    private bool isDezoom;
    private Camera playerCamera;
    public float dezoomValue;

    private void Start() { isDezoom = false; }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") && !other.CompareTag("PlayerDashing")) return;
        playerCamera = other.GetComponentInChildren<Camera>();
        isDezoom = true;
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player") && !other.CompareTag("PlayerDashing")) return;
        isDezoom = false;
    }

    private void Update()
    {
        switch (isDezoom && playerCamera != null)
        {
            case true:
                playerCamera.orthographicSize = Mathf.Lerp(playerCamera.orthographicSize, 17.32f, dezoomValue);
                break;
            case false:
            {
                if (playerCamera == null) return;
                if (playerCamera.orthographicSize != 9.5f) playerCamera.orthographicSize = Mathf.Lerp(playerCamera.orthographicSize, 9.5f, dezoomValue);
                break;
            }
        }
    }
}
