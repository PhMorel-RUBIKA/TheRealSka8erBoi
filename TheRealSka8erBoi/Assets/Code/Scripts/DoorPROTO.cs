using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorPROTO : MonoBehaviour
{
    public LoadSceneManagerPROTO LoadSceneManagerProto;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Porte Franchi COLLIDER");
            LoadSceneManagerProto.ChangeRoom();
        }
    }
}
