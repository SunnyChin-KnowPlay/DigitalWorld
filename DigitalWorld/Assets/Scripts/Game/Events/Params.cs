using Dream.Core;

namespace DigitalWorld.Game
{
    public class ParamUnit1 : IPooledObject
    {
        public UnitHandle ch1;

        public virtual void OnAllocate()
        {

        }

        public virtual void OnRecycle()
        {
            ch1.Reset();
        }

        public virtual void Recycle()
        {

        }

        public virtual void SetPool(IObjectPool pool)
        {

        }
    }

    public class ParamUnit2 : ParamUnit1
    {
        public UnitHandle ch2;

        public override void OnRecycle()
        {
            base.OnRecycle();

            ch2.Reset();
        }
    }

    public class ParamUnit3 : ParamUnit2
    {
        public UnitHandle ch3;

        public override void OnRecycle()
        {
            base.OnRecycle();

            ch3.Reset();
        }
    }

    public class ParamSkill : ParamUnit2
    {

    }
}
