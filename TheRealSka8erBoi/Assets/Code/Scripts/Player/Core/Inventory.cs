using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<slot> slots = new List<slot>();
    public bool deathDefiance1, deathDefiance2;

    private void Update()
    {
        if (Input.GetAxisRaw("LeftSpell") > 0.95f && slots[0].isFull)
            UsedSpell(0);

        if (Input.GetAxisRaw("RightSpell") > 0.95f && slots[1].isFull)
            UsedSpell(1);
    }

    void UsedSpell(int slotNumber)
    {
        Destroy(slots[slotNumber].item);
        slots[slotNumber].item = null;
        slots[slotNumber].isFull = false; 
    }
}

[Serializable]
public class slot
{
    public bool isFull;
    public GameObject item;
}
