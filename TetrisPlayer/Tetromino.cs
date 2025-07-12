using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BadAppleTrickyTowersMod.TetrisPlayer
{
    public class Tetromino
    {
        public string ResourceId { get; private set; }
        public List<Vector2> Cells { get; private set; }
        public Vector2 Offset;

        public Tetromino(string resourceId, List<Vector2> cells, Vector2 offset)
        {
            ResourceId = resourceId;
            Cells = cells;
            Offset = offset;
        }

        public List<List<Vector2>> GetRotations()
        {
            var rotations = new List<List<Vector2>>();
            var current = Cells;
            for (int i = 0; i < 4; i++)
            {
                rotations.Add(current);
                // Rotate 90°
                current = current.Select(c => new Vector2(-c.y, c.x)).ToList(); 
            }

            return Deduplicate(rotations);
        }

        private List<List<Vector2>> Deduplicate(List<List<Vector2>> input)
        {
            var unique = new List<List<Vector2>>();

            foreach (var shape in input)
            {
                if (!unique.Any(existing => ShapesEqual(existing, shape)))
                    unique.Add(shape);
            }

            return unique;
        }

        private bool ShapesEqual(List<Vector2> a, List<Vector2> b)
        {
            if (a.Count != b.Count) return false;

            var aSorted = a.OrderBy(p => p.x).ThenBy(p => p.y).ToList();
            var bSorted = b.OrderBy(p => p.x).ThenBy(p => p.y).ToList();

            for (int i = 0; i < aSorted.Count; i++)
            {
                if (aSorted[i] != bSorted[i])
                    return false;
            }

            return true;
        }
    }
}
