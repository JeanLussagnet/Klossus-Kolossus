using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused;
    public GameObject PauseMenuUI;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            PauseGame();
         
        }
    }

    void PauseGame()
    {
        if (isPaused)
        {
            PauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            PauseMenuUI.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void Resume()
    {
        
      isPaused = !isPaused;
        PauseGame();
    }

    void Pause()
    {
        
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game");
    }


}
