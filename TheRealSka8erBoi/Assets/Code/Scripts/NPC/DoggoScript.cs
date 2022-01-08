using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoggoScript : MonoBehaviour
{
    public MarcheantGameManager Manager;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.JoystickButton1))
            {
                StartCoroutine(Manager.SceneChangement()); 
            }
        }
    }
}
