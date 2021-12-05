using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManagerPROTO : MonoBehaviour
{
    public List<string> roomOrder;
    public int numberOfRoom;
    public Animator transition;
    public float transitionTime;
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
    
    public IEnumerator ChangeRoom()
    { 
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadSceneAsync(roomOrder[numberOfRoom]);
        transition.SetTrigger("Stop");
        
        numberOfRoom++;
        
    }
}
