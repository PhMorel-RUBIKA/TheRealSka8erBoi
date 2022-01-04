using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundCaller : MonoBehaviour
{
    public static SoundCaller instance;
    
    [Header("Step Sound")]
    public float timeBetweenSoundsStep = 0.3f;
    public AudioSource stepAudioSource;
    public AudioClip[] stepSound;

    [Space]
    
    [Header("Dash Sound")]
    public AudioSource dashAudioSource;
    public AudioClip dashSound;
    
    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public void StepSound()
    {
        int rand = Random.Range(0, 2);
        stepAudioSource.PlayOneShot(stepSound[rand]);
    }

    public void DashSound()
    {
        dashAudioSource.PlayOneShot(dashSound);
    }
}
