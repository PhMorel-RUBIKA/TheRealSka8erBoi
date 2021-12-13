using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(DestroyElObject());
    }

    IEnumerator DestroyElObject()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(this.gameObject);
    }
}
