using Dream.Core;
using UnityEngine;

namespace DigitalWorld.Logic
{
    public sealed partial class LogicHelper : Singleton<LogicHelper>
    {
        /// <summary>
        /// 直接从池子里找到对应的节点
        /// 运行时会从池子中获取
        /// 非运行下直接new
        /// </summary>
        /// <typeparam name="T">Node的派生类</typeparam>
        /// <returns>你要的东西</returns>
        public T GetNode<T>() where T : NodeBase, new()
        {
            if (Application.isPlaying)
            {
                return ObjectPool<T>.Instance.Allocate();
            }

            return new T();
        }


    }
}
