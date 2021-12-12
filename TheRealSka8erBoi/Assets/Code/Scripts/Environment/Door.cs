using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public int doorNumber;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            LoadSceneManager.instance.transition.SetTrigger("Start");
            StartCoroutine(LoadSceneManager.instance.ChangeRoom());
            
            switch (doorNumber)
            {
                case 0:
                    return;
                case 1:
                    LoadSceneManager.instance.nextItemToSpawn = WaveManager.instance.itemDoor1;
                    break;
                case 2:
                    LoadSceneManager.instance.nextItemToSpawn = WaveManager.instance.itemDoor2;
                    break;
            }
        }
    }
}
