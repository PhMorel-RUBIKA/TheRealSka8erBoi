using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeakPoints : MonoBehaviour
{
    [SerializeField] private FinalBossBehaviour fb;

    void TakeDamage(int damage)
    {
        fb.hpBoss -= damage;
    }

}
