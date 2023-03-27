using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalWorld.Game
{
    public class ManagerBuilding : UnitManager
    {
        #region Override
        public override string Name => "Buildings";
        public override EUnitType Type => EUnitType.Building;
        #endregion
    }
}
