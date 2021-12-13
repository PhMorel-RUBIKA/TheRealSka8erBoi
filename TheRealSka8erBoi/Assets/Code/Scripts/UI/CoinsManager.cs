using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinsManager : MonoBehaviour
{
    private Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
        StartCoroutine(CoinFlip());
        
        
    }

    void Update()
    {
        
    }

    IEnumerator CoinFlip()
    {
        yield return new WaitForSeconds(7);
        anim.Play("CoinFlip",0,0);
        StartCoroutine(CoinFlip());
    }
}
