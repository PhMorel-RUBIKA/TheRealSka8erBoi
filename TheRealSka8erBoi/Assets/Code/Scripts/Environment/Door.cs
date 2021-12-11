using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            LoadSceneManager.instance.transition.SetTrigger("Start");
            StartCoroutine(LoadSceneManager.instance.ChangeRoom());
        }
    }
}
