using Dream.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DigitalWorld.Logic
{
    public sealed partial class LogicHelper : Singleton<LogicHelper>
    {
        /// <summary>
        /// 直接从池子里找到对应的Action
        /// </summary>
        /// <typeparam name="T">Action的派生类</typeparam>
        /// <returns>你要的东西</returns>
        public T GetAction<T>() where T : ActionBase, new()
        {
            if (Application.isPlaying)
            {
                return ObjectPool<T>.Instance.Allocate();
            }

            return new T();
        }

        /// <summary>
        /// 直接从池子里找到对应的Condition
        /// </summary>
        /// <typeparam name="T">Condition的派生类</typeparam>
        /// <returns>你要的东西</returns>
        public T GetCondition<T>() where T : ConditionBase, new()
        {
            if (Application.isPlaying)
            {
                return ObjectPool<T>.Instance.Allocate();
            }

            return new T();
        }
    }
}
