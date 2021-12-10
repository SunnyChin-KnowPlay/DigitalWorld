using UnityEngine.AI;

namespace DigitalWorld.Logic
{
    /// <summary>
    /// 砖块对象
    /// </summary>
    public class ControlTile : ControlLogic
    {
        #region Params
        protected NavMeshObstacle obstacle = null;
        #endregion

        #region Behaviour
        protected override void Awake()
        {
            base.Awake();

            this.obstacle = this.GetComponent<NavMeshObstacle>();
        }
        #endregion
    }
}
