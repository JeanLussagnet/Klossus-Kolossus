using System;
using System.Threading;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public Board Board { get; private set; }
    public TetrominoData data { get; private set; }
    public Vector3Int Position { get; private set; }
    public Vector3Int[] cells { get; private set; }
    public int RotationIndex { get; private set; }
    private float stepDelay = 1.05f;
    public float lockDelay = 0.1f;
    public bool isNextBlock;
    public bool isHeld;
    public bool isHeldThisTurn { get; set; }
    private float stepTime;
    private float lockTime;
    public void Initialize(Board board, Vector3Int position, TetrominoData data)
    {
        this.Board = board;
        this.Position = position;
        this.data = data;
        this.stepTime = Time.time + this.stepDelay;
        this.lockTime = 0f;
        if (this.cells == null)
        {
            this.cells = new Vector3Int[data.cells.Length];
        }

        for (int i = 0; i < data.cells.Length; i++)
        {
            this.cells[i] = (Vector3Int)data.cells[i];
        }
    }

    public void Update()
    {
        if (PauseMenu.isPaused)
        {
            return;
        }
        else
        {
            if(!isNextBlock && !isHeld)
            {


            this.Board.Clear(this);


            this.lockTime += Time.deltaTime;


            if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (this.Position.x > 0)
                {
                    Rotate(1);
                }
                else
                {
                    Rotate(-1);
                }
            }
            else if (Input.GetKeyDown(KeyCode.E))
                Rotate(1);

            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                Thread.Sleep(100);
                Move(Vector2Int.left);
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                Thread.Sleep(100);
                Move(Vector2Int.right);
            }

            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                Thread.Sleep(100);
                Move(Vector2Int.down);
            }

            if (Input.GetKeyDown(KeyCode.C) && !isHeldThisTurn)
                {
                    Board.HoldPiece(this.data);
                    isHeldThisTurn = !isHeldThisTurn;
                }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                HardDrop();
            }
            if (Time.time >= this.stepTime)
            {
                Step();
            }
            this.Board.Set(this);
            }
            else
            {

                return;
               
            }
        }



    }

    private void Step()
    {
        this.stepTime = Time.time + this.stepDelay - ((float)Board.level*0.05f);

        Move(Vector2Int.down);

        if (this.lockTime >= this.lockDelay)
        {
            Lock();
        }
    }

    private void Lock()
    {
        this.Board.Set(this);
        this.Board.ClearLines();
        isHeldThisTurn = false;
        this.Board.SpawnPiece();
    }

    private void HardDrop()
    {
        while (Move(Vector2Int.down))
            continue;
    }

    private bool Move(Vector2Int translation)
    {
        Vector3Int newPosition = this.Position;
        newPosition.x += translation.x;
        newPosition.y += translation.y;
        bool valid = this.Board.IsValidPosition(this, newPosition);

        if (valid)
        {
            this.Position = newPosition;
            this.lockTime = 0f;
        }

        return valid;
    }

    private void Rotate(int direction)
    {
        int originalRotationIndex = this.RotationIndex;
        this.RotationIndex += Wrap(this.RotationIndex + direction, 0, 4);

        ApplyRotationMatrix(direction);

        if (!TestWallKicks(this.RotationIndex, direction))
        {
            this.RotationIndex = originalRotationIndex;
            ApplyRotationMatrix(-direction);
        }
    }

    private void ApplyRotationMatrix(int direction)
    {
        for (int i = 0; i < this.cells.Length; i++)
        {
            int x = 0, y = 0;
            Vector3 cell = this.cells[i];

            switch (this.data.tetromino)
            {
                case Tetromino.I:
                case Tetromino.O:
                    cell.x -= 0.5f;
                    cell.y -= 0.5f;
                    x = Mathf.CeilToInt((cell.x * Data.RotationMatrix[0] * direction) + (cell.y * Data.RotationMatrix[1] * direction));
                    y = Mathf.CeilToInt((cell.x * Data.RotationMatrix[2] * direction) + (cell.y * Data.RotationMatrix[3] * direction));
                    break;


                default:
                    x = Mathf.RoundToInt((cell.x * Data.RotationMatrix[0] * direction) + (cell.y * Data.RotationMatrix[1] * direction));
                    y = Mathf.RoundToInt((cell.x * Data.RotationMatrix[2] * direction) + (cell.y * Data.RotationMatrix[3] * direction));
                    break;
            }

            this.cells[i] = new Vector3Int(x, y, 0);
        }
    }

    private bool TestWallKicks(int rotationIndex, int direction)
    {
        int wallKickIndex = GetWallKickIndex(rotationIndex, direction);

        for (int i = 0; i < this.data.wallKicks.GetLength(1); i++)
        {
            Vector2Int translation = this.data.wallKicks[wallKickIndex, i];

            if (Move(translation))
            {
                return true;
            }

        }

        return false;
    }

    private int GetWallKickIndex(int rotationIndex, int direction)
    {
        int wallKickIndex = rotationIndex * 2;

        if (direction < 0)
        {
            wallKickIndex--;
        }
        else if (direction > 1)
        {
            wallKickIndex++;
        }
        return Wrap(wallKickIndex, 0, this.data.wallKicks.GetLength(0));
    }



    private int Wrap(int input, int min, int max)
    {
        if (input < min)
        {
            return max - (min - input) % (max - min);
        }
        else
        {
            return min + (input - min) % (max - min);
        }
    }


}

