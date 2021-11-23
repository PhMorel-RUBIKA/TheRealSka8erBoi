using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<slot> slots = new List<slot>();
    
    public bool resetTriggerLeft;
    public bool resetTriggerRight;

    private void Start()
    {
        resetTriggerLeft = true;
        resetTriggerRight = true;
    }

    private void Update()
    {
        if (Input.GetAxisRaw("LeftSpell") > 0.95f && slots[0].isFull && resetTriggerLeft)
            if (slots[0].item.GetComponent<Item>().TheItem.SpellItem.spellScriptable.cooldownTime >= 0)
            {
                UsedSpell(0);
                resetTriggerLeft = false;
            }

        if (Input.GetAxisRaw("RightSpell") > 0.95f && slots[1].isFull && resetTriggerRight)
            if (slots[1].item.GetComponent<Item>().TheItem.SpellItem.spellScriptable.cooldownTime >= 0)
            {
                UsedSpell(1);
                resetTriggerRight = false;
            }

        if (Input.GetAxisRaw("LeftSpell") < 0.05f)
            resetTriggerLeft = true;
        if (Input.GetAxisRaw("RightSpell") < 0.05f)
            resetTriggerRight = true;
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
