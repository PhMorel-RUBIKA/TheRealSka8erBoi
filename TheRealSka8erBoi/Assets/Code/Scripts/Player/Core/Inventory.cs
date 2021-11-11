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
        if (Input.GetKeyDown(KeyCode.JoystickButton4))
        {
            if (slots[0].isFull)
            {
                GameObject itemToDish = slots[0].item;
                itemToDish.GetComponent<SpriteRenderer>().sprite =
                    itemToDish.GetComponent<Item>().TheItem.SpellItem.spellImage;
                Instantiate(itemToDish, gameObject.transform.position, quaternion.identity);
                itemToDish.transform.SetParent(null);
                itemToDish.SetActive(true);
                
                slots[0].item = null;
                slots[0].isFull = false;
                Debug.Log(slots[0]);
            }
        }
        
        if (Input.GetKeyDown(KeyCode.JoystickButton5))
        {
            if (slots[1].isFull)
            {
                GameObject itemToDish = slots[1].item;
                itemToDish.GetComponent<SpriteRenderer>().sprite =
                    itemToDish.GetComponent<Item>().TheItem.SpellItem.spellImage;
                Instantiate(itemToDish, gameObject.transform.position, quaternion.identity);
                itemToDish.transform.SetParent(null);
                itemToDish.SetActive(true);

                
                slots[1].item = null;
                slots[1].isFull = false;
                Debug.Log(slots[0]);
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
