using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookBehaviour : MonoBehaviour
{
    public GameObject frontBook; 
    public GameObject backBook;

    private void Update()
    {
        GetRotation();
    }

    void GetRotation()
    {
        if (gameObject.transform.rotation.eulerAngles.z >= 0 && gameObject.transform.rotation.eulerAngles.z < 180)
        {
            frontBook.SetActive(false);
            backBook.SetActive(true);
        }
        else
        {
            frontBook.SetActive(true);
            backBook.SetActive(false);
        }
    }
}
