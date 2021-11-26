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
            case typeOfItem.STICKER_RED :
                gameObject.GetComponent<SpriteRenderer>().sprite = TheItem.stickerRed.sprite;
                break; 
            case typeOfItem.STICKER_BLUE : 
                gameObject.GetComponent<SpriteRenderer>().sprite = TheItem.stickerBlue.sprite;
                break;
            case typeOfItem.STICKER_GREEN :
                gameObject.GetComponent<SpriteRenderer>().sprite = TheItem.stickerGreen.sprite;
                break;
        }
    }

    private void Update()
    {
        switch (TheItem.TypeOfItem)
        {
            case typeOfItem.SPELL :
                GetSpellItem();
                break;
            case typeOfItem.COINS : 
                break;
            case typeOfItem.STICKER_RED :
                GetItemStickerRed();
                break; 
            case typeOfItem.STICKER_BLUE : 
                GetItemStickerBlue();
                break;
            case typeOfItem.STICKER_GREEN :
                GetItemStickerGreen();
                break;
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
    
    // FONCTIONS LIER AU ITEMS SPELL

    void GetSpellItem()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton1) && checkIfGood)
        {
            Debug.Log("State 1");
            if (_inventory.slots[0].isFull)
            {
                Debug.Log("State 2");
                switch (_inventory.slots[1].isFull)
                {
                    case true:
                        Debug.Log("State 3");
                        return;
                    case false:
                        Debug.Log("State 4");
                        WitchSpellItem(1);
                        break;
                }
            }
            else WitchSpellItem(0);
        }
    }

    void WitchSpellItem(int slotNumber)
    {
        if (_inventory.slots[slotNumber].isFull == false)
        {
            _inventory.slots[slotNumber].item = this.gameObject;
            _inventory.slots[slotNumber].isFull = true;
            this.gameObject.SetActive(false);
            this.gameObject.transform.parent = _inventory.transform;
            Debug.Log("J'ai pris le spell");
        }
    }
    
    // FONCTIONS LIER AU ITEMS STICKER

    void GetItemStickerRed()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button4) && checkIfGood)
        {
            BonusManager.instance.redStat += TheItem.stickerRed.value;
            Destroy(gameObject);
        }
    }
    void GetItemStickerBlue()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button4) && checkIfGood)
        {
            BonusManager.instance.blueStat += TheItem.stickerBlue.value;
            Destroy(gameObject);
        }
    }
    void GetItemStickerGreen()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button4) && checkIfGood)
        {
            BonusManager.instance.greenStat += TheItem.stickerGreen.value;
            Destroy(gameObject);
        }
    }
}

public enum typeOfItem
{
    SPELL, 
    COINS,
    STICKER_RED,
    STICKER_BLUE,
    STICKER_GREEN,
}

[Serializable]
public class theItem
{
    public typeOfItem TypeOfItem;
    public spellItem SpellItem;
    public coinItem CoinItem;
    public stickerRed stickerRed;
    public stickerBlue stickerBlue;
    public stickerGreen stickerGreen;
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

[Serializable]
 public class stickerRed
 {
     public int value;
     public Sprite sprite;
 }
 
[Serializable]
public class stickerBlue
{
    public int value;
    public Sprite sprite;
}

[Serializable]
public class stickerGreen
{
    public int value;
    public Sprite sprite;
}