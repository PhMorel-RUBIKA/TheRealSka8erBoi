using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManagerPROTO : MonoBehaviour
{
    public List<string> roomOrder;
    public int numberOfRoom;
    public static LoadSceneManagerPROTO LoadSceneManagerProtoInstance;
    
    public void Awake()
    {
        if (LoadSceneManagerProtoInstance == null)
        {
            LoadSceneManagerPROTO.LoadSceneManagerProtoInstance = this;
        }
        
        DontDestroyOnLoad(this.gameObject);
        numberOfRoom = 0;
    }
    
    public void ChangeRoom()
    { 
        SceneManager.LoadSceneAsync(roomOrder[numberOfRoom]);
        numberOfRoom++;
    }
}
