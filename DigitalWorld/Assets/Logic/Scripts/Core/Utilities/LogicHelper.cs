using DigitalWorld.Asset;
using Dream.Core;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using UnityEngine;

namespace DigitalWorld.Logic
{
    public static partial class LogicHelper
    {
        #region Event
        public delegate void OnEditNodeHandle(ENodeType nodeType, NodeBase parent, NodeBase initialNode);
        public static event OnEditNodeHandle OnEditNode;
        #endregion

        #region Apply
        public static void ApplyEditNode(ENodeType nodeType, NodeBase parent, NodeBase initialNode)
        {
            if (null != OnEditNode)
            {
                OnEditNode.Invoke(nodeType, parent, initialNode);
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
        #endregion

        #region Allocate
        /// <summary>
        /// 分配一个触发器
        /// </summary>
        /// <param name="path">触发器的资产路径</param>
        /// <returns></returns>
        public static Trigger AllocateTrigger(string path)
        {
            ByteAsset asset = AssetManager.LoadAsset<ByteAsset>(path);
            if (null == asset)
                return null;

            using (Stream stream = new MemoryStream(asset.bytes))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                Trigger trigger = formatter.Deserialize(stream) as Trigger;

                return trigger;
            }
        }
        #endregion
    }
}
