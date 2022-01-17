using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class VendingMachine : MonoBehaviour
{
    private bool checkIfGood;

    [TextArea(4,5)] public string description;
    public string objectType;
    [Space]
    public possibleItem[] possibleItems;

    [HideInInspector] public GameObject item;
    [HideInInspector] public int itemValue;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) checkIfGood = true;
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) checkIfGood = false;
    }

    private void Start()
    {
        switch (objectType)
        {
            case "sticker" :
                int rand = Random.Range(1, 4);
                item = possibleItems[rand].prefab;
                itemValue = possibleItems[rand].value;
                gameObject.GetComponent<SpriteRenderer>().sprite = item.GetComponent<SpriteRenderer>().sprite;
                break;
            case "food" : 
                item = possibleItems[0].prefab;
                itemValue = possibleItems[0].value;
                gameObject.GetComponent<SpriteRenderer>().sprite = item.GetComponent<SpriteRenderer>().sprite;
                break;
            case "death" : 
                item = possibleItems[4].prefab;
                itemValue = possibleItems[4].value;
                gameObject.GetComponent<SpriteRenderer>().sprite = item.GetComponent<SpriteRenderer>().sprite;
                break;
        }
    }

    private void Update()
    {
        if (!checkIfGood) return;
       ObjectType();
    }

    void ObjectType()
    {
        if (!Input.GetKeyDown(KeyCode.JoystickButton3)) return;
        if (Input.GetKeyDown(KeyCode.JoystickButton3) && BonusManager.instance.money < itemValue) return;
        
        Instantiate(item, gameObject.transform.position + new Vector3(0,-1f,0), Quaternion.identity);
        BonusManager.instance.GainCoins(-itemValue);
        Destroy(gameObject);
    }
}

[Serializable]
public class possibleItem
{
    public GameObject prefab;
    public int value;
}
