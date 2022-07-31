using System;
using System.Collections.Generic;
using System.Xml;

namespace DigitalWorld.Logic
{
    public abstract partial class ConditionBase : Effect
    {
        #region Params
        public override int Id => throw new NotImplementedException();

        public override ENodeType NodeType => ENodeType.Condition;
        /// <summary>
        /// 与字段数量一致 每个字段的操作符
        /// </summary>
        protected ECheckOperator[] operators = null;
        #endregion

        #region Common
        public override T CloneTo<T>(T obj)
        {
            ConditionBase bc = base.CloneTo(obj) as ConditionBase;
            if (null != bc)
            {
                for (int i = 0; i < this.operators.Length; ++i)
                {
                    bc.operators[i] = this.operators[i];
                }
            }

            return bc as T;
        }
        #endregion

        #region Logic
        public override bool CheckRequirement()
        {
            return base.CheckRequirement();
        }
        #endregion

        #region Oper
        protected static bool CheckValueOper(int p1, int p2, ECheckOperator oper)
        {
            switch (oper)
            {
                case ECheckOperator.Equal:
                {
                    return p1 == p2;
                }
                case ECheckOperator.NotEqual:
                {
                    return p1 != p2;
                }
                case ECheckOperator.GreaterThanOrEquipTo:
                {
                    return p1 >= p2;
                }
                case ECheckOperator.GreaterThan:
                {
                    return p1 > p2;
                }
                case ECheckOperator.LessThanOrEquipTo:
                {
                    return p1 <= p2;
                }
                case ECheckOperator.LessThan:
                {
                    return p1 < p2;
                }
            }

            return false;
        }

        protected static bool CheckValueOper(float p1, float p2, ECheckOperator oper)
        {
            switch (oper)
            {
                case ECheckOperator.Equal:
                {
                    return p1 == p2;
                }
                case ECheckOperator.NotEqual:
                {
                    return p1 != p2;
                }
                case ECheckOperator.GreaterThanOrEquipTo:
                {
                    return p1 >= p2;
                }
                case ECheckOperator.GreaterThan:
                {
                    return p1 > p2;
                }
                case ECheckOperator.LessThanOrEquipTo:
                {
                    return p1 <= p2;
                }
                case ECheckOperator.LessThan:
                {
                    return p1 < p2;
                }
            }

            return false;
        }

        protected static bool CheckValueOper(double p1, double p2, ECheckOperator oper)
        {
            switch (oper)
            {
                case ECheckOperator.Equal:
                {
                    return p1 == p2;
                }
                case ECheckOperator.NotEqual:
                {
                    return p1 != p2;
                }
                case ECheckOperator.GreaterThanOrEquipTo:
                {
                    return p1 >= p2;
                }
                case ECheckOperator.GreaterThan:
                {
                    return p1 > p2;
                }
                case ECheckOperator.LessThanOrEquipTo:
                {
                    return p1 <= p2;
                }
                case ECheckOperator.LessThan:
                {
                    return p1 < p2;
                }
            }

            return false;
        }

        public static bool CheckValueOper<T>(T p1, T p2, ECheckOperator oper) where T : Enum
        {
            int v1 = Convert.ToInt32(p1);
            int v2 = Convert.ToInt32(p2);

            return CheckValueOper(v1, v2, oper);
        }

        protected virtual ECheckOperator GetOperatorFromField(string fieldName)
        {
            throw new NotImplementedException();
        }

        protected virtual void SetOperatorByField(string fieldName, ECheckOperator oper)
        {
            throw new NotImplementedException();
        }

        protected virtual string GetOperKey(int index)
        {
            return null;
        }

        protected ECheckOperator GetOper(int index)
        {
            if (index < 0 || index >= this.operators.Length)
                throw new ArgumentOutOfRangeException();

            return operators[index];
        }

        protected void SetOper(int index, ECheckOperator oper)
        {
            if (index < 0 || index >= this.operators.Length)
                throw new ArgumentOutOfRangeException();

            this.operators[index] = oper;
        }
        #endregion

        #region Proto
        protected override void OnEncode(XmlElement element)
        {
            base.OnEncode(element);

            XmlElement operatorsEle = element.OwnerDocument.CreateElement("operators");
            element.AppendChild(operatorsEle);
            for (int i = 0; i < this.operators.Length; ++i)
            {
                string operKey = this.GetOperKey(i);
                XmlElement ele = element.OwnerDocument.CreateElement(operKey);
                ele.SetAttribute("operator", this.GetOperatorFromField(operKey).ToString());

                operatorsEle.AppendChild(ele);
            }
        }

        protected override void OnDecode(XmlElement element)
        {
            base.OnDecode(element);

            XmlElement operatorsEle = element["operators"];
            if (null != operatorsEle)
            {
                foreach (var subN in operatorsEle.ChildNodes)
                {
                    XmlElement ele = subN as XmlElement;
                    if (ele.HasAttribute("operator"))
                    {
                        string value = ele.GetAttribute("operator");
                        ECheckOperator op = (ECheckOperator)Enum.Parse(typeof(ECheckOperator), value);
                        this.SetOperatorByField(ele.Name, op);
                    }
                }
            }
        }

        protected override void OnCalculateSize()
        {
            base.OnCalculateSize();

            int length = this.operators.Length;
            CalculateSize(length);
            for (int i = 0; i < length; ++i)
            {
                ECheckOperator oper = this.operators[i];
                CalculateSizeEnum(oper);
            }
        }

        protected override void OnEncode()
        {
            base.OnEncode();

            List<int> operators = new List<int>(this.operators.Length);
            for (int i = 0; i < this.operators.Length; ++i)
            {
                operators.Add((int)this.operators[i]);
            }
            Encode(operators);
        }

        protected override void OnDecode()
        {
            base.OnDecode();

            List<int> operators = null;
            Decode(ref operators);

            for (int i = 0; i < operators.Count; ++i)
            {
                this.SetOper(i, (ECheckOperator)operators[i]);
            }
        }
        #endregion
    }
}
