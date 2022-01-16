using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InBossDezoomer : MonoBehaviour
{
    private bool dezoom = false;
    private Camera playerCam;
    [SerializeField] private float camSize;
    public float dezoomValue;
    
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") && !other.CompareTag("PlayerDashing")) return;
        playerCam = other.GetComponentInChildren<Camera>();
    }

    void Update()
    {
        if (playerCam==null) return;
        if (playerCam.orthographicSize == camSize) return;
        playerCam.orthographicSize = Mathf.Lerp(playerCam.orthographicSize, camSize, dezoomValue);
    }
    

}
