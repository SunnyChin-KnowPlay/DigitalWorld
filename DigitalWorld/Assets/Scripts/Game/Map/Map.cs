using System.Collections.Generic;
using UnityEngine;

namespace DigitalWorld.Game
{
    public class Map
    {
        public int width;
        public int height;
        public List<Grid> grids;

        public Map(int width, int height)
        {
            this.width = width;
            this.height = height;
            this.grids = new List<Grid>();

            // create all grids
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    grids.Add(new Grid(y * width + x, new Vector2Int(x, y), GridType.Empty, true, this));
                }
            }
        }

        // get grid at position
        public Grid GetGrid(Vector2Int position)
        {
            int index = position.y * width + position.x;
            if (index < 0 || index >= grids.Count)
            {
                return null;
            }
            return grids[index];
        }
    }

}
