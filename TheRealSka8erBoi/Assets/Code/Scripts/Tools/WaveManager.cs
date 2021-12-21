using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class WaveManager : MonoBehaviour
{
    
    [Header("Core Attributs")]
    public WaveLevel waveLevel;
    public GameObject gateZone1;
    public GameObject gateZone2;
    public float timeBetweenWave;
    public GameObject enemySpawn;
    public GameObject fireflies;
    [Space]
    [Header("Array Attributs")]
    public Transform[] spawnPoints;
    public GameObject[] rewardObjects;
    
    public GameObject itemDoor1;
    public GameObject itemDoor2;

    private bool canEndwave;
    private bool canSpawn = true;
    private float nextSpawnTime;
    private Wave currentWave;
    private int currentWaveNumber;
    [HideInInspector] public List<int> enemyOnScreen;
    private List<Transform> waypointUsed = new List<Transform>();
    private List<int> currentNumberOfEnemies = new List<int>();
    private GameObject[] probabilityForSpawn;
    public static WaveManager instance;

    private void Start()
    {
        WaveManager.instance = this;
        canEndwave = true;
        for (int i = 0; i < waveLevel.waves.Length; i++) currentNumberOfEnemies.Add(waveLevel.waves[i].numberOfenemies);
    }

    private void Update()
    {
        currentWave = waveLevel.waves[currentWaveNumber];
        StartCoroutine(CoroutineForWave());
        if (enemyOnScreen.Count == 0 && !canSpawn && currentWaveNumber+1 != waveLevel.waves.Length)
        {
            currentWaveNumber ++;
            Debug.Log(currentWaveNumber);
            canSpawn = true;
        }
        else if (enemyOnScreen.Count == 0 && !canSpawn && currentWaveNumber + 1 == waveLevel.waves.Length && canEndwave) EndWave();
    }

    IEnumerator CoroutineForWave()
    {
        yield return new WaitForSeconds(timeBetweenWave);
        SpawnWave();
    }

    void SpawnWave()
    {
        if (canSpawn && nextSpawnTime < Time.time)
        {
            StartCoroutine(SpawnEnemy(GetRandomPoint().position));
            
            enemyOnScreen.Add(1);
            
            currentNumberOfEnemies[currentWaveNumber]--;
            nextSpawnTime = Time.time + currentWave.spawnInterval;

            if (currentNumberOfEnemies[currentWaveNumber] == 0) canSpawn = false;
        }
    }

    IEnumerator SpawnEnemy(Vector3 position)
    {
        GameObject randomEnemy = GetRandomEnemy();
        Instantiate(enemySpawn, position + new Vector3(-0.05f, -0.5f, 0), Quaternion.identity);
        yield return new WaitForSeconds(0.8f);
        Instantiate(randomEnemy, position, Quaternion.identity);
    }

    GameObject GetRandomEnemy()
    {
        int i = 0;
        int value = 0;
        int random = Random.Range(0,100);

        while ( random > currentWave.typeOfEnnemies[i].probability + value)
        {
            value += currentWave.typeOfEnnemies[i].probability;
            i++;
        }

        return currentWave.typeOfEnnemies[i].enemy;
    }

    Transform GetRandomPoint()
    {
        Transform randomPoint = null;
        int index = Random.Range(0, spawnPoints.Length);

        while (waypointUsed.Contains(spawnPoints[index]))
            index = Random.Range(0, spawnPoints.Length);

        randomPoint = spawnPoints[index];
        waypointUsed.Add(spawnPoints[index]);

        return randomPoint;
    }

    void EndWave()
    {
        if (LoadSceneManager.instance.nextItemToSpawn != null)
            Instantiate(LoadSceneManager.instance.nextItemToSpawn, BonusManager.instance.gameObject.transform.position, Quaternion.identity);

        LoadSceneManager.instance.nextItemToSpawn = null;
        itemDoor1 = null;
        gateZone1.GetComponent<SpriteRenderer>().sprite = null;
        itemDoor2 = null;
        gateZone1.GetComponent<SpriteRenderer>().sprite = null;
        
        int rand1 = Random.Range(0, rewardObjects.Length);
        int rand2 = Random.Range(0, rewardObjects.Length);
        while (rand2 == rand1) rand2 = Random.Range(0, rewardObjects.Length);

        itemDoor1 = rewardObjects[rand1];
        gateZone1.GetComponent<SpriteRenderer>().sprite = itemDoor1.GetComponent<SpriteRenderer>().sprite;
        itemDoor2 = rewardObjects[rand2];
        gateZone2.GetComponent<SpriteRenderer>().sprite = itemDoor2.GetComponent<SpriteRenderer>().sprite;
        
        RandomItem(itemDoor1);
        RandomItem(itemDoor2);
        
        gateZone1.SetActive(true);
        gateZone2.SetActive(true);

        if (fireflies != null) fireflies.SetActive(true);

        int randMoney = Random.Range(30, 40); 
        BonusManager.instance.GainCoins(randMoney);
        
        canEndwave = false;
    }

    void RandomItem(GameObject item)
    {
        switch (item.GetComponent<Item>().TheItem.TypeOfItem)
        {
            case typeOfItem.STICKER_RED:
                item.GetComponent<Item>().TheItem.stickerRed.value += 1;
                break;
            case typeOfItem.STICKER_GREEN:
                item.GetComponent<Item>().TheItem.stickerGreen.value += 1;
                break;
            case typeOfItem.STICKER_BLUE:
                item.GetComponent<Item>().TheItem.stickerBlue.value += 1;
                break;
            case typeOfItem.COINS :
                item.GetComponent<Item>().TheItem.CoinItem.value += (int)(200 + LoadSceneManager.instance.numberOfRoom * 2f);
                break;
        } 
    }
}