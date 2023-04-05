using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.Tilemaps;


public class Board : MonoBehaviour
{
    public static event Action GameOverEvent;
    public Tilemap tilemap { get; private set; }
    public TetrominoData[] tetrominos;
    public Piece activePiece1;
    public Piece NextBlock1;
    public Piece HeldPiece1;
    public Piece activePiece2;
    public Piece NextBlock2;
    public Piece HeldPiece2;
    
   


    public bool isCoOp;
    public Vector3Int spawnPosition1;/* = isCoOp ? new Vector3Int(-4,8,0) : new Vector3Int(-1, 8, 0);*/
    public Vector3Int spawnPosition2;/* = new Vector3Int(6, 8, 0);*/
    public Vector2Int boardSize;/* = isCoOp ? new Vector2Int(20, 20) : new Vector2Int(10, 20);*/

    public Vector3Int holdPosition1;
    public Vector3Int holdPosition2;


    public Vector3Int nextBlockPosition2;
    public Vector3Int nextBlockPosition1;
    public Vector2Int nextBlockSize = new Vector2Int(4, 4);


    public int scoreOneLine = 40;

    public int scoreTwoLines = 100;
    public int scoreThreeLines = 300;
    public int scoreFourLines = 1200;

    private int clearedLines = 0;
    public TMP_Text hud_score;
    public TMP_Text hud_finalScore;
    public TMP_Text hud_countDown;
    public GameObject countDownUI;
    private int numberOfClearedLines = 0;
    public int level = 1;
    public int FinalScore { get; set; }



    private int currentScore = 0;

    public void UpdateScore()
    {
        if (clearedLines > 0)
        {
            numberOfClearedLines += clearedLines;

            if (clearedLines == 1)
            {
                currentScore += scoreOneLine * level;
            }
            else if (clearedLines == 2)
            {
                currentScore += scoreTwoLines * level;
            }
            else if (clearedLines == 3)
            {
                currentScore += scoreThreeLines * level;
            }
            else if (clearedLines == 4)
            {
                currentScore += scoreFourLines * level;
            }
            clearedLines = 0;
            UpdateLevel();
            UpdateUI();
        }

    }

    private void UpdateLevel()
    {
        if (numberOfClearedLines / 10 > 0 && numberOfClearedLines < 200)
            level = (numberOfClearedLines / 10) + 1;
        else if (numberOfClearedLines / 10 >= 20)
            level = 20;

        else
            level = 1;



    }

    public void UpdateUI()
    {
        hud_score.text = $"Score: \n {currentScore}";
    }



    IEnumerator Countdown(int seconds)
    {
        PauseMenu.isPaused = true;
        countDownUI.SetActive(true);

        int count = seconds;

        while (count > 0)
        {

            if (count == 1)
            {
                hud_countDown.text = "Go";
            }
            else
            {
                hud_countDown.text = (count - 1).ToString();
            }
            yield return new WaitForSeconds(1);
            count--;
        }
        countDownUI.SetActive(false);
        PauseMenu.isPaused = false;
      

    }

 

    public void HoldPiece(TetrominoData data)
    {

        if (HeldPiece1.Position == holdPosition1)
        {
            Clear(activePiece1);
            this.activePiece1.Initialize(this, spawnPosition1, HeldPiece1.data);

            Clear(HeldPiece1);
            this.HeldPiece1.Initialize(this, holdPosition1, data);
            Set(HeldPiece1);


            if (IsValidPosition(this.activePiece1, spawnPosition1))
            {
                Set(this.activePiece1);
            }
            else
            {
                GameOver();
            }
        }
        else
        {
            this.HeldPiece1.Initialize(this, holdPosition1, activePiece1.data);
            Set(HeldPiece1);
            Clear(activePiece1);
            SpawnPiece();
        }

    }

    public void HoldPiece(TetrominoData data, bool isPlayer1)
    {


        if (isPlayer1)
        {
            if (HeldPiece1.Position == holdPosition1)
            {
                Clear(activePiece1);
                this.activePiece1.Initialize(this, spawnPosition1, HeldPiece1.data);

                Clear(HeldPiece1);
                this.HeldPiece1.Initialize(this, holdPosition1, data);
                Set(HeldPiece1);


                if (IsValidPosition(this.activePiece1, spawnPosition1))
                {
                    Set(this.activePiece1);
                }
                else
                {
                    GameOver();
                }
            }
            else
            {
                this.HeldPiece1.Initialize(this, holdPosition1, activePiece1.data);
                Set(HeldPiece1);
                Clear(activePiece1);
                SpawnPiece(true);
            }
        }
        else
        {
            if (HeldPiece2.Position == holdPosition2)
            {
                Clear(activePiece2);
                this.activePiece2.Initialize(this, spawnPosition2, HeldPiece2.data);

                Clear(HeldPiece2);
                this.HeldPiece2.Initialize(this, holdPosition2, data);
                Set(HeldPiece2);


                if (IsValidPosition(this.activePiece2, spawnPosition2))
                {
                    Set(this.activePiece2);
                }
                else
                {
                    GameOver();
                }
            }
            else
            {
                this.HeldPiece2.Initialize(this, holdPosition2, activePiece2.data);
                Set(HeldPiece2);
                Clear(activePiece2);
                SpawnPiece(false);
            }
        }



        //    var activePiece = isPlayer1 ? activePiece1 : activePiece2;
        //    var spawnPosition = isPlayer1 ? spawnPosition1 : spawnPosition2;
        //    var heldPiece = isPlayer1 ? HeldPiece1 : HeldPiece2;
        //var heldPosition = isPlayer1 ? holdPosition1 : holdPosition2;


        //if (heldPiece.Position == heldPosition)
        //{
        //    Clear(activePiece);
        //    activePiece.Initialize(this, spawnPosition, heldPiece.data);

        //    Clear(heldPiece);
        //    heldPiece.Initialize(this, heldPosition, data);
        //    Set(heldPiece);


        //    if (IsValidPosition(activePiece, spawnPosition))
        //    {
        //        Set(activePiece);
        //    }
        //    else
        //    {
        //        GameOver();
        //    }
        //}
        //else
        //{
        //    heldPiece.Initialize(this, heldPosition, data);
        //    Set(heldPiece);
        //    Clear(activePiece);
        //    SpawnPiece();
        //}

    }



    public void QueuePiece()
    {
        int random = UnityEngine.Random.Range(0, this.tetrominos.Length);
        TetrominoData data = this.tetrominos[random];

        this.NextBlock1.Initialize(this, nextBlockPosition1, data);
        Set(NextBlock1);


    }

    public void QueuePiece(bool isPlayer1)
    {
        var nextBlock = isPlayer1 ? NextBlock1 : NextBlock2;

        var nextBlockPosition = isPlayer1 ? nextBlockPosition1 : nextBlockPosition2;
        int random1 = UnityEngine.Random.Range(0, this.tetrominos.Length);
        TetrominoData data = this.tetrominos[random1];
        nextBlock.Initialize(this, nextBlockPosition, data);
        Set(nextBlock);


    }
    public RectInt Bounds
    {
        get
        {
            Vector2Int position = new Vector2Int(-this.boardSize.x / 2, -this.boardSize.y / 2);
            return new RectInt(position, this.boardSize);
        }
    }

    private void Awake()
    {
    
        this.tilemap = GetComponentInChildren<Tilemap>();
        for (int i = 0; i < this.tetrominos.Length; i++)
        {
            this.tetrominos[i].Initialize();
        }
    }

    private void Start()
    {
        StartCoroutine(Countdown(4));
        if (isCoOp)
        {
            QueuePiece(true);
            QueuePiece(false);
            SpawnPiece(true);
            SpawnPiece(false);
        }
        else
        {
            QueuePiece();
            SpawnPiece();
        }
    }


    public void SpawnPiece()
    {

        this.activePiece1.Initialize(this, spawnPosition1, NextBlock1.data);
        Clear(NextBlock1);
        QueuePiece();

        if (IsValidPosition(this.activePiece1, spawnPosition1))
        {

            Set(this.activePiece1);

        }
        else
        {

            GameOver();
        }
    }

    public void SpawnPiece(bool isPlayer1)
    {
        var activePiece = isPlayer1 ? activePiece1 : activePiece2;
        var nextBlock = isPlayer1 ? NextBlock1 : NextBlock2;
        var spawnPosition = isPlayer1 ? spawnPosition1 : spawnPosition2;

        activePiece.Initialize(this, spawnPosition, nextBlock.data);
        Clear(nextBlock);
        QueuePiece(isPlayer1);
        if (IsValidPosition(activePiece, spawnPosition))
        {
            Set(activePiece);
        }
        else
        {
            GameOver();
        }
    }


    private void GameOver()
    {
        FinalScore = currentScore;
        Time.timeScale = 0f;
        hud_finalScore.text = currentScore.ToString();

        
      
        GameOverEvent?.Invoke();

    }

    public void PlayAgain()
    {

        level = 1;
        currentScore = 0;
        if (isCoOp)
        {
            Clear(HeldPiece2);
            Clear(HeldPiece1);
            Clear(NextBlock2);
            Clear(NextBlock1);
            Clear(activePiece1);
            Clear(activePiece2);
        }
        else
        {
            Clear(activePiece1);
            Clear(NextBlock1);
            Clear(HeldPiece1);
        }

        this.tilemap.ClearAllTiles();
        Time.timeScale = 1f;
        Start();
    }

    public void Exit()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void Set(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.Position;
            this.tilemap.SetTile(tilePosition, piece.data.tile);
        }

    }

    public void Clear(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.Position;
            this.tilemap.SetTile(tilePosition, null);
        }
    }

    public bool IsValidPosition(Piece piece, Vector3Int position)
    {
        RectInt bounds = this.Bounds;
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + position;

            if (!bounds.Contains((Vector2Int)tilePosition))
                return false;

            if (this.tilemap.HasTile(tilePosition))
                return false;


        }
        return true;
    }


    public void ClearLines()
    {
        RectInt bounds = this.Bounds;
        int row = bounds.yMin;

        while (row < bounds.yMax)
        {
            if (IsLineFull(row))
            {
                clearedLines++;
                LineClear(row);
            }
            else
                row++;
        }
        UpdateScore();

    }

    private void LineClear(int row)
    {
        if (isCoOp)
        {
            var activePiece = activePiece1.Position.y > activePiece2.Position.y ? activePiece1 : activePiece2;
            Vector3Int[] activePiecePositions = new Vector3Int[activePiece.cells.Length];
            for (int i = 0; i < activePiece.cells.Length; i++)
            {
                activePiecePositions[i] = activePiece.cells[i] + activePiece.Position;
            }

            RectInt bounds = this.Bounds;
            for (int col = bounds.xMin; col < bounds.xMax; col++)
            {
                Vector3Int position = new Vector3Int(col, row, 0);
                this.tilemap.SetTile(position, null);
            }

            while (row < bounds.yMax)
            {
                for (int col = bounds.xMin; col < bounds.xMax; col++)
                {
                    Vector3Int abovePosition = new Vector3Int(col, row + 1, 0);
                    TileBase above = this.tilemap.GetTile(abovePosition);
                    Vector3Int currentPosition = new Vector3Int(col, row, 0);
                    TileBase current = this.tilemap.GetTile(currentPosition);
                    if (activePiecePositions.Contains(currentPosition) || activePiecePositions.Contains(abovePosition))
                    {
                        this.tilemap.SetTile(currentPosition, null);

                    }
                    else
                    {
                        this.tilemap.SetTile(currentPosition, above);
                    }


                }
                row++;
            }
        }
        else
        {
            RectInt bounds = this.Bounds;
            for (int col = bounds.xMin; col < bounds.xMax; col++)
            {
                Vector3Int position = new Vector3Int(col, row, 0);
                this.tilemap.SetTile(position, null);
            }
            while (row < bounds.yMax)
            {
                for (int col = bounds.xMin; col < bounds.xMax; col++)
                {
                    Vector3Int position = new Vector3Int(col, row + 1, 0);
                    TileBase above = this.tilemap.GetTile(position);
                    position = new Vector3Int(col, row, 0);
                    this.tilemap.SetTile(position, above);
                }
                row++;
            }
        }
    }

    private bool IsLineFull(int row)
    {
        RectInt bounds = this.Bounds;

        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);

            if (!this.tilemap.HasTile(position))
            {
                return false;
            }
        }
        return true;
    }


}
