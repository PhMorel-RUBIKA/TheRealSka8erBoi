using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{

    public static bool gameIsPaussed = false;

    public GameObject pauseMenuUI;
    public GameObject deathMenu;

    public MMFeedbacks timeFeedbacks;
    public MMFeedbacks noTimeFeedbacks;
    
    public GameObject pauseFirstButton, optionsFirstButton, optionsQuitButton;

    public GameObject optionsMenuUI;

    public AudioMixer audioMixer;

    public GameObject[] managers;
    


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
   void Update()
    {
        if (Input.GetAxisRaw("Dash") > 0)
        {
            if (deathMenu.activeSelf)
            {
                foreach (GameObject manager in managers )
                {
                    Destroy(manager);
                }
                SceneManager.LoadScene("MainMenu");
            }
        }
         
        
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

        /*if (Input.GetKeyDown(KeyCode.JoystickButton3))
        {
            optionsMenuUI.SetActive(false);
        }*/
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

    public void GameOverPause()
    {
        timeFeedbacks.StopFeedbacks();
        noTimeFeedbacks.PlayFeedbacks();
        //Time.timeScale = 0f;
        gameIsPaussed = true;
        Debug.Log("Game Over");
    }

    public void LoadMenu()
    {
        timeFeedbacks.StopFeedbacks();
        noTimeFeedbacks.PlayFeedbacks();
        Time.timeScale = 1;
        foreach (GameObject manager in managers )
        {
            Destroy(manager);
        }
        SceneManager.LoadScene("MainMenu");
    }

    public void OptionsMenu()
    {
        optionsMenuUI.SetActive(true);
        Debug.Log("OPTIONS");
        
        //clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        //Set new selected object
        EventSystem.current.SetSelectedGameObject(optionsFirstButton);
    }

    public void CloseOptionsMenu()
    {
        optionsMenuUI.SetActive(false);

        //clear selected object
        EventSystem.current.SetSelectedGameObject(null);
        //Set new selected object
        EventSystem.current.SetSelectedGameObject(optionsQuitButton);
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("MasterVolume", volume);
    }
    
    public void SetVolumeEffects(float volume)
    {
        audioMixer.SetFloat("SfxVolume", volume);
    }



}
