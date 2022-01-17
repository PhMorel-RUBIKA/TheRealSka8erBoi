using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    
    private void Awake()
    {
        if (instance == null) instance = this;
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        if (Time.timeScale != 1) Time.timeScale = 1;
    }
}
