using DigitalWorld.Game;
using UnityEngine;
using System;
using Dream;
using Dream.Core;
using System.Collections.Generic;
#if UNITY_EDITOR
using System.Xml;
#endif

namespace DigitalWorld.Logic.Actions
{
	/// <summary>
    /// 创建角色
    /// </summary>
	public partial class CreateCharacter : ActionBase
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
        /// 角色配置ID
        /// </summary> 
		public System.Int32 cfgId = default;
		/// <summary>
        /// 世界坐标
        /// </summary> 
		public Dream.FixMath.FixVector3 worldPosition = default;

		public override void OnAllocate()
        {
			base.OnAllocate();

        }

		public override void OnRecycle()
        {
            base.OnRecycle();
			cfgId = default;
			worldPosition = default;
        }

		public override object Clone()
        {
			CreateCharacter v = null;
			if (Application.isPlaying)
            {
				v = Dream.Core.ObjectPool<CreateCharacter>.Instance.Allocate();
            }
			else
			{
				v = new CreateCharacter();
			}
			
			if (null != v)
			{
				this.CloneTo(v);
			}

            return v;
        }

		public override T CloneTo<T>(T obj)
        {
            CreateCharacter v = base.CloneTo(obj) as CreateCharacter;
            if (null != v)
            {
				v.cfgId = this.cfgId;
				v.worldPosition = this.worldPosition;
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
                    descs.Add("cfgId", "角色配置ID");
                    descs.Add("worldPosition", "世界坐标");
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
	
			CalculateSize(this.cfgId);
	
			CalculateSize(this.worldPosition);
  
        }

		protected override void OnEncode()
        {
            base.OnEncode();
			Encode(this.cfgId);
			Encode(this.worldPosition);
          
        }

        protected override void OnDecode()
        {
            base.OnDecode();
			
			Decode(ref this.cfgId);
			Decode(ref this.worldPosition);
        }
		
#if UNITY_EDITOR
        protected override void OnDecode(XmlElement node)
        {
            base.OnDecode(node);
			
			Decode(ref this.cfgId, "cfgId");
			Decode(ref this.worldPosition, "worldPosition");
        }

        protected override void OnEncode(XmlElement node)
        {
			base.OnEncode(node);
			Encode(this.cfgId, "cfgId");
			Encode(this.worldPosition, "worldPosition");
        }


#endif
#endregion
	}
}
