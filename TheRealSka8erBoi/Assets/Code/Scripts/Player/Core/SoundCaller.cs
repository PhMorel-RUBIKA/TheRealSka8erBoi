using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SoundCaller : MonoBehaviour
{
    public static SoundCaller instance;
    public float volumeSD;
    public float volumeMusic;
    public AudioSource musicAudioSource;

    [Header("Player Maj")] 
    public AudioClip pickUpItems;
    public AudioClip playerDeath;
    public AudioClip lacheTir;
    public AudioClip spellSound;
    
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

    private void Update()
    {
        volumeSD = Mathf.Clamp(volumeSD, 0f, 0.05f);
        gameManagerAudioSource.volume = volumeSD;
        dashAudioSource.volume = volumeSD;
        stepAudioSource.volume = volumeSD;

        if (musicAudioSource == null)
        {
            GameObject audioSource = GameObject.FindWithTag("WorldTag");
            musicAudioSource = audioSource.GetComponent<AudioSource>();
        }
        
        volumeMusic = Mathf.Clamp(volumeMusic, 0f, 0.30f);
        musicAudioSource.volume = volumeMusic;
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

    public void SpawnEnemiesSound()
    {
        gameManagerAudioSource.PlayOneShot(spawnEnemies[0]);
    }

    public void PickUpItemsSound()
    {
        gameManagerAudioSource.PlayOneShot(pickUpItems);
    }

    public void PlayerDeath()
    {
        gameManagerAudioSource.PlayOneShot(playerDeath);
    }

    public void PLayerTirSound()
    {
        gameManagerAudioSource.PlayOneShot(lacheTir);
    }
    
    public void SpellSound()
    {
        gameManagerAudioSource.PlayOneShot(spellSound);
    }

    public void SliderSD(float volume)
    {
        volumeSD = volume;
    }

    public void SliderMusic(float volume)
    {
        volumeMusic = volume;
    }
}
