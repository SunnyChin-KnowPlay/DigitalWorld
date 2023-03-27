using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalWorld.Game
{
    public class ManagerCharacter : UnitManager
    {
        #region Override
        public override string Name => "Characters";
        public override EUnitType Type => EUnitType.Character;
        #endregion
    }
}
