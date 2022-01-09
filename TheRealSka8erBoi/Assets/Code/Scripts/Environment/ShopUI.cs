using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour

{
    
    
    
    public GameObject buttonUI;
    //public GameObject shopUI;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("PlayerDashing"))
        {
            //shopUI.SetActive(true);
            buttonUI.SetActive(true);
            Debug.Log("boo");
        }
            
    }
    
    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("PlayerDashing"))
        {
            //shopUI.SetActive(false);
            buttonUI.SetActive(false);
            Debug.Log("aaahhh");
        }
            
    }
}
