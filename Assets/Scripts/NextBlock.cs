using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.Scripts
{
    public class NextBlock : MonoBehaviour
    {

        public Tilemap tilemap { get; set; }
        public Vector3Int spawnPosition;
        public Vector2Int boardSize;
        public Tetromino[] tetrominos;
        public TetrominoData data { get; private set; }
        public Vector3Int Position { get; private set; }
        public Vector3Int[] cells { get; private set; }

       

        public void Initialize(Vector3Int position, TetrominoData data)
        {
            
            this.Position = position;
            this.data = data;
          
            if (this.cells == null)
            {
                this.cells = new Vector3Int[data.cells.Length];
            }

            for (int i = 0; i < data.cells.Length; i++)
            {
                this.cells[i] = (Vector3Int)data.cells[i];
            }
        }
    }
}
