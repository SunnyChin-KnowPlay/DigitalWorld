using System;
using UnityEngine;
using Dream.FixMath;

namespace DigitalWorld.Utilities
{
    public static class Convert
    {
        public static Vector3 ToVector3(FixVector3 value)
        {
            return new Vector3(value.X, value.Y, value.Z);
        }

        public static Vector3 ToVector3(FixVector2 value)
        {
            return new Vector3(value.X , value.Y, 0);
        }

        public static Vector2 ToVector2(FixVector3 value)
        {
            return new Vector2(value.X, value.Y);
        }

        public static Vector2 ToVector2(FixVector2 value)
        {
            return new Vector2(value.X, value.Y);
        }
    }
}
