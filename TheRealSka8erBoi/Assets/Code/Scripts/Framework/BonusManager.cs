using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BonusManager : MonoBehaviour
{
    public static BonusManager BonusManagerInstance;
    public int redStat, greenStat, blueStat;
    public TextMeshProUGUI redText, greenText, blueText;

    private void Awake()
    {
        if (BonusManagerInstance == null)
            BonusManagerInstance = this;
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
