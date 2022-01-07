using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulleApparition : MonoBehaviour
{
    public CanvasGroup CanvasGroup;
    private bool isActive;

    private void Start()
    {
        isActive = false;
    }

    private void Update()
    {
        switch (isActive)
        {
            case true :
                if (CanvasGroup.alpha == 1) return;
                CanvasGroup.alpha = Mathf.Lerp(CanvasGroup.alpha, 1, 0.5f);
                break;
            case false :
                if (CanvasGroup.alpha == 0) return;
                CanvasGroup.alpha = Mathf.Lerp(CanvasGroup.alpha, 0, 0.5f);
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) isActive = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("PlayerDashing")) isActive = false;
    }
}
