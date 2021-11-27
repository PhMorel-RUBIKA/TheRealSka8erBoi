using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletPool : MonoBehaviour
{
    
    public EnemyBulletPool enemyBulletPoolInstance;
    public GameObject pooledBullet;
    public GameObject pooledFollower;
    
    private bool notEnoughtfollower = true;
    private bool notEnoughtbullet = true;
    private List<GameObject> enemyBullets;
    private List<GameObject> followerBullets;

   private void Awake()
   {
       enemyBulletPoolInstance = this;
   }

   void Start()
   {
       enemyBullets = new List<GameObject>();
       followerBullets = new List<GameObject>();
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
             if(notEnoughtbullet)
             {
                 GameObject bul = Instantiate(pooledBullet);
                 bul.SetActive(false);
                 enemyBullets.Add(bul);
                 return bul;
             }
             return null;
         }
   
   public GameObject GetFollowBullet()
    {
        if(followerBullets.Count >0)
        {
            for (int y = 0; y < followerBullets.Count; y++)
            {
                if (!followerBullets[y].activeInHierarchy)
                {
                    return followerBullets[y];
                }
            }
        }
        if(notEnoughtfollower)
        {
            GameObject fol = Instantiate(pooledFollower);
            fol.SetActive(false);
            followerBullets.Add(fol);
            return fol;
        }
        return null;
    }

}
