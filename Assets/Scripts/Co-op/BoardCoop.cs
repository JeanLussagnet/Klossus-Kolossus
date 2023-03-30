
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.Tilemaps;

public class BoardCoop : MonoBehaviour
{
    public Tilemap tilemap { get; private set; }
    public TetrominoData[] tetrominos;
    public Block activePiece1;
    public Block activePiece2;
    public Vector3Int spawnPosition1;
    public Vector3Int spawnPosition2;
    public Vector2Int boardSize = new Vector2Int(20, 20);
    public int clearedLines { get; private set; }
    


    public RectInt Bounds
    {
       get {Vector2Int position = new Vector2Int(-this.boardSize.x/2, -this.boardSize.y/2);
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
        SpawnPiece(true);
        SpawnPiece(false);
    }


    public void SpawnPiece(bool isPlayer1 )
    {
        var activePiece = isPlayer1 ? activePiece1 : activePiece2;

        var spawnPosition = isPlayer1 ? spawnPosition1 : spawnPosition2;
        int random = Random.Range(0, this.tetrominos.Length);
        TetrominoData data = this.tetrominos[random];
      
        activePiece.Initialize(this, spawnPosition, data);

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
        this.tilemap.ClearAllTiles();
    }

    public void Set(Block piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.Position;
            this.tilemap.SetTile(tilePosition, piece.data.tile);
        }
       
    }

    public void Clear(Block piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.Position;
            this.tilemap.SetTile(tilePosition, null);
        }
    }

    public bool IsValidPosition(Block piece, Vector3Int position)
    {
        RectInt bounds = this.Bounds;
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + position;

            if(!bounds.Contains((Vector2Int)tilePosition)) 
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


    }

    private void LineClear(int row)
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
                if (activePiecePositions.Contains(currentPosition) ||activePiecePositions.Contains(abovePosition))
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
