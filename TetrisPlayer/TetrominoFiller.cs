using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BadAppleTrickyTowersMod.TetrisPlayer
{
    public class TetrominoFiller
    {
        private TetrominoGrid grid;
        private bool[,] frame = null;

        public List<PlacedTetromino> PlacedBricks { get; private set; } = new List<PlacedTetromino>();
     
        public void SetFrame(bool[,] inputFrame)
        {
            int width = inputFrame.GetLength(0);
            int height = inputFrame.GetLength(1);
            grid = new TetrominoGrid(width, height);

            frame = inputFrame;
        }

        public PlacedTetromino FillInitialFrame(Tetromino brick)
        {
            if (frame == null)
            {
                throw new System.Exception("Initial frame is not set.");
            }
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
                            Vector2 pos = anchorPos - new Vector2(anchorCell.x, anchorCell.y);

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
            // Could not place brick
            return null; 
        }

        private bool CanPlaceOnFrame(List<Vector2> shape, Vector2 offset)
        {
            foreach (var cell in shape)
            {
                Vector2 p = offset + new Vector2(Mathf.RoundToInt(cell.x), Mathf.RoundToInt(cell.y));
                if (!grid.IsInside(p) || !grid.IsCellEmpty(p))
                {
                    return false;
                }
            }
            return true;
        }

        public void ApplyFrame(bool[,] frame)
        {
            HashSet<Vector2> targetPixels = new HashSet<Vector2>();
            for (int x = 0; x < frame.GetLength(0); x++)
            {
                for (int y = 0; y < frame.GetLength(1); y++)
                {
                    if (frame[x, y])
                    {
                        targetPixels.Add(new Vector2(x, y));
                    }
                }
            }

            foreach (var brick in PlacedBricks)
            {
                bool shouldBeVisible = brick.CoveredCells().Any(c => targetPixels.Contains(c));
                brick.Visible = shouldBeVisible;
            }
        }
    }
}
