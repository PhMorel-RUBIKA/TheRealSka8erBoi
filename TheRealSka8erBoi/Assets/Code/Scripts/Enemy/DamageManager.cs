using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManager : MonoBehaviour
{
    public AbstComp abstComp;
    private float currentHealth;
    public static DamageManager instance;

    private void Start()
    {
        DamageManager.instance = this;
        currentHealth = this.abstComp.hp;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(this.gameObject);
        //WaveManager.instance.enemyOnScreen.RemoveAt(0);
    }
}
