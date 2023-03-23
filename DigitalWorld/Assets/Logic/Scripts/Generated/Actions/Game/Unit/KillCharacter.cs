// ------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//  
//     对此文件的更改可能导致不正确的行为，如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
// ------------------------------------------------------------------------------
using DigitalWorld.Game;
using DigitalWorld.Logic.Properties;
using UnityEngine;
using System;
using Dream;
using Dream.Core;
using System.Runtime.Serialization;
using System.Collections.Generic;
#if UNITY_EDITOR
using System.Xml;
#endif

namespace DigitalWorld.Logic.Actions.Game.Unit
{
	/// <summary>
    /// 杀掉角色
    /// </summary>
	[Serializable]
	public partial class KillCharacter : ActionBase
	{
		#region Properties
       
        #endregion

		#region Fields
		#endregion
		
		#region Common
		public override int Id
		{
			get
			{
				return 1;
			}
		}

#if UNITY_EDITOR
		/// <summary>
        /// 当被编辑器创建时的回调
        /// </summary>
        public override void OnCreate()
        {
			base.OnCreate();
            Type[] types = null;

  
        }
#endif

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
        private static Dictionary<string, string> fieldDescs = new Dictionary<string, string>();

        private static Dictionary<string, string> FieldDescs
        {
            get
            {
                if (null == fieldDescs)
                    fieldDescs = new Dictionary<string, string>();

                if (fieldDescs.Count < 1)
                {
                }

                return fieldDescs;
            }
        }

		protected override string GetFieldDesc(string fieldName)
        {
            Dictionary<string, string> descs = FieldDescs;
            string v = string.Empty;
            descs.TryGetValue(fieldName, out v);
            return v;
        }

        public override string Desc
        {
            get
            {
                return "杀掉角色";
            }
        }
#endif
		#endregion

		#region Serialization
		public KillCharacter()
		{

		}

        public KillCharacter(SerializationInfo info, StreamingContext context)
        {
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

        }
        #endregion
	}
}
