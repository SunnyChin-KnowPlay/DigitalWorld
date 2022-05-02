namespace DigitalWorld.Game
{
    public class ControlMove : ControlLogic
    {
        #region Params
        public int MoveSpeed
        {
            get
            {
                return this.unit.Property.MoveSpeed.Value;
            }
        }
        #endregion

        protected override void Awake()
        {
            base.Awake();


        }

        protected override void Update()
        {
            base.Update();
        }

    }
}
