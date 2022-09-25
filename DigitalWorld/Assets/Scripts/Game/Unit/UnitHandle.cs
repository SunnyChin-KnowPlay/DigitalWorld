using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalWorld.Game
{
    public struct UnitHandle : IEquatable<UnitHandle>
    {
        public uint Uid => uid;
        private uint uid;
        private ControlUnit obj;

        public ControlUnit Unit => obj;

        public static UnitHandle Null = default;

        public UnitHandle(ControlUnit obj)
        {
            if (null != obj && obj.Uid > 0u)
            {
                this.uid = obj.Uid;
                this.obj = obj;
            }
            else
            {
                this.uid = 0u;
                this.obj = (ControlUnit)((object)null);
            }
        }

        public void SyncUid()
        {
            this.uid = (null == this.obj) ? 0u : obj.Uid;
        }

        public void Reset()
        {
            this.uid = 0u;
            this.obj = (ControlUnit)((object)null);
        }

        public bool Equals(UnitHandle other)
        {
            return this == other;
        }

        public override bool Equals(object obj)
        {
            return obj != null && obj.GetType() == base.GetType() && this == (UnitHandle)obj;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static implicit operator bool(UnitHandle ptr)
        {
            return null != ptr.obj && ptr.obj.Uid == ptr.uid;
        }

        public static bool operator ==(UnitHandle lhs, UnitHandle rhs)
        {
            return lhs.obj == rhs.obj && lhs.uid == rhs.uid;
        }

        public static bool operator !=(UnitHandle lhs, UnitHandle rhs)
        {
            return lhs.obj != rhs.obj || lhs.uid != rhs.uid;
        }

        public static implicit operator ControlUnit(UnitHandle ptr)
        {
            return ptr.Unit;
        }
    }
}
