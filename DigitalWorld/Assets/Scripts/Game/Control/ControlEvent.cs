using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalWorld.Game
{
    public class ControlEvent : ControlLogic
    {
        #region Logic
        public void Invoke(EUnitEventType type, EventArgs args)
        {
            UnitEventManager.Instance.Invoke(this.UnitHandle, type, args);
        }
        #endregion
    }
}
