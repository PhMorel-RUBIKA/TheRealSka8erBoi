using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundCaller : MonoBehaviour
{
    public static SoundCaller instance;
    
    public float timeBetweenSoundsStep = 0.3f;
    public AudioSource audioSource;
    public AudioClip[] stepSound;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public void StepSound()
    {
        int rand = Random.Range(0, 2);
        audioSource.PlayOneShot(stepSound[rand]);
    }
}
