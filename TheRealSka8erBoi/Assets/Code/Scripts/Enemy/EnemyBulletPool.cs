using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletPool : MonoBehaviour
{
    
    public EnemyBulletPool enemyBulletPoolInstance;
    public GameObject pooledBullet; 
    private bool notEnought = true; 
    private List<GameObject> enemyBullets;

   private void Awake()
   {
       enemyBulletPoolInstance = this;
   }

   void Start()
   {
       enemyBullets = new List<GameObject>();
   }
   
   public GameObject GetBullet()
    {
        if(enemyBullets.Count >0)
        {
            for (int i = 0; i < enemyBullets.Count; i++)
            {
                if (!enemyBullets[i].activeInHierarchy)
                {
                    return enemyBullets[i];
                }
            }
        }
        if(notEnought)
        {
            GameObject bul = Instantiate(pooledBullet);
            bul.SetActive(false);
            enemyBullets.Add(bul);
            return bul;
        }
        return null;
    }

}
