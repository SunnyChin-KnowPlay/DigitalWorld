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
using System.Xml.Serialization;
using System.Runtime.Serialization;
#if UNITY_EDITOR
using System.Xml;
#endif

namespace DigitalWorld.Logic.Actions.Game.Unit
{
    /// <summary>
    /// 基于单位的创建角色，位置使用相对关系
    /// </summary>
    [Serializable]
    public partial class CreateCharacter : ActionBase
	{
#region Common
		public override int Id
		{
			get
			{
				return 3;
			}
		}
		/// <summary>
        /// 角色配置ID
        /// </summary> 
		public System.Int32 cfgId = default;
		/// <summary>
        /// 本地坐标偏移量
        /// </summary> 
		public Dream.FixMath.FixVector3 localPosition = default;

		public override void OnAllocate()
        {
			base.OnAllocate();

        }

		public override void OnRecycle()
        {
            base.OnRecycle();
			cfgId = default;
			localPosition = default;
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
				v.localPosition = this.localPosition;
            }
            return obj;
        }

        public CreateCharacter()
        {

        }

        public CreateCharacter(SerializationInfo info, StreamingContext context)
        {

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
                    descs.Add("localPosition", "本地坐标偏移量");
                }

                return descs;
            }
        }

        public override string Desc
        {
            get
            {
                return "基于单位的创建角色，位置使用相对关系";
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
