using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Home()
    {
        SceneManager.LoadScene(0);
    }
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }
    public void CoOp()
    {
        SceneManager.LoadScene(2);
    }
    public void Highscore()
    {
        SceneManager.LoadScene(3);
    }
    public void QuitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }

}
