using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEditor;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject score_text;
    public GameObject FinalScore;
    public GameObject gameOverMenu;
    public Board board;
    public HighScore highScore;
    public GameObject HighScore;


    public UIManager(Board board, HighScore highScore)
    {
        this.board = board;
        this.highScore = highScore;

    }

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
        if (highScore.CheckIfHighscore(board.FinalScore))
        {
            HighScore.SetActive(true);
        }
        else
        {
            HighScore.SetActive(false);

        }
     
        FinalScore.SetActive(true);
        score_text.SetActive(false);
    }

}