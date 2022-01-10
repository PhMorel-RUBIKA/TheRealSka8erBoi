using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForEnemyZone : MonoBehaviour
{

    [SerializeField] private float areaSize;

    [SerializeField] private int damage=10;

    [SerializeField] private float cd = 2;
    // Start is called before the first frame update
    void Start()
    {
        areaSize = transform.localScale.x;
        StartCoroutine(Implosion());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Implosion()
    {
        yield return new WaitForSeconds(cd);
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position,areaSize);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.gameObject.CompareTag("Player"))
            {
                PlayerBehaviour.playerBehaviour.TakeDamage(damage);
            }
        }
    }
}
