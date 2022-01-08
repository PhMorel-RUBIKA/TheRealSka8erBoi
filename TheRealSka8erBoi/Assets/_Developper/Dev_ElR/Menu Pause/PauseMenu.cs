using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{

    public static bool gameIsPaussed = false;

    public GameObject pauseMenuUI;

    public MMFeedbacks timeFeedbacks;
    public MMFeedbacks noTimeFeedbacks;
    
    public GameObject pauseFirstButton, optionsFirstButton, quitFirstButton;




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
   void Update()
    {
        
         
        
        if (Input.GetKeyDown(KeyCode.JoystickButton7))
        {
            if (gameIsPaussed)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

   public void FixedUpdate()
   {
       //clear selected object
       EventSystem.current.SetSelectedGameObject(null);
       //Set new selected object
       EventSystem.current.SetSelectedGameObject(pauseFirstButton);
   }


   public void Resume()
    {
        pauseMenuUI.SetActive(false);
        timeFeedbacks.PlayFeedbacks();
        //Time.timeScale = 1f;
        gameIsPaussed = false;

        Debug.Log("resume...");
        
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        timeFeedbacks.StopFeedbacks();
        noTimeFeedbacks.PlayFeedbacks();
        //Time.timeScale = 0f;
        gameIsPaussed = true;
        Debug.Log("pause...");
        
    }

    public void LoadMenu()
    {
        timeFeedbacks.StopFeedbacks();
        noTimeFeedbacks.PlayFeedbacks();
        SceneManager.LoadScene("MainMenu");
    }

    
    
}
