using DigitalWorld.Game;
using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using System.Xml;
#endif

namespace DigitalWorld.Logic
{
	/// <summary>
    /// 
    /// </summary>
	public partial class ActionKillCharacter : ActionBase
	{
#region Common
		public override int Id
		{
			get
			{
				return 2;
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
			ActionKillCharacter v = null;
			if (Application.isPlaying)
            {
				v = Dream.Core.ObjectPool<ActionKillCharacter>.Instance.Allocate();
            }
			else
			{
				v = new ActionKillCharacter();
			}
			
			if (null != v)
			{
				this.CloneTo(v);
			}

            return v;
        }

		public override T CloneTo<T>(T obj)
        {
            ActionKillCharacter v = base.CloneTo(obj) as ActionKillCharacter;
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
                return "";
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
		protected override void OnEncode(byte[] buffer, int pos)
        {
            base.OnEncode(buffer, pos);
          
        }

        protected override void OnDecode(byte[] buffer, int pos)
        {
            base.OnDecode(buffer, pos);
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
