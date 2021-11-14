using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Inventory Inventory;
    public GameObject player;
    public Sprite neutral;
    public Image spell1;
    public Image spell2;
    
    private SpellHolder spellHolder1;
    private SpellHolder spellHolder2;
 
    private void Start()
    {
        SpellHolder[] spellHolders = player.GetComponents<SpellHolder>();
        foreach (SpellHolder spellHolder in spellHolders)
        {
            if (spellHolder.spellNumber == 0)
                spellHolder1 = spellHolder; 
            else
                spellHolder2 = spellHolder;
        }
    }

    private void Update()
    {
        if (Inventory.slots[0].item != null)
        {
            spell1.sprite = Inventory.slots[0].item.GetComponent<Item>().TheItem.SpellItem.spellUI;
            spell1.fillAmount = spellHolder1.cooldownNumber;
        }
        if (Inventory.slots[1].item != null)
        {
            spell2.sprite = Inventory.slots[1].item.GetComponent<Item>().TheItem.SpellItem.spellUI;
            spell2.fillAmount = spellHolder2.cooldownNumber;
        }
        
        if (Inventory.slots[0].item == null)
        {
            spell1.sprite = neutral;
        }
        if (Inventory.slots[1].item == null)
        {
            spell2.sprite = neutral;
        }
    }
}
