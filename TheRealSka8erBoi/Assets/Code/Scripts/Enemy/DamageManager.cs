using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageManager : MonoBehaviour
{
    public AbstComp abstComp;
    private float currentHealth;
    private GameObject player;
    public static DamageManager instance;
    public GameObject spellItem;

    private void Start()
    {
        DamageManager.instance = this;
        currentHealth = this.abstComp.hp;
        player = GameObject.FindWithTag("Player");
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
        GiveTheSpell();
        Destroy(this.gameObject);
        WaveManager.instance.enemyOnScreen.RemoveAt(0);
    }

    void GiveTheSpell()
    {
        int random = Random.Range(0, 2);

        if (random == 1)
        {
            GameObject spellToGive = Instantiate(spellItem, this.gameObject.transform.position, Quaternion.identity);
            spellToGive.GetComponent<Item>()._inventory = player.GetComponent<Inventory>();
            spellToGive.GetComponent<SpriteRenderer>().sprite =
                spellItem.GetComponent<Item>().TheItem.SpellItem.spellImage;
        }
    }
}
