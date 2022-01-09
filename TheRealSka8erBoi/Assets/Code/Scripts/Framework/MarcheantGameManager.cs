using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarcheantGameManager : MonoBehaviour
{
    public GameObject canvas1, canvas2;
    public Animation animation;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canvas1.SetActive(true);
        }
    }

    public IEnumerator SceneChangement()
    {
        canvas1.SetActive(false);
        canvas2.SetActive(true);
        
        CameraShake.instance.StartShake(12f, 2f, 7f);

        yield return new WaitForSeconds(3);
        canvas2.SetActive(false);
        Debug.Log("AlgériePourLaVie");
        
        LoadSceneManager.instance.transition.SetTrigger("Start");
        StartCoroutine(LoadSceneManager.instance.ChangeRoom());
    }
}
