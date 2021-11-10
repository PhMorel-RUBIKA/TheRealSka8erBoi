using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    [SerializeField] private Inventory _inventory;
    public List<theItem> TheItems = new List<theItem>();

    private void Start()
    {
        switch (TheItems[0].TypeOfItem)
        {
            case typeOfItem.SPELL :
                gameObject.GetComponent<SpriteRenderer>().sprite = TheItems[0].SpellItem.spellImage;
                break;
            case typeOfItem.COINS : 
                break;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKey(KeyCode.Joystick1Button4))
            {
                
            }

            if (Input.GetKey(KeyCode.Joystick1Button5))
            {
                
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
