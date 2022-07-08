using System;

namespace DigitalWorld.Logic
{
    public abstract partial class ConditionBase : Effect
    {
        #region Params
        public override int Id => throw new NotImplementedException();

        protected ECheckOperator[] operators = null;
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
    }
}
