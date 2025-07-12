
namespace BadAppleTrickyTowersMod.TetrisPlayer
{
    using System.Collections.Generic;
    using UnityEngine;

    public class TetrominoGrid
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        private bool[,] grid;

        public TetrominoGrid(int width, int height)
        {
            Width = width;
            Height = height;
            grid = new bool[width, height];
        }

        public TetrominoGrid(int width, int height, bool[,] grid)
        {
            Width = width;
            Height = height;
            this.grid = grid;
        }

        public bool IsInside(Vector2 pos)
        {
            return pos.x >= 0 && pos.x < Width && pos.y >= 0 && pos.y < Height;
        }

        public bool IsCellEmpty(Vector2 pos)
        {
            return IsInside(pos) && !grid[(int)pos.x, (int)pos.y];
        }

        public bool CanPlace(List<Vector2> shape, Vector2 offset)
        {
            foreach (var cell in shape)
            {
                var gridPos = offset + new Vector2(Mathf.RoundToInt(cell.x), Mathf.RoundToInt(cell.y));
                if (!IsInside(gridPos) || grid[(int)gridPos.x, (int)gridPos.y])
                    return false;
            }
            return true;
        }

        public bool PlaceBrick(Tetromino tetromino, Vector2 offset)
        {
            var shape = tetromino.Cells;
            if (!CanPlace(shape, offset))
                return false;

            foreach (var cell in shape)
            {
                var gridPos = offset + new Vector2(Mathf.RoundToInt(cell.x), Mathf.RoundToInt(cell.y));
                grid[(int)gridPos.x, (int)gridPos.y] = true;
            }

            return true;
        }

        public void Clear()
        {
            grid = new bool[Width, Height];
        }

        public bool[,] GetGridCopy()
        {
            var copy = new bool[Width, Height];
            System.Array.Copy(grid, copy, grid.Length);
            return copy;
        }

        public void PrintToConsole()
        {
            for (int y = Height - 1; y >= 0; y--)
            {
                string line = "";
                for (int x = 0; x < Width; x++)
                {
                    line += grid[x, y] ? "X" : ".";
                }
                Debug.Log(line);
            }
        }
    }

}
