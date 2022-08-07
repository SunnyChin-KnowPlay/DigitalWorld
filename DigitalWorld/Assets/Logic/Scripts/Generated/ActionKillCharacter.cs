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
		/// <summary>
        /// 是否直接死亡
        /// </summary> 
		public bool isDeath = default(bool);

		public override void OnAllocate()
        {
			base.OnAllocate();
			isDeath = default(bool);
        }

		public override void OnRecycle()
        {
            base.OnRecycle();
			isDeath = default(bool);
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
				v.isDeath = this.isDeath;
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
                    descs.Add("isDeath", "是否直接死亡");
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
		protected override void OnCalculateSize()
        {
            base.OnCalculateSize();
			CalculateSize(this.isDeath);
  
        }

		protected override void OnEncode()
        {
            base.OnEncode();
			Encode(this.isDeath);
          
        }

        protected override void OnDecode()
        {
            base.OnDecode();
			Decode(ref this.isDeath);
        }
		
#if UNITY_EDITOR
        protected override void OnDecode(XmlElement node)
        {
            base.OnDecode(node);
			Decode(ref this.isDeath, "isDeath");
        }

        protected override void OnEncode(XmlElement node)
        {
			base.OnEncode(node);
			Encode(this.isDeath, "isDeath");
        }


#endif
#endregion
	}
}
