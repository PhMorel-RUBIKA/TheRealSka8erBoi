using DG.Tweening;
using UnityEngine;

public class ArmBossBehaviour : MonoBehaviour
{
    [Header("Behaviour")] 
    [SerializeField] private FinalBossBehaviour fbb;
    [SerializeField] private Transform target;

    [Space] [Header("AtckParameters")] 
    [SerializeField] private CircleCollider2D zone;
    [SerializeField] private float strenght;
    [SerializeField] private int damage = 5;
    private int pushValue;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        target = PlayerBehaviour.playerBehaviour.transform;
        
        if (fbb.bossIsMidLife)
        {
            damage *= 2;
        }
        Destroy(gameObject,.5f);
    }
    

    private void Update()
    {
        
    }
    
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        var projection = new Vector2(other.transform.position.x-transform.position.x, other.transform.position.y - transform.position.y);
        if (other.gameObject.CompareTag("Player")) 
        {
            PlayerBehaviour.playerBehaviour.TakeDamage(damage);
            other.gameObject.GetComponent<Rigidbody2D>().AddForce(projection*strenght);
            Destroy(this);
        }
        /*if (other.gameObject.CompareTag("Target"))
        {
            other.GetComponent<DamageManager>().TakeDamage(damage);
            other.gameObject.GetComponent<Rigidbody2D>().AddForce(projection*strenght);
        }*/
    }
}
