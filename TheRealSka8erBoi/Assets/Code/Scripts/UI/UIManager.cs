using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Inventory Inventory;
    public Sprite neutral;
    public Image spell1;
    public Image spell2;

    private void Update()
    {
        if (Inventory.slots[0].item != null)
            spell1.sprite = Inventory.slots[0].item.GetComponent<Item>().TheItem.SpellItem.spellUI;
        if (Inventory.slots[1].item != null) 
            spell2.sprite = Inventory.slots[1].item.GetComponent<Item>().TheItem.SpellItem.spellUI;

        if (Inventory.slots[0].item == null) spell1.sprite = neutral;
        if (Inventory.slots[1].item == null) spell2.sprite = neutral;
    }
}
