using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManagerBehaviour : MonoBehaviour
{
    public Button button1;
    public Button button2;
    public Button button3;
    public List<Button> buttonList;
    public int actualButton = 0;
    public bool changed = false;

    public GameObject mainMenu;
    public GameObject optionMenu;


    public void Start()
    {
        buttonList.Add(button1);
        buttonList.Add(button2);
        buttonList.Add(button3);
    }

    public void Update()
    {
        if (Input.GetAxisRaw("Vertical") > 0.5f && !changed && actualButton > 0 && mainMenu.activeInHierarchy)
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
        }
    }

    public void FixedUpdate()
    {
        buttonList[actualButton].Select();
    }

    public void StartPlaying()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
