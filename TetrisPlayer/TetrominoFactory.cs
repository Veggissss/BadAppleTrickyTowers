using System.Collections.Generic;
using UnityEngine;

namespace BadAppleTrickyTowersMod.TetrisPlayer
{
    public static class TetrominoFactory
    {
        public static Tetromino Create(string resourceId)
        {
            switch (resourceId)
            {
                case "BRICK_T":
                    return new Tetromino(resourceId, new List<Vector2>
                    {
                        new Vector2(0, 0), new Vector2(-1, 0), new Vector2(1, 0), new Vector2(0, -1)
                    }, new Vector2(0, 0)
                );
                case "BRICK_L":
                    return new Tetromino(resourceId, new List<Vector2>
                    {
                        new Vector2(0, 0), new Vector2(-1, 0), new Vector2(1, 0), new Vector2(-1, -1)
                    }, new Vector2(0, 0)
                );
                case "BRICK_J":
                    return new Tetromino(resourceId, new List<Vector2>
                    {
                        new Vector2(0, 0), new Vector2(-1, 0), new Vector2(1, 0), new Vector2(1, -1)
                    }, new Vector2(0, 0)
                );
                case "BRICK_O":
                    return new Tetromino(resourceId, new List<Vector2>
                    {
                        new Vector2(0, 0), new Vector2(1, 0), new Vector2(0, 1), new Vector2(1, 1)
                    }, new Vector2(0.5f, 1f)
                );
                case "BRICK_I":
                    return new Tetromino(resourceId, new List<Vector2>
                    {
                        new Vector2(-1, 0), new Vector2(0, 0), new Vector2(1, 0), new Vector2(2, 0)
                    }, new Vector2(0.5f, 0.5f)
                );
                case "BRICK_S":
                    return new Tetromino(resourceId, new List<Vector2>
                    {
                        new Vector2(0, 0), new Vector2(1, 0), new Vector2(0, -1), new Vector2(-1, -1)
                    }, new Vector2(0,0)
                );
                case "BRICK_Z":
                    return new Tetromino(resourceId, new List<Vector2>
                    {
                        new Vector2(0, 0), new Vector2(-1, 0), new Vector2(0, -1), new Vector2(1, -1)
                    }, new Vector2(0, 0)
                );
                default:
                    Debug.LogError("Invalid Tetromino ID: " + resourceId);
                    return null;
            }
        }
    }
}
