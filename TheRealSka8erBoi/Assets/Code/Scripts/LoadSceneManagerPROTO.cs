using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManagerPROTO : MonoBehaviour
{
    public List<string> roomOrder;
    public int numberOfRoom;
    
    public void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        numberOfRoom = 0;
    }
    
    public void ChangeRoom()
    { 
        SceneManager.LoadSceneAsync(roomOrder[numberOfRoom]);
        numberOfRoom++;
    }
}
