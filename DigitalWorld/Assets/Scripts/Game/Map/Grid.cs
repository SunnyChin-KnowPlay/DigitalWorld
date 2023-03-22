using Dream.Core;
using UnityEngine;

namespace DigitalWorld.Game
{
    public class Grid : IPooledObject
    {
        #region Params
        /// <summary>
        /// 方向
        /// </summary>
        public enum Direction
        {
            Up,
            Left,
            Down,
            Right
        }

        private int index;
        public int Index
        {
            get => index;
            private set => index = value;
        }

        private Vector2Int position;
        public Vector2Int Position
        {
            get => position;
            private set => position = value;
        }

        private bool isWalkable;
        public bool IsWalkable
        {
            get => isWalkable;
            set => isWalkable = value;
        }

        private Map map;
        #endregion

        #region Logic
        public virtual void Setup(Map map, int index, Vector2Int position, bool isWalkable)
        {
            this.map = map;
            this.index = index;
            this.position = position;
            this.isWalkable = isWalkable;   
        }

        public Grid GetNeighbor(Map map, Direction direction)
        {
            int newX = Position.x;
            int newY = Position.y;

            switch (direction)
            {
                case Direction.Up:
                    newY++;
                    break;
                case Direction.Left:
                    newX--;
                    break;
                case Direction.Down:
                    newY--;
                    break;
                case Direction.Right:
                    newX++;
                    break;
            }

            return map.GetGridAt(newX, newY);
        }
        #endregion

        #region Pool
        public void OnAllocate()
        {

        }

        public void OnRecycle()
        {
            this.index = 0;
            this.position = Vector2Int.zero;
            this.isWalkable = false;
            this.map = null;
        }

        private IObjectPool pool;
        public void SetPool(IObjectPool pool)
        {
            this.pool = pool;
        }

        public void Recycle()
        {
            if (null != pool)
            {
                this.pool.ApplyRecycle(this);
            }
        }
        #endregion
    }

}
