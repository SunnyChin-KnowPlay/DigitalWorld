using DigitalWorld.Game;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace DigitalWorld.Logic.Events
{
    /// <summary>
    /// 事件
    /// </summary>
    public partial struct Event
    {
        #region Params
        /// <summary>
        /// 事件ID
        /// </summary>
        public int Id
        {
            get; private set;
        }

        /// <summary>
        /// 触发者
        /// </summary>
        public UnitHandle Triggering { get; private set; }

        /// <summary>
        /// 目标
        /// </summary>
        public UnitHandle Target { get; private set; }
        #endregion

        #region Construction
        public static Event Create(int id, UnitHandle triggering, UnitHandle target = default)
        {
            Event ev = new Event
            {
                Id = id,
                Triggering = triggering,
                Target = target,
            };
            return ev;
        }
        #endregion

        #region GUI
#if UNITY_EDITOR
        public void OnGUIStatus()
        {
            UnitHandle handle = this.Triggering;
            if (handle)
            {
                string v = string.Format("Triggering ID:<color=#32D22FFF>{0}</color>", handle.Unit.Uid);
                EditorGUILayout.LabelField(v);
               
                if (GUILayout.Button(new GUIContent("Select Target"), GUILayout.MaxHeight(18)))
                {
                    if (null != handle.Unit.gameObject)
                        Selection.activeGameObject = handle.Unit.gameObject;
                }
            }
        }
#endif
        #endregion
    }
}
