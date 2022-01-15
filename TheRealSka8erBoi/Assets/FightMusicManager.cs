using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightMusicManager : MonoBehaviour
{
    [SerializeField] private AudioSource intro;

    [SerializeField] private AudioSource fightloop;

    public bool playsong = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!intro.isPlaying & !fightloop.isPlaying)
        {
            LoopClip();
            playsong = true;

        }
    }

    void LoopClip()
    {
        if (playsong)
        {
           fightloop.Play();
           playsong = false;
        }
        
    }
}
