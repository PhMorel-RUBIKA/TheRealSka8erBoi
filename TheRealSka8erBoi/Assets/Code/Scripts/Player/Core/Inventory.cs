using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public bool[] isFull;
    public List<GameObject> slots = new List<GameObject>();

    private void Update()
    {
        if (Input.GetKey(KeyCode.Joystick1Button4))
        {
            if (isFull[0] == true)
            {
                GameObject itemToDish = slots[0].gameObject; 
                itemToDish.transform.SetParent(null);
            }
        }
        
        if (Input.GetKey(KeyCode.Joystick1Button5))
        {
            if (isFull[1] == false)
            {
                GameObject itemToDish = slots[0].gameObject; 
                itemToDish.transform.SetParent(null);
            }
        }
    }
}
