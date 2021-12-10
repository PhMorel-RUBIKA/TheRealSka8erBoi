using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDamage : MonoBehaviour
{
    private Collider2D coll2D;

    void Start()
    {
        coll2D = GetComponent<PolygonCollider2D>();
        StartCoroutine(DealDamage());
    }

    IEnumerator DealDamage()
    {
        yield return new WaitForSeconds(0.3f);
        List<Collider2D> hits = new List<Collider2D>();
        coll2D.OverlapCollider(new ContactFilter2D().NoFilter(), hits);
        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject.CompareTag("Target"))
            {
                hit.GetComponent<DamageManager>().TakeDamage(4);
            }
        }
        StartCoroutine(DealDamage());
    }
}