using DigitalWorld.Game;
using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using System.Xml;
#endif

namespace DigitalWorld.Logic
{
	/// <summary>
    /// 创建角色
    /// </summary>
	public partial class ActionCreateCharacter : ActionBase
	{
#region Common
		public override int Id
		{
			get
			{
				return 1;
			}
		}
		/// <summary>
        /// 名字
        /// </summary> 
		public string name = default(string);

		public override void OnAllocate()
        {
			base.OnAllocate();
			name = default(string);
        }

		public override void OnRecycle()
        {
            base.OnRecycle();
			name = default(string);
         }

		public override object Clone()
        {
			ActionCreateCharacter v = null;
			if (Application.isPlaying)
            {
				v = Dream.Core.ObjectPool<ActionCreateCharacter>.Instance.Allocate();
            }
			else
			{
				v = new ActionCreateCharacter();
			}
			
			if (null != v)
			{
				this.CloneTo(v);
			}

            return v;
        }

		public override T CloneTo<T>(T obj)
        {
            ActionCreateCharacter v = base.CloneTo(obj) as ActionCreateCharacter;
            if (null != v)
            {
				v.name = this.name;
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
                    descs.Add("name", "名字");
                }

                return descs;
            }
        }

        public override string Desc
        {
            get
            {
                return "创建角色";
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
			CalculateSize(this.name);
  
        }

		protected override void OnEncode()
        {
            base.OnEncode();
			Encode(this.name);
          
        }

        protected override void OnDecode()
        {
            base.OnDecode();
			Decode(ref this.name);
        }
		
#if UNITY_EDITOR
        protected override void OnDecode(XmlElement node)
        {
            base.OnDecode(node);
			Decode(ref this.name, "name");
        }

        protected override void OnEncode(XmlElement node)
        {
			base.OnEncode(node);
			Encode(this.name, "name");
        }


#endif
#endregion
	}
}
