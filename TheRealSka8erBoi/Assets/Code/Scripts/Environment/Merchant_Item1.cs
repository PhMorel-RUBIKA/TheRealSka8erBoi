using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Merchant_Item1 : MonoBehaviour
{
    [SerializeField] private RectTransform currentImage;
    [SerializeField]
    private Transform currentPosition;
    [SerializeField]
    private bool setImagePosition;

    [SerializeField] private Camera currentCamera;
    

    private void Update()
    {
        if (setImagePosition)
        {
            SetUpImagePosition();
        }
    }

    void SetUpImagePosition()
    {
     currentImage.position =   currentCamera.WorldToScreenPoint(currentPosition.position);     
        
    }

   
}
