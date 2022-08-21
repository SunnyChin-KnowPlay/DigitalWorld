using DigitalWorld.Game;
using UnityEngine;
using System;
using Dream;
using Dream.Core;
using System.Collections.Generic;
#if UNITY_EDITOR
using System.Xml;
#endif

namespace DigitalWorld.Logic.Action
{
	/// <summary>
    /// 空行动
    /// </summary>
	public partial class None : ActionBase
	{
#region Common
		public override int Id
		{
			get
			{
				return 0;
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
			None v = null;
			if (Application.isPlaying)
            {
				v = Dream.Core.ObjectPool<None>.Instance.Allocate();
            }
			else
			{
				v = new None();
			}
			
			if (null != v)
			{
				this.CloneTo(v);
			}

            return v;
        }

		public override T CloneTo<T>(T obj)
        {
            None v = base.CloneTo(obj) as None;
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
                return "空行动";
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
