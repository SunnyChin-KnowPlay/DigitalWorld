using System;

namespace DigitalWorld.Game
{
    internal class EventArgsTarget : EventArgs
    {
        public UnitHandle Target => target;
        private UnitHandle target;

        public EventArgsTarget(UnitHandle target)
        {
            this.target = target;
        }
    }
}
