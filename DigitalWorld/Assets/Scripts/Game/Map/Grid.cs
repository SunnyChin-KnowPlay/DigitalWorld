using System.Collections.Generic;
using UnityEngine;

namespace DigitalWorld.Game
{
    public enum Direction
    {
        Up,
        Left,
        Down,
        Right
    }

    public enum GridType
    {
        Empty,
        Wall,
        Water,
        Grass
    }


    public class Grid
    {
        public int index;
        public Vector2Int position;
        public GridType type;
        public bool isWalkable;
        public Map map;

        public Grid(int index, Vector2Int position, GridType type, bool isWalkable, Map map)
        {
            this.index = index;
            this.position = position;
            this.type = type;
            this.isWalkable = isWalkable;
            this.map = map;
        }

        // get neighbor grid in specific direction
        public Grid GetNeighbor(Direction direction)
        {
            Vector2Int neighborPosition = position;
            switch (direction)
            {
                case Direction.Up:
                    neighborPosition += Vector2Int.up;
                    break;
                case Direction.Left:
                    neighborPosition += Vector2Int.left;
                    break;
                case Direction.Down:
                    neighborPosition += Vector2Int.down;
                    break;
                case Direction.Right:
                    neighborPosition += Vector2Int.right;
                    break;
            }
            return map.GetGrid(neighborPosition);
        }
    }

    
    

   
}
