using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockwaveBehaviour : MonoBehaviour
{
    public int damage = 10;
    public float force = 10;
    public float radius = 10f;
    void Start()
    {
        StartCoroutine(Explosion());
    }

    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(0.3f);
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject.CompareTag("Target"))
            {
                if (hit.gameObject.GetComponent<DamageManager>() != null) hit.gameObject.GetComponent<DamageManager>().TakeDamage(damage);
                else hit.gameObject.GetComponent<handScript>().TakeDamage(damage);
                hit.GetComponent<Rigidbody2D>().AddForce( (hit.transform.position - transform.position).normalized* force);
            }

            if (hit.gameObject.CompareTag("EnemyBullet"))
            {
                Destroy(hit.gameObject);
            }
        }
    }
}
