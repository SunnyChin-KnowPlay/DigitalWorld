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
    /// 单位造成伤害
    /// </summary>
    public partial class Damage : ActionBase
    {
        #region Common
        public override int Id
        {
            get
            {
                return 4;
            }
        }
        /// <summary>
        /// 伤害类型
        /// </summary> 
        public DigitalWorld.Game.EDamagerType damageType = default;

        public override void OnAllocate()
        {
            base.OnAllocate();

        }

        public override void OnRecycle()
        {
            base.OnRecycle();
            damageType = default;
        }

        public override object Clone()
        {
            Damage v = null;
            if (Application.isPlaying)
            {
                v = Dream.Core.ObjectPool<Damage>.Instance.Allocate();
            }
            else
            {
                v = new Damage();
            }

            if (null != v)
            {
                this.CloneTo(v);
            }

            return v;
        }

        public override T CloneTo<T>(T obj)
        {
            Damage v = base.CloneTo(obj) as Damage;
            if (null != v)
            {
                v.damageType = this.damageType;
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
                    descs.Add("damageType", "伤害类型");
                }

                return descs;
            }
        }

        public override string Desc
        {
            get
            {
                return "单位造成伤害";
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

            CalculateSizeEnum(this.damageType);

        }

        protected override void OnEncode()
        {
            base.OnEncode();
            EncodeEnum(this.damageType);

        }

        protected override void OnDecode()
        {
            base.OnDecode();

            DecodeEnum(ref this.damageType);
        }

#if UNITY_EDITOR
        protected override void OnDecode(XmlElement node)
        {
            base.OnDecode(node);

            DecodeEnum(ref this.damageType, "damageType");
        }

        protected override void OnEncode(XmlElement node)
        {
            base.OnEncode(node);
            EncodeEnum(this.damageType, "damageType");
        }


#endif
        #endregion
    }
}
