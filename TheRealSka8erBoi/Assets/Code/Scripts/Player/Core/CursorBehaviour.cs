using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorBehaviour : MonoBehaviour
{
    [SerializeField] Vector2 leftJoy;
    public GameObject EnemyDetector;
    [SerializeField]private float angle;
    private Quaternion target;
    private Vector2 closestDirection;
    private List<Vector2> closeEnemies;
    private Vector2 closestEnemy;
    private float minAngle;
    [SerializeField] private float smoothSpeed;
    [SerializeField] private float autoAimField;
    
    

    private void OnEnable()
    {
        /*leftJoy = GetComponentInParent<PlayerBehaviour>().latestDirection;
        transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(leftJoy.y, leftJoy.x) * Mathf.Rad2Deg + 90);
        closestEnemy = new Vector2(200, 200);
        if (GetClosestEnemies().Count != 0)
        {
            foreach (var direc in GetClosestEnemies())
            {
                if (Mathf.Abs(direc.x) + Mathf.Abs(direc.y) < Mathf.Abs(closestEnemy.x) + Mathf.Abs(closestEnemy.y))
                {
                    closestEnemy = direc;
                }
            }
            transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(closestEnemy.y, closestEnemy.x) * Mathf.Rad2Deg + 90);
        }*/
        transform.rotation = AimDirection().Item1;
    }
    
    void FixedUpdate()
     {
         if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.3f || Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0.3f)
         {
             leftJoy.x = Input.GetAxisRaw("Horizontal");
             leftJoy.y = Input.GetAxisRaw("Vertical");
         }
         target = AimDirection().Item1; 
         transform.rotation = Quaternion.Lerp(transform.rotation, target, smoothSpeed);
     }

    

    private Vector2 GetClosestEnemy()
     {
         Transform closestEnemy = null;
         float minDist = Mathf.Infinity;
         foreach (GameObject enemy in EnemyDetector.GetComponent<EnemyDetector>().closeEnemies)
         {
             float dist = Vector2.Distance(enemy.transform.position, transform.position);
             if (minDist > dist )
             {
                 minDist = dist;
                 closestEnemy = enemy.transform;
             }
         }

         if (closestEnemy != null)
         {
             return new Vector2(closestEnemy.position.x - transform.position.x, closestEnemy.position.y - transform.position.y);   
         }
         else
         {
             return Vector2.zero;
         }

     }
     
     
     private List<Vector2> GetClosestEnemies()
     {
         List<Vector2> direcCloseEnemies = new List<Vector2>();
         foreach (GameObject enemy in EnemyDetector.GetComponent<EnemyDetector>().closeEnemies)
         {
             direcCloseEnemies.Add(new Vector2(enemy.transform.position.x - transform.position.x, enemy.transform.position.y - transform.position.y));
         }

         return direcCloseEnemies;
     }

     public Tuple<Quaternion,Vector2> AimDirection()
     {
         closeEnemies = GetClosestEnemies();

         if (closeEnemies.Count != 0)
         {
             minAngle = 200;
             foreach (Vector2 dirEnemy in closeEnemies)
             {
                 
                 angle = Vector2.Angle(new Vector2(dirEnemy.x, dirEnemy.y), leftJoy);
                 
                 if (minAngle > angle)
                 {
                     minAngle = angle;
                     closestDirection = dirEnemy;
                 }
                 
             }
             if (minAngle < autoAimField)
             {
                 angle = Mathf.Atan2(closestDirection.y, closestDirection.x) * Mathf.Rad2Deg;
                 return Tuple.Create(Quaternion.Euler(0f,0f,angle),closestDirection);
                 
             }
             else
             {
                 angle = Mathf.Atan2(leftJoy.y, leftJoy.x) * Mathf.Rad2Deg;
                 return Tuple.Create(Quaternion.Euler(0f,0f,angle),leftJoy);
             }
         }
         else
         {
             angle = Mathf.Atan2(leftJoy.y, leftJoy.x) * Mathf.Rad2Deg;
             return Tuple.Create(Quaternion.Euler(0f,0f,angle),leftJoy);
         }
     }
}
