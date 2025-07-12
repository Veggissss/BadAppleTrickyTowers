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

        public PlacedTetromino(Tetromino tetromino, Vector2 position, List<Vector2> shape)
        {
            Tetromino = tetromino;
            Position = position;
            Shape = shape;
        }
    }
}
