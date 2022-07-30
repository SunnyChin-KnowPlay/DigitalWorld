using DigitalWorld.Game;
using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using System.Xml;
#endif

namespace DigitalWorld.Logic
{
	/// <summary>
    /// 运行时间
    /// </summary>
	public partial class ConditionRunningTime : ConditionBase
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
        /// 时长
        /// </summary>
		public float duration = default(float);
		/// <summary>
        /// 测试用
        /// </summary>
		public int testVVV = default(int);
		private enum EValueIndex
		{
			duration = 0, 
			testVVV = 1, 
		}

		public ConditionRunningTime()
		{
			operators = new ECheckOperator[2];
		}
		
		public override void OnAllocate()
        {
			base.OnAllocate();
			duration = default(float);
			testVVV = default(int);
			
        }

		public override void OnRecycle()
        {
            base.OnRecycle();
			duration = default(float);
			testVVV = default(int);
        }

		public override object Clone()
        {
            ConditionRunningTime v = null;
			if (Application.isPlaying)
            {
				v = Dream.Core.ObjectPool<ConditionRunningTime>.Instance.Allocate();
            }
			else
			{
				v = new ConditionRunningTime();
			}
			
			if (null != v)
			{
				this.CloneTo(v);
			}
			
            return v;
        }

		public override T CloneTo<T>(T obj)
        {
            ConditionRunningTime v = base.CloneTo(obj) as ConditionRunningTime;
            if (null != v)
            {
				v.duration = this.duration;
				v.testVVV = this.testVVV;
            }
            return obj;
        }

		private ECheckOperator GetOper(EValueIndex index)
        {
            return GetOper((int)index);
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
                    descs.Add("duration", "时长");
                    descs.Add("testVVV", "测试用");
                }

                return descs;
            }
        }

        public override string Desc
        {
            get
            {
                return "运行时间";
            }
        }

        protected override string GetFieldDesc(string fieldName)
        {
            Dictionary<string, string> descs = Descs;
            string v = string.Empty;
            descs.TryGetValue(fieldName, out v);
            return v;
        }

		protected override ECheckOperator GetOperatorFromField(string fieldName)
        {
            EValueIndex @enum = (EValueIndex)System.Enum.Parse(typeof(EValueIndex), fieldName);
			return GetOper((int)@enum);
        }

        protected override void SetOperatorByField(string fieldName, ECheckOperator oper)
        {
            bool ret = System.Enum.TryParse<EValueIndex>(fieldName, true, out EValueIndex retV);
            if (ret)
            {
                int index = (int)retV;
                SetOper(index, oper);
            }
        }

		protected override string GetOperKey(int index)
        {
            EValueIndex i = (EValueIndex)index;
            return i.ToString();
        }
#endif
		#endregion

		#region Serialization
		protected override void OnCalculateSize()
        {
            base.OnCalculateSize();
			CalculateSize(this.duration);
			CalculateSize(this.testVVV);
  
        }

		protected override void OnEncode()
        {
            base.OnEncode();
			Encode(this.duration);
			Encode(this.testVVV);
          
        }

        protected override void OnDecode()
        {
            base.OnDecode();
			Decode(ref this.duration);
			Decode(ref this.testVVV);
        }

#if UNITY_EDITOR
        protected override void OnDecode(XmlElement node)
        {
            base.OnDecode(node);
			Decode(ref this.duration, "duration");
			Decode(ref this.testVVV, "testVVV");
        }

        protected override void OnEncode(XmlElement node)
        {
			base.OnEncode(node);
			Encode(this.duration, "duration");
			Encode(this.testVVV, "testVVV");
        }
#endif
		#endregion
	}
}
