using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalWorld.Logic
{
    public partial class ActionBase : Effect
    {
        #region Params
        public override int Id => throw new NotImplementedException();
        #endregion

        #region Logic
        protected override void OnUpdate(float delta)
        {
            if (this.State == EState.Idle)
            {
                if (this.GetRequirement())
                {
                    this.State = EState.Running;
                }
            }

            base.OnUpdate(delta);
        }
        #endregion
    }
}
