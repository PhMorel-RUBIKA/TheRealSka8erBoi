using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorPROTO : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            LoadSceneManagerPROTO.LoadSceneManagerProtoInstance.transition.SetTrigger("Start");
            StartCoroutine(LoadSceneManagerPROTO.LoadSceneManagerProtoInstance.ChangeRoom());
        }
    }
}
