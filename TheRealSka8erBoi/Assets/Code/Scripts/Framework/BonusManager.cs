using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BonusManager : MonoBehaviour
{
    public static BonusManager instance;
    public bool isDebug;
    public int redStat, greenStat, blueStat, money;
    public TextMeshProUGUI redText, greenText, blueText, moneyText;
    public Canvas canvas;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        redStat = 0;
        greenStat = 0;
        blueStat = 0;
        money = 0;
        
        switch (isDebug)
        {
            case true :
                IsDebug(true);
                break;
            case false :
                IsDebug(false);
                break;
        }
    }

    private void Update()
    {
        if(isDebug) ActualizeText();
    }

    void IsDebug(bool isActive)
    {
        canvas.gameObject.SetActive(isActive);
    }

    void ActualizeText()
    {
        redText.text = redStat.ToString();
        greenText.text = greenStat.ToString();
        blueText.text = blueStat.ToString();
        moneyText.text = money.ToString();
    }
}
