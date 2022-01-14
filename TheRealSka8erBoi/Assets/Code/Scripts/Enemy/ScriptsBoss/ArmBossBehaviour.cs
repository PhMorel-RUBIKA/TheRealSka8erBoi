using DG.Tweening;
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
        Destroy(gameObject,3f);
    }
    

    private void Update()
    {
        transform.DOScale(new Vector3(10, 10, 1), 1f).SetEase(Ease.OutBack);
    }

    private void ShockWave()
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
        if (other.gameObject.CompareTag("Target"))
        {
            other.GetComponent<DamageManager>().TakeDamage(damage);
            other.gameObject.GetComponent<Rigidbody2D>().AddForce(projection*strenght);
        }
    }
}
