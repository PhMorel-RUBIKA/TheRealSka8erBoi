using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class Faucheuse : Spell
{
    public float attackRange;
    public int damage;
    public GameObject VFX;


    public override void Activate(GameObject parent)
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(parent.transform.position, attackRange);
        Instantiate(VFX, parent.transform);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.gameObject.CompareTag("Target"))
            {
                enemy.GetComponent<DamageManager>().TakeDamage(damage); 
            }
        }
    }

    /*public override void BeginCooldown(GameObject parent)
    {
        spellImage.GetComponent<Image>().fillAmount -= 1 / cooldownTime * Time.deltaTime;

        if (spellImage.GetComponent<Image>().fillAmount <= 0)
        {
            spellImage.GetComponent<Image>().fillAmount = 0;
        }
    }*/

    public override void Gizmo(GameObject parent)
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(parent.transform.position, attackRange);
    }
}