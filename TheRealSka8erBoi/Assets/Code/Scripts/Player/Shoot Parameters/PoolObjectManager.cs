using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObjectManager : MonoBehaviour
{
    // Création de la Class Bullet qui permet de renseigner les différents types de balles, avec le nom, le prefab ainsi que la taille de la pool
    [Serializable]
    public class Bullet
    {
        public string name;
        public GameObject prefab;
        public int size;
    }
    
    // Création d'une class copie de Bullet mais en tant que prefab afin de plus facilement accéder aux données via les autres scripts, notamment avec le BulletPoolBehavior
    [Serializable]
    public class BulletPrefab
    {
        public string name;
        public GameObject prefab;
        public Transform prefabParent;

        public BulletPrefab(string name, GameObject prefab, Transform prefabParent)
        {
            this.name = name;
            this.prefab = prefab;
            this.prefabParent = prefabParent; 
        }
    }
    
    // Mise en instance du script afin de l'accèder via d'autre script
    public static PoolObjectManager Instance;
    
    // Mise en place du Dictionnary, fonctionnement du Pool 
    public List<Bullet> bullets;
    public Dictionary<string, Queue<GameObject>> PoolDictionary;
    [HideInInspector] List<BulletPrefab> prefabList = new List<BulletPrefab>(); 
    
    //Mise en place de l'instance
    private void Awake()
    {
        if (Instance == null) Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    //Création de la pool
    private void Start()
    {
        PoolDictionary = new Dictionary<string, Queue<GameObject>>();
        
        //En gros, pour chaque balle mis dans la classe, tu créés un parent qui va contenir toutes les balles et tu mets dans la pool tous les objects que tu mets
        foreach (Bullet bullet in bullets)
        {
            GameObject bulletParent = new GameObject();
            bulletParent.name = bullet.name + "Pool";
            GameObject.DontDestroyOnLoad(bulletParent);
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < bullet.size; i++)
            {
                GameObject obj = Instantiate(bullet.prefab, bulletParent.transform);
                obj.GetComponent<BulletPoolBehaviour>().bulletName = bullet.name;
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            PoolDictionary.Add(bullet.name, objectPool);
            prefabList.Add(new BulletPrefab(bullet.name, bullet.prefab, bulletParent.transform));
        }
    }

    //Ceci correspond à la fonction à call lorsque que tu veux réaliser ton tir
    public GameObject GetBullet(string bulletName, Vector2 position, Quaternion rotation)
    {
        if (PoolDictionary.ContainsKey(bulletName))
        {
            if (PoolDictionary[bulletName].Count > 0)
            {
                GameObject obj = PoolDictionary[bulletName].Dequeue();
                obj.transform.position = position;
                obj.transform.rotation = rotation;
                obj.SetActive(true);
                return obj;
            }
            else
            {
                for (int i = 0; i < prefabList.Count; i++)
                {
                    if (bulletName == prefabList[i].name)
                    {
                        GameObject obj = Instantiate(prefabList[i].prefab, prefabList[i].prefabParent);
                        obj.GetComponent<BulletPoolBehaviour>().bulletName = bulletName;
                        PoolDictionary[bulletName].Enqueue(obj);


                        PoolDictionary[bulletName].Dequeue();
                        obj.transform.position = position;
                        obj.transform.rotation = rotation;
                        return obj;
                    }
                }
            }
        }

        return null;
    }
    
    //Fonction a call lorsque tu veux "détruire" la bullet
    public void DestroyBullet(string bulletName, GameObject poolObject)
    {
        if (PoolDictionary.ContainsKey(bulletName))
        {
            PoolDictionary[bulletName].Enqueue(poolObject);
            poolObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            poolObject.SetActive(false);
        }
    }
}