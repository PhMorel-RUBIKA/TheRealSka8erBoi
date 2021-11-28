using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveEffect : MonoBehaviour
{
    [SerializeField] private Material material;
    
    private float dissolveSpeed;
    private float dissolveAmount;
    private bool isDissolving;

    private void Update()
    {
        if (isDissolving)
        {
            dissolveAmount = Mathf.Clamp01(dissolveAmount + dissolveSpeed * Time.deltaTime); 
            material.SetFloat("_DissolveAmount", dissolveAmount);
        }
        else
        {
            dissolveAmount = Mathf.Clamp01(dissolveAmount - dissolveSpeed * Time.deltaTime); 
            material.SetFloat("_DissolveAmount", dissolveAmount);
        }
    }
    
    public void StartDissolve(float dissolveSpeed)
    {
        isDissolving = true;
        this.dissolveSpeed = dissolveSpeed; 
    }

    public void StopDissolve(float dissolveSpeed)
    {
        isDissolving = false;
        this.dissolveSpeed = dissolveSpeed;
    }
}
