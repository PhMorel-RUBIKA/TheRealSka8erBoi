using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;

public class DamageManager : MonoBehaviour
{
    public AbstComp abstComp;
    private float currentHealth;
    private GameObject player;
    public static DamageManager instance;
    public GameObject spellItem;
    public MMFeedbacks damageFeedback;
    public MMFeedbacks floatingDamage;

    private void Start()
    {
        DamageManager.instance = this;
        currentHealth = this.abstComp.hp;
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
        GameObject spellToGive = Instantiate(spellItem, this.gameObject.transform.position, Quaternion.identity);
        spellToGive.GetComponent<Item>()._inventory = player.GetComponent<Inventory>();
        spellToGive.GetComponent<SpriteRenderer>().sprite = spellItem.GetComponent<Item>().TheItem.SpellItem.spellImage;
    }
}
