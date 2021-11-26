using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BonusManager : MonoBehaviour
{
    public static BonusManager instance;
    public int redStat, greenStat, blueStat;
    public TextMeshProUGUI redText, greenText, blueText;

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
    }

    private void Update()
    {
        ActualizeText();
    }

    void ActualizeText()
    {
        redText.text = redStat.ToString();
        greenText.text = greenStat.ToString();
        blueText.text = blueStat.ToString();
    }
}
