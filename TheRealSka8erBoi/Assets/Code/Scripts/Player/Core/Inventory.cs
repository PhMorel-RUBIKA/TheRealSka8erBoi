using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<slot> slots = new List<slot>();
    private void Update()
    {
        if (Input.GetAxis("Croix") < 0)
        {
            if (slots[0].isFull)
            {
                GameObject itemToDish = slots[0].item;
                itemToDish.GetComponent<SpriteRenderer>().sprite =
                    itemToDish.GetComponent<Item>().TheItem.SpellItem.spellImage;
                itemToDish.transform.SetParent(null);
                itemToDish.transform.position = this.gameObject.transform.position;
                itemToDish.SetActive(true);
                
                slots[0].item = null;
                slots[0].isFull = false; 
            }
        }
        
        if (Input.GetAxis("Croix") >  0)
        {
            if (slots[1].isFull)
            {
                GameObject itemToDish = slots[1].item;
                itemToDish.GetComponent<SpriteRenderer>().sprite =
                    itemToDish.GetComponent<Item>().TheItem.SpellItem.spellImage;
                itemToDish.transform.SetParent(null);
                itemToDish.transform.position = this.gameObject.transform.position;
                itemToDish.SetActive(true);

                
                slots[1].item = null;
                slots[1].isFull = false;
            }
        }
    }
}

[Serializable]
public class slot
{
    public bool isFull;
    public GameObject item;
}
