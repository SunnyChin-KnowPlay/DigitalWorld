using DigitalWorld.Game;
using UnityEngine;
using System;
using Dream;
using Dream.Core;
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
		public Property<Int32> name = default(Property<Int32>);
		/// <summary>
        /// 测试枚举
        /// </summary> 
		public Property<DigitalWorld.Proto.Logic.EEventType> testType = default(Property<DigitalWorld.Proto.Logic.EEventType>);

		public override void OnAllocate()
        {
			base.OnAllocate();

        }

		public override void OnRecycle()
        {
            base.OnRecycle();
			if (null != name)
			{
				name.Recycle();
				name = null;
			}	
			if (null != testType)
			{
				testType.Recycle();
				testType = null;
			}	
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
                    descs.Add("testType", "测试枚举");
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
	
			if (null != this.name)
				CalculateSize(this.name);
	
			if (null != this.testType)
				CalculateSize(this.testType);
  
        }

		protected override void OnEncode()
        {
            base.OnEncode();
			if (null != this.name)
				Encode(this.name);
			if (null != this.testType)
				Encode(this.testType);
          
        }

        protected override void OnDecode()
        {
            base.OnDecode();
			int id = 0;
			ParseId(this._buffer, this._pos, out id);
			if (null == this.name)
			{
				this.name = LogicHelper.GetProperty<Property<Int32>>(id);
			}
			Decode(ref this.name);
			ParseId(this._buffer, this._pos, out id);
			if (null == this.testType)
			{
				this.testType = LogicHelper.GetProperty<Property<DigitalWorld.Proto.Logic.EEventType>>(id);
			}
			Decode(ref this.testType);
        }
		
#if UNITY_EDITOR
        protected override void OnDecode(XmlElement node)
        {
            base.OnDecode(node);
			
			if (null == this.name)
			{
				XmlElement ele = node["name"];
				if (null != ele)
				{
					System.Type type = null;
                    bool ret = ParseType(ele, out type);
                    if (ret)
                    {
                        if (System.Activator.CreateInstance(type) is Property<Int32> child)
                        {
                            this.name = child;
                        }
					}
				}
			}
			Decode(ref this.name, "name");
			if (null == this.testType)
			{
				XmlElement ele = node["testType"];
				if (null != ele)
				{
					System.Type type = null;
                    bool ret = ParseType(ele, out type);
                    if (ret)
                    {
                        if (System.Activator.CreateInstance(type) is Property<DigitalWorld.Proto.Logic.EEventType> child)
                        {
                            this.testType = child;
                        }
					}
				}
			}
			Decode(ref this.testType, "testType");
        }

        protected override void OnEncode(XmlElement node)
        {
			base.OnEncode(node);
			Encode(this.name, "name");
			Encode(this.testType, "testType");
        }


#endif
#endregion
	}
}
