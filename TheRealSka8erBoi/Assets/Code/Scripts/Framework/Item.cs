using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public Inventory _inventory;
    public theItem TheItem;

    public bool checkIfGood; 

    private void Start()
    {
        checkIfGood = false;
        
        switch (TheItem.TypeOfItem)
        {
            case typeOfItem.SPELL :
                gameObject.GetComponent<SpriteRenderer>().sprite = TheItem.SpellItem.spellImage;
                break;
            case typeOfItem.COINS : 
                break;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton4) && checkIfGood)
        {
            if (_inventory.slots[0].isFull == false)
            {
                _inventory.slots[0].item = this.gameObject;
                _inventory.slots[0].isFull = true;
                Debug.Log("Je ramasse0");
                this.gameObject.SetActive(false);
                this.gameObject.transform.parent = _inventory.transform;
            }
        }

        if (Input.GetKeyDown(KeyCode.JoystickButton5) && checkIfGood)
        {
            if (_inventory.slots[1].isFull == false)
            {
                _inventory.slots[1].item = gameObject;
                _inventory.slots[1].isFull = true;
                Debug.Log("Je ramasse");
                gameObject.SetActive(false);
                gameObject.transform.parent = _inventory.transform;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            checkIfGood = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            checkIfGood = false;
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
    public Sprite spellUI;
}

[Serializable]
public class coinItem
{
    public int value;
}
