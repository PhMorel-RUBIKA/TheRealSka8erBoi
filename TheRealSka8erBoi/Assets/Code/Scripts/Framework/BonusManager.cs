using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BonusManager : MonoBehaviour
{
    public static BonusManager instance;
    public bool isDebug;
    public int redStat, greenStat, blueStat, money, finalScore;
    public TextMeshProUGUI redText, greenText, blueText;
    public TextMeshProUGUI moneyText, finalScoreText;
    public Canvas canvas;
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        redStat = 0;
        redText.text = redStat.ToString();
        
        greenStat = 0;
        greenText.text = greenStat.ToString();
        
        blueStat = 0;
        blueText.text = blueStat.ToString();
        
        money = 0;
        finalScore = 0;
    }

    private void Update()
    {
        redText.text = redStat.ToString();
        greenText.text = greenStat.ToString();
        blueText.text = blueStat.ToString();
    }

    void ActualizeText()
    {
        moneyText.text = money.ToString();
        finalScoreText.text = finalScore.ToString();
    }

    public void GainCoins(int gain)
    {
        money += gain;
        ActualizeText();
    }

    public void GainScore(int score)
    {
        finalScore += score;
        ActualizeText();
    }
    
    
}
