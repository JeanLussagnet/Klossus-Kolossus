
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.Tilemaps;



public class Board : MonoBehaviour
{
    public Tilemap tilemap { get; private set; }
    public TetrominoData[] tetrominos;
    public Piece activePiece1 { get; private set; }

    public Vector3Int spawnPosition1 = new Vector3Int(-1, 8, 0);
  
    public Vector2Int boardSize = new Vector2Int(10, 20);

    public int scoreOneLine = 40;

    public int scoreTwoLines = 100;
    public int scoreThreeLines = 300;
    public int scoreFourLines = 1200;

    private int clearedLines = 0;
    public TMP_Text hud_score;

    private int currentScore = 0;
    public void UpdateScore()
    {
        if (clearedLines > 0)
        {
            if(clearedLines == 1) {
                currentScore += scoreOneLine;
            }
            else if(clearedLines ==2) {
                currentScore += scoreTwoLines;
            }
            else if(clearedLines ==3) {
                currentScore += scoreThreeLines;
            }
            else if(clearedLines ==4) { 
                currentScore += scoreFourLines;
            }
            clearedLines = 0;
            UpdateUI();
        }
        
    }

    public void UpdateUI()
    {
        hud_score.text = currentScore.ToString();
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
        
        this.activePiece1 = GetComponentInChildren<Piece>();
        this.tilemap = GetComponentInChildren<Tilemap>();
        for (int i = 0; i < this.tetrominos.Length; i++)
        {
            this.tetrominos[i].Intialize();
        }
    }

    private void Start()
    {
     
        SpawnPiece();

    }


    public void SpawnPiece()
    {

        int random = Random.Range(0, this.tetrominos.Length);
        TetrominoData data = this.tetrominos[random];

        this.activePiece1.Initialize(this, spawnPosition1, data);

        if (IsValidPosition(this.activePiece1, spawnPosition1))
        {

            Set(this.activePiece1);

        }
        else
        {

            GameOver();
        }
    }


    private void GameOver()
    {
        this.tilemap.ClearAllTiles();
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
