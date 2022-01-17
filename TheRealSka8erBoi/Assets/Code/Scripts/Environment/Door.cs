using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public int doorNumber;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("PlayerDashing"))
        {
            if (!LoadSceneManager.instance.canChangeRoom) return;

            switch (doorNumber)
            {
                case 0:
                    CallTheChangeRoom();
                    return;
                case 1:
                    CallTheChangeRoom();
                    LoadSceneManager.instance.nextItemToSpawn = WaveManager.instance.itemDoor1;
                    break;
                case 2:
                    CallTheChangeRoom();
                    LoadSceneManager.instance.nextItemToSpawn = WaveManager.instance.itemDoor2;
                    break;
            }
        }
    }

    void CallTheChangeRoom()
    {
        LoadSceneManager.instance.canChangeRoom = false;
        LoadSceneManager.instance.transition.SetTrigger("Start");
        StartCoroutine(LoadSceneManager.instance.ChangeRoom());
    }
}
