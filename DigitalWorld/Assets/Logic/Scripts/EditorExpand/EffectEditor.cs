#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace DigitalWorld.Logic
{
    public partial class Effect
    {
#if UNITY_EDITOR
        #region GUI
        public override void OnGUI()
        {
            base.OnGUI();
        }

        
        #endregion
#endif
    }
}
