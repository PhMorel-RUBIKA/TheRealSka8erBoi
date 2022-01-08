using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{

    public static bool gameIsPaussed = false;

    public GameObject pauseMenuUI;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetAxisRaw("Vertical") > 0.5f && !changed && actualButton > 0 && mainMenu.activeInHierarchy)
        {
            actualButton -= 1;
            changed = true;
        }
        else if (Input.GetAxisRaw("Vertical") < -0.5f && !changed && actualButton < 2 && mainMenu.activeInHierarchy)
        {
            actualButton += 1;
            changed = true;
        }
        else if (Input.GetAxisRaw("Vertical") < 0.5f && Input.GetAxisRaw("Vertical") > -0.5f)
        {
            changed = false;
        }

        if (Input.GetButtonDown("Cancel"))
        {
            mainMenu.SetActive(true);
            optionMenu.SetActive(false);
        }*/
        
        if (Input.GetButtonDown("Fire1"))
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

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaussed = false;
        
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaussed = true;
    }

    public void LoadMenu()
    {
        Debug.Log("Loading...");
    }

    public void QuitGame()
    {
        Debug.Log("Quit...");
    }
}
