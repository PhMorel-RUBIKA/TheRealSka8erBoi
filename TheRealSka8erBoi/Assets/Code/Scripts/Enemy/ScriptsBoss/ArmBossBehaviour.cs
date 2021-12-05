using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.NiceVibrations;
using UnityEditorInternal.VersionControl;
using UnityEngine;

public class ArmBossBehaviour : MonoBehaviour
{
    [Header("Behaviour")] 
    [SerializeField] private FinalBossBehaviour fbb;
    [SerializeField] private Transform target;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float moveSpeed;
    [Space]
    [Header("AtckParameters")]
    [SerializeField] private float strenght;
    [SerializeField] private int damage = 7;
    private int pushValue;
    
    
    // Start is called before the first frame update
    void Start()
    {
        target = PlayerBehaviour.playerBehaviour.transform;
        ShockWave();
        if (fbb.bossIsMidLife)
        {
            damage *= 2;
        }
    }
    

    private void Update()
    {
        
    }

    private void ShockWave()
    {
       
        var toplayer = new Vector2(target.position.x-transform.position.x, transform.position.y);
        if (toplayer.x>0)
        {
            rb.AddForce(Vector2.right * moveSpeed);
            pushValue = 1;
        }
        else if (toplayer.x<0)
        {
            rb.AddForce(Vector2.left* moveSpeed);
            pushValue = 2;
        }

    }
    
        private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerBehaviour.playerBehaviour.TakeDamage(damage);
            if(pushValue==1)
            {
                other.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.right*strenght);
            }
            else if (pushValue==2)
            {
                other.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.left*strenght);
            }
            Destroy(gameObject);

        }

    }
}
