using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class SeedManager : MonoBehaviour
{
    [Header("Classic Seed")]
    public bool randomizeSeed;
    public int seed;
    
    [Header("String Seed in case of")]
    public bool useStringSeed;
    public string stringSeed;

    private void Awake()
    {
        if (useStringSeed)
            seed = stringSeed.GetHashCode();
        if (randomizeSeed)
            seed = Random.Range(0, 99999);
        Random.InitState(seed);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
