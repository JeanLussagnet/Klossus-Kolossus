using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject score_text;
    public GameObject FinalScore;
    public GameObject gameOverMenu;

    private void OnEnable()
    {
        Board.GameOverEvent += EnableGameOverMenu;
    }

    private void OnDisable()
    {
        Board.GameOverEvent -= EnableGameOverMenu;
    }

    public void EnableGameOverMenu()
    {
        gameOverMenu.SetActive(true);
        FinalScore.SetActive(true);
        score_text.SetActive(false);
    }

}