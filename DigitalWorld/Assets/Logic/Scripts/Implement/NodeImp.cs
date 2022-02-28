namespace DigitalWorld.Logic
{
    public partial class BaseNode
    {
        #region Logic
        public virtual void ToState(EState state)
        {
            if (State != state)
            {
                EState lastState = State;
                State = state;
                OnStateChanged(lastState);
                if (State == EState.End)
                {
                    State = EState.Idle;
                }
            }
        }

        protected virtual void OnStateChanged(EState laststate)
        {

        }
        #endregion
    }
}
