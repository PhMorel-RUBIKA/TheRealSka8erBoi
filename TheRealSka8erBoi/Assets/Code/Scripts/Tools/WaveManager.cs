using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class WaveManager : MonoBehaviour
{
    public Transform[] spawnPoints;
    public WaveLevel waveLevel;
    public GameObject gate;

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

        for (int i = 0; i < waveLevel.waves.Length; i++)
        {
            currentNumberOfEnemies.Add(waveLevel.waves[i].numberOfenemies);
        }
        
    }

    private void Update()
    {
        currentWave = waveLevel.waves[currentWaveNumber];
        SpawnWave();
        if (enemyOnScreen.Count == 0 && !canSpawn && currentWaveNumber+1 != waveLevel.waves.Length)
        {
            currentWaveNumber ++;
            waypointUsed.Clear();
            canSpawn = true;
        }

        if (currentWaveNumber + 1 == waveLevel.waves.Length)
            gate.SetActive(true);
    }

    void SpawnWave()
    {
        if (canSpawn && nextSpawnTime < Time.time)
        {
            GameObject randomEnemy = GetRandomEnemy();
            Instantiate(randomEnemy, GetRandomPoint().position, quaternion.identity);
            enemyOnScreen.Add(1);
            
            currentNumberOfEnemies[currentWaveNumber]--;
            nextSpawnTime = Time.time + currentWave.spawnInterval;

            if (currentNumberOfEnemies[currentWaveNumber] == 0)
            {
                canSpawn = false;
            }
        }
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
        {
            index = Random.Range(0, spawnPoints.Length);
        }
        
        randomPoint = spawnPoints[index];
        waypointUsed.Add(spawnPoints[index]);

        return randomPoint;
    }
    
}