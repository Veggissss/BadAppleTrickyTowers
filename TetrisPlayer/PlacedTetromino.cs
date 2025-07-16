using System.Collections.Generic;
using UnityEngine;

namespace BadAppleTrickyTowersMod.TetrisPlayer
{
    public class PlacedTetromino
    {
        public Tetromino Tetromino;
        public Vector2 Position;
        public Vector2 ShapeOffset;
        public List<Vector2> Shape;
        public Quaternion Rotation;
        public Brick brickInstance;
        public bool Visible = true;

        public PlacedTetromino(Tetromino tetromino, Vector2 position, List<Vector2> shape, Quaternion rotation)
        {
            Tetromino = tetromino;
            Position = position;
            Shape = shape;
            Rotation = rotation;
        }

        public IEnumerable<Vector2> CoveredCells()
        {
            foreach (var cell in Shape)
                yield return new Vector2(cell.x + Position.x, cell.y + Position.y);
        }
    }
}
