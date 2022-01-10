using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalWorld.Game
{
    public struct CharacterHandle : IEquatable<CharacterHandle>
    {
        public uint uid;
        private ControlCharacter obj;

        public ControlCharacter Character => obj;

        public static CharacterHandle Null = default(CharacterHandle);

        public CharacterHandle(ControlCharacter obj)
        {
            if (null != obj && obj.Uid > 0u)
            {
                this.uid = obj.Uid;
                this.obj = obj;
            }
            else
            {
                this.uid = 0u;
                this.obj = (ControlCharacter)((object)null);
            }
        }

        public void SyncUid()
        {
            this.uid = (null == this.obj) ? 0u : obj.Uid;
        }

        public void Reset()
        {
            this.uid = 0u;
            this.obj = (ControlCharacter)((object)null);
        }

        public bool Equals(CharacterHandle other)
        {
            return this == other;
        }

        public override bool Equals(object obj)
        {
            return obj != null && obj.GetType() == base.GetType() && this == (CharacterHandle)obj;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static implicit operator bool(CharacterHandle ptr)
        {
            return null != ptr.obj && ptr.obj.Uid == ptr.uid;
        }

        public static bool operator ==(CharacterHandle lhs, CharacterHandle rhs)
        {
            return lhs.obj == rhs.obj && lhs.uid == rhs.uid;
        }

        public static bool operator !=(CharacterHandle lhs, CharacterHandle rhs)
        {
            return lhs.obj != rhs.obj || lhs.uid != rhs.uid;
        }

        public static implicit operator ControlCharacter(CharacterHandle ptr)
        {
            return ptr.Character;
        }
    }
}
