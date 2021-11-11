using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public Inventory _inventory;
    public theItem TheItem;

    private void Start()
    {
        switch (TheItem.TypeOfItem)
        {
            case typeOfItem.SPELL :
                gameObject.GetComponent<SpriteRenderer>().sprite = TheItem.SpellItem.spellImage;
                break;
            case typeOfItem.COINS : 
                break;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.JoystickButton4))
            {
                if (!_inventory.slots[0].isFull)
                {
                    slot newSlot = new slot();
                    newSlot.item = gameObject;
                    newSlot.isFull = true;
                    _inventory.slots.Insert(0, newSlot);

                    _inventory.slots.Remove(_inventory.slots[1]);
                    Destroy(gameObject);
                }
            }

            if (Input.GetKeyDown(KeyCode.JoystickButton5))
            {
                if (!_inventory.slots[1].isFull)
                {
                    slot newSlot = new slot();
                    newSlot.item = gameObject;
                    newSlot.isFull = true;
                    _inventory.slots.Insert(1, newSlot);

                    _inventory.slots.Remove(_inventory.slots[3]);
                    Destroy(gameObject);
                }
            }
        }
    }
}

public enum typeOfItem
{
    SPELL, 
    COINS,
}

[Serializable]
public class theItem
{
    public typeOfItem TypeOfItem;
    public spellItem SpellItem;
    public coinItem CoinItem;
}

[Serializable]
public class spellItem
{
    public Spell spellScriptable;
    public Sprite spellImage;
}

[Serializable]
public class coinItem
{
    public int value;
}
