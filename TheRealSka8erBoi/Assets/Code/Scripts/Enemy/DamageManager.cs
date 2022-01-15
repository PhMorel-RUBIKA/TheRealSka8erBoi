using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;

public class DamageManager : MonoBehaviour
{
    public AbstComp abstComp;
    public float currentHealth;
    private GameObject player;
    public GameObject spellItem;
    public MMFeedbacks damageFeedback;
    public MMFeedbacks floatingDamage;

    private void Start()
    {
        //currentHealth = this.abstComp.hp;
        player = GameObject.FindWithTag("Player");
        if (player == null)
        {
            player = GameObject.FindWithTag("PlayerDashing");
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        damageFeedback.PlayFeedbacks();
        if (floatingDamage.TryGetComponent(out MMFeedbackFloatingText feedbackFloatingText))
        {
            feedbackFloatingText.Value = damage.ToString();
        }
        floatingDamage.PlayFeedbacks();
        
        CameraShake.instance.StartShake(0.05f, 0.05f, 3f);
        SoundCaller.instance.EnemiesDamage();
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        GiveTheSpell();
        BonusManager.instance.GainScore(Random.Range(200, 251));
        BonusManager.instance.GainCoins(Random.Range(2,8));
        if (WaveManager.instance.enemyOnScreen.Count != 0) WaveManager.instance.enemyOnScreen.RemoveAt(0);
        Destroy(this.gameObject);
    }

    void GiveTheSpell()
    {
        int rand = Random.Range(1,5);
        if (rand != 2) return;
        GameObject spellToGive = Instantiate(spellItem, this.gameObject.transform.position, Quaternion.identity);
        spellToGive.GetComponent<Item>()._inventory = player.GetComponent<Inventory>();
        spellToGive.GetComponent<SpriteRenderer>().sprite = spellItem.GetComponent<Item>().TheItem.SpellItem.spellImage;
    }
}
