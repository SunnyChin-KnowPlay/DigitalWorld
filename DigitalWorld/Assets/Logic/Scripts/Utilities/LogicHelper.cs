using Dream.Core;
using System.Xml;
using UnityEngine;

namespace DigitalWorld.Logic
{
    public static partial class LogicHelper
    {
        #region Event
        public delegate void OnAddNodeHandle(ENodeType nodeType, NodeBase parent);
        public static event OnAddNodeHandle OnAddNode;

        public delegate void OnEditNodeHandle(ENodeType nodeType, NodeBase parent, NodeBase initialNode);
        public static event OnEditNodeHandle OnEditNode;
        #endregion

        #region Apply
        public static void ApplyAddNode(ENodeType nodeType, NodeBase parent)
        {
            if (null != OnAddNode)
            {
                OnAddNode.Invoke(nodeType, parent);
            }
        }
        #endregion

        #region Get
        /// <summary>
        /// 直接从池子里找到对应的节点
        /// 运行时会从池子中获取
        /// 非运行下直接new
        /// </summary>
        /// <typeparam name="T">Node的派生类</typeparam>
        /// <returns>你要的东西</returns>
        public static T GetNode<T>() where T : NodeBase, new()
        {
            if (Application.isPlaying)
            {
                return ObjectPool<T>.Instance.Allocate();
            }

            return new T();
        }

        /// <summary>
        /// 通过xml来获取对应的节点对象
        /// </summary>
        /// <param name="element">xml信息</param>
        /// <returns>如果xml找不到或者异常 则返回null</returns>
        public static NodeBase GetNode(XmlElement element)
        {
            if (null == element)
                return null;

            if (NodeBase.ParseType(element, out System.Type type))
            {
                return System.Activator.CreateInstance(type) as NodeBase;
            }

            return null;
        }

        /// <summary>
        /// 通过二进制流来获取对应的节点对象
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        public static NodeBase GetNode(byte[] buffer, int pos)
        {
            if (null == buffer)
                return null;

            if (NodeBase.ParseType(buffer, pos, out ENodeType nodeType, out int id))
            {
                return GetNode(nodeType, id);
            }
            return null;
        }
        #endregion
    }
}
