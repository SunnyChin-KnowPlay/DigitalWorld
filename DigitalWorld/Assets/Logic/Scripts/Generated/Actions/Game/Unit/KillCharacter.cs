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
    /// 杀掉角色
    /// </summary>
	public partial class KillCharacter : ActionBase
	{
#region Common
		public override int Id
		{
			get
			{
				return 1;
			}
		}

		public override void OnAllocate()
        {
			base.OnAllocate();

        }

		public override void OnRecycle()
        {
            base.OnRecycle();
        }

		public override object Clone()
        {
			KillCharacter v = null;
			if (Application.isPlaying)
            {
				v = Dream.Core.ObjectPool<KillCharacter>.Instance.Allocate();
            }
			else
			{
				v = new KillCharacter();
			}
			
			if (null != v)
			{
				this.CloneTo(v);
			}

            return v;
        }

		public override T CloneTo<T>(T obj)
        {
            KillCharacter v = base.CloneTo(obj) as KillCharacter;
            if (null != v)
            {
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
                }

                return descs;
            }
        }

        public override string Desc
        {
            get
            {
                return "杀掉角色";
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

#region Serializion
		protected override void OnCalculateSize()
        {
            base.OnCalculateSize();
  
        }

		protected override void OnEncode()
        {
            base.OnEncode();
          
        }

        protected override void OnDecode()
        {
            base.OnDecode();
			
        }
		
#if UNITY_EDITOR
        protected override void OnDecode(XmlElement node)
        {
            base.OnDecode(node);
			
        }

        protected override void OnEncode(XmlElement node)
        {
			base.OnEncode(node);
        }


#endif
#endregion
	}
}
