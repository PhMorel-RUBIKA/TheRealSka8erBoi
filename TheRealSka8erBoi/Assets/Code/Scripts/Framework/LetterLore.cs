using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using UnityEngine;

public class LetterLore : MonoBehaviour
{
    public GameObject canvasLetter;
    private bool isActive;
    private bool canActive;
    public MMFeedbacks letterOutline;

    private void Start()
    {
        isActive = false;
        canActive = false;
        letterOutline.PlayFeedbacks();
    }

    private void Update()
    {
        if (canActive)
        {
            if (Input.GetKeyDown(KeyCode.JoystickButton3))
            {
                switch (isActive)
                {
                    case false :
                        canvasLetter.SetActive(true);
                        isActive = true;
                        break; 
                    case true : 
                        canvasLetter.SetActive(false);
                        isActive = false;
                        break;
                }   
            }  
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) canActive = true; 
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) canActive = false;
        if (isActive)
        {
            canvasLetter.SetActive(false);
            isActive = false;
        }
    }
}
