using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileScript : MonoBehaviour
{
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Border"))
        {
            Destroy(gameObject);
        }

        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerBehaviour>().TakeDamage(5);
            StartCoroutine(CameraShake.instance.Shake(1, 0.5f));
            StartCoroutine(CameraShake.instance.Shake(1, 0.5f));

            Destroy(gameObject);
        }
    }

}
