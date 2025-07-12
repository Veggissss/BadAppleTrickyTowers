using System.Collections.Generic;
using UnityEngine;

namespace BadAppleTrickyTowersMod.TetrisPlayer
{
    public class TetrominoFiller
    {
        private TetrominoGrid grid;
        private bool[,] frame;

        public List<PlacedTetromino> PlacedBricks { get; private set; } = new List<PlacedTetromino>();

        public TetrominoFiller(bool[,] inputFrame)
        {
            int width = inputFrame.GetLength(0);
            int height = inputFrame.GetLength(1);

            frame = inputFrame;
            grid = new TetrominoGrid(width, height);
        }

        public PlacedTetromino TryPlaceBrick(Tetromino brick)
        {
            int width = frame.GetLength(0);
            int height = frame.GetLength(1);

            int i = 0;
            foreach (var shape in brick.GetRotations())
            {
                Quaternion rotationQuat = Quaternion.Euler(0, 0, -90f * i);
                i++;
                foreach (var anchorCell in shape)
                {
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            Vector2 anchorPos = new Vector2(x, y);
                            Vector2 pos = anchorPos - new Vector2(Mathf.RoundToInt(anchorCell.x), Mathf.RoundToInt(anchorCell.y));

                            if (CanPlaceOnFrame(shape, pos))
                            {
                                Tetromino placed = new Tetromino(brick.ResourceId, shape, brick.Offset);
                                grid.PlaceBrick(placed, pos);

                                var placedInfo = new PlacedTetromino(placed, pos, shape, rotationQuat);
                                PlacedBricks.Add(placedInfo);

                                return placedInfo;
                            }
                        }
                    }
                }
            }

            return null; // Could not place
        }

        private bool CanPlaceOnFrame(List<Vector2> shape, Vector2 offset)
        {
            foreach (var cell in shape)
            {
                Vector2 p = offset + new Vector2(Mathf.RoundToInt(cell.x), Mathf.RoundToInt(cell.y));
                if (!grid.IsInside(p) || !grid.IsCellEmpty(p) || frame[(int)p.x, (int)p.y] == false)
                    return false;
            }
            return true;
        }

        public TetrominoGrid GetFilledGrid() => grid;

        public TetrominoGrid GetFrameGrid() => new TetrominoGrid(grid.Width, grid.Height, frame);
    }
}
