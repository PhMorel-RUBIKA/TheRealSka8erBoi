using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    public List<GameObject> closeEnemies;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Target"))
        {
            closeEnemies.Add(other.transform.gameObject); 
        }
    }
    private void Update()
    {
        for(var i = closeEnemies.Count - 1; i > -1; i--)
        {
            if (closeEnemies[i] == null)
                closeEnemies.RemoveAt(i);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Target"))
        {
            closeEnemies.Remove(other.transform.gameObject); 
        }
    }
}
