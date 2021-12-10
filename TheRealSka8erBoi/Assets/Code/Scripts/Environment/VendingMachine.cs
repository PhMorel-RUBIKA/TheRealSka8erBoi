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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) checkIfGood = true;
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) checkIfGood = false;
    }

    private void Update()
    {
        if (!checkIfGood) return;
        switch (objectType)
        {
            case "sticker" :
                int rand = Random.Range(1, 4);
                ObjectType(rand);
                break;
            case "food" : 
                ObjectType(0);
                break;
            case "death" : 
                ObjectType(4);
                break;
        }
    }

    void ObjectType(int number)
    {
        if (!Input.GetKeyDown(KeyCode.JoystickButton3)) return;
        if (Input.GetKeyDown(KeyCode.JoystickButton3) && BonusManager.instance.money < possibleItems[number].value) return;
        
        Instantiate(possibleItems[number].prefab, gameObject.transform.position, Quaternion.identity);
        BonusManager.instance.money -= possibleItems[number].value;
        Destroy(gameObject);
    }
}

[Serializable]
public class possibleItem
{
    public GameObject prefab;
    public int value;
}
