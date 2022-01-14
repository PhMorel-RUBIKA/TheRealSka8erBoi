using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BossProjectileBehaviour : MonoBehaviour
{
    
    private Transform target;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private int damages = 3;
    [SerializeField] private float speedModifier;
    [SerializeField] private float factorFollow;
    public GameObject playerImpact;
    
     private void Start()
    {
        target = PlayerBehaviour.playerBehaviour.gameObject.transform;
        
        StartCoroutine(Evaporate());
        
    }
    
    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerBehaviour>().TakeDamage(damages);
            gameObject.SetActive(false);
            Instantiate(playerImpact, transform.position, quaternion.identity);
        }
    }
    

    IEnumerator Evaporate()
    {
        yield return new WaitForSeconds(.5f);
        Vector2 direction = Vector2.MoveTowards(rb.velocity, (target.position - transform.position), 0.25f);
        Vector2 adjust = direction.normalized;
        
        transform.rotation = Quaternion.FromToRotation(Vector3.forward, target.position);
        rb.velocity = adjust * factorFollow;
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, speedModifier);
        yield return new WaitForSeconds(3.5f);
        Destroy(gameObject);
    }
}
