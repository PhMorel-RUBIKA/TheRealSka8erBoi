using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugController : MonoBehaviour
{
    private bool showConsole;
    private string input;

    public static DebugCommand KILL_ALL;
    public static DebugCommand MOULAGA;
    public static DebugCommand OVER9000;
    public static DebugCommand SUPERMAN;
    
    public List<object> commandList;

    private void Awake()
    {
        KILL_ALL = new DebugCommand("kill_all", "Removes all enemies from the scene.", "kill_all", () =>
        {
            WaveManager.instance.KillEnemies();
        });

        MOULAGA = new DebugCommand("moulaga", "Adds 1000 coins.", "moulaga", () =>
        {
            BonusManager.instance.GainCoins(1000);
        });

        OVER9000 = new DebugCommand("over9000", "Multiply Base damage by 1000.", "over9000", () =>
        {
            PlayerBehaviour.playerBehaviour.over9000Power = !PlayerBehaviour.playerBehaviour.over9000Power;
        });
        
        SUPERMAN = new DebugCommand("superman", "Prevent Damage.", "superman", () =>
        {
            PlayerBehaviour.playerBehaviour.canTakeDamage = !PlayerBehaviour.playerBehaviour.canTakeDamage;
        });

        commandList = new List<object>
        {
            KILL_ALL,
            MOULAGA,
            OVER9000,
            SUPERMAN,
        };
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) showConsole = !showConsole;
        if (!Input.GetKeyDown(KeyCode.Return)) return;
        if (!showConsole) return;
        HandleInput();
        input = "";
    }

    private void OnGUI()
    {
        if (!showConsole) return;
        
        float y  = 0f;
        GUI.Box(new Rect(0, y, Screen.width, 30), "");
        GUI.backgroundColor = new Color(0, 0, 0, 0);
        
        input = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20f, 20f), input);
    }

    private void HandleInput()
    {
        for (int i = 0; i < commandList.Count; i++)
        {
            DebugCommandBase commandBase = commandList[i] as DebugCommandBase;
            if (input.Contains(commandBase.commandId))
            {
                if (commandList[i] as DebugCommand != null)
                {
                    (commandList[i] as DebugCommand).Invoke();
                }
            }
        }
    }
}