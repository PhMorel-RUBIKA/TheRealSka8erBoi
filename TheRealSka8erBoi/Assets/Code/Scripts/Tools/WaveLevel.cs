using System;
using UnityEngine;

[Serializable]
public class Wave
{ 
    /* Création de la classe Wave.
     * Cela correpond à une wave entière avec l'ensemble de ses informations
     */
    public string waveName; // Un nom attribué à la wave, purement présent dans l'inspector
    public float spawnInterval; // Le temps d'interval entre le spawn de chaque ennemies
    public int numberOfenemies; // Le nombre d'ennemies à faire spawn dans la wave
    [Space]
    public TypeOfEnnemies[] typeOfEnnemies; // Se référer à la classe correspondante
}

[Serializable]
public class TypeOfEnnemies
{
    /* Création de la class Type d'Ennemies
     * Cela correspond aux informations relatives à chacuns des ennemies que l'on veut spawn dans la wave
     */
    
    public GameObject enemy; // Prefab de l'ennemi en question
    [Range(0,100)] public int probability = 0; // Sa chance d'apparition de spawn, en pourcentage sur 100
}

[CreateAssetMenu]
public class WaveLevel : ScriptableObject
{
    public Wave[] waves;
}
