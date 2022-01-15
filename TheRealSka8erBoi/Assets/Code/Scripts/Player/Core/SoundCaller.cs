using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundCaller : MonoBehaviour
{
    public static SoundCaller instance;

    [Header("Player Maj")] public AudioClip pickUpItems;
    
    [Space]
    
    [Header("Step Sound")]
    public float timeBetweenSoundsStep = 0.3f;
    public AudioSource stepAudioSource;
    public AudioClip[] stepSound;

    [Space]
    
    [Header("Dash Sound")]
    public AudioSource dashAudioSource;
    public AudioClip dashSound;
    
    [Space]
    
    [Header("EnnemiSound")]
    public AudioSource gameManagerAudioSource;
    public AudioClip[] enemyDamageSound;
    public AudioClip[] spawnEnemies;
    public AudioClip damageSound;
    
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

    public void EnemiesShotSound()
    {
        int rand = Random.Range(0, enemyDamageSound.Length); 
        gameManagerAudioSource.PlayOneShot(enemyDamageSound[rand]);
    }
    
    public void EnemiesDamage()
    {
        gameManagerAudioSource.PlayOneShot(damageSound);
    }

    public IEnumerator SpawnEnemiesSound()
    {
        gameManagerAudioSource.PlayOneShot(spawnEnemies[0]);
        yield return new WaitForSeconds(0.5f);
        gameManagerAudioSource.PlayOneShot(spawnEnemies[1]);
    }

    public void PickUpItemsSound()
    {
        gameManagerAudioSource.PlayOneShot(pickUpItems);
    }
}
