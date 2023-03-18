/**
 * 该文件通过代码生成器生成
 * 请不要修改这些代码
 * 当然，修改了也没什么用，如果你有兴趣你可以试试。
 */
using DigitalWorld.Game;
using UnityEngine;
using System;
using Dream;
using Dream.Core;
using System.Collections.Generic;
#if UNITY_EDITOR
using System.Xml;
#endif

namespace DigitalWorld.Logic.Actions.Game.Unit
{
	/// <summary>
    /// 播放动画
    /// </summary>
	public partial class PlayAnimator : ActionBase
	{
#region Common
		public override int Id
		{
			get
			{
				return 5;
			}
		}
		/// <summary>
        /// 动画控制器的触发键
        /// </summary> 
		public System.String triggerKey = default;

		public override void OnAllocate()
        {
			base.OnAllocate();

        }

		public override void OnRecycle()
        {
            base.OnRecycle();
			triggerKey = default;
        }

		public override object Clone()
        {
			PlayAnimator v = null;
			if (Application.isPlaying)
            {
				v = Dream.Core.ObjectPool<PlayAnimator>.Instance.Allocate();
            }
			else
			{
				v = new PlayAnimator();
			}
			
			if (null != v)
			{
				this.CloneTo(v);
			}

            return v;
        }

		public override T CloneTo<T>(T obj)
        {
            PlayAnimator v = base.CloneTo(obj) as PlayAnimator;
            if (null != v)
            {
				v.triggerKey = this.triggerKey;
            }
            return obj;
        }
#endregion

#region Editor
#if UNITY_EDITOR
        private static Dictionary<string, string> descs = new Dictionary<string, string>();

        private static Dictionary<string, string> Descs
        {
            get
            {
                if (null == descs)
                    descs = new Dictionary<string, string>();

                if (descs.Count < 1)
                {
                    descs.Add("triggerKey", "动画控制器的触发键");
                }

                return descs;
            }
        }

        public override string Desc
        {
            get
            {
                return "播放动画";
            }
        }

        protected override string GetFieldDesc(string fieldName)
        {
            Dictionary<string, string> descs = Descs;
            string v = string.Empty;
            descs.TryGetValue(fieldName, out v);
            return v;
        }
#endif
#endregion


	}
}
