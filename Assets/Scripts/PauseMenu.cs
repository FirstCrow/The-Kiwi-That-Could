using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

// This script controlls the pause menu

public class PauseMenu : MonoBehaviour
{
    [Header("Links")]
    public GameObject pauseMenuUI;
    public GameObject settingMenuUI;

    [Tooltip("Lets other scripts know if the game is paused")]
    private static bool GameIsPaused = false;

    private void Update()
    {
        // Lets the player pause and unpause the game using escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
                Resume();
            else
                Pause();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            Time.timeScale = 100f;
        }


        if (Input.GetKeyUp(KeyCode.F))
        {
            Time.timeScale = 1f;
        }
    }

    // Resumes the game
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        settingMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    // Pauses the game
    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    // Quits to the Main Menu
    public void QuitGame()
    {
        Resume();
        SceneManager.LoadScene(0);
    }

    // Returns true if game is paused and false otherwise
    public static bool getGameIsPaused()
    {
        return GameIsPaused;
    }
}
