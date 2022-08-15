using DigitalWorld.Game;
using UnityEngine;
using System;
using Dream;
using Dream.Core;

namespace DigitalWorld.Logic
{
	/// <summary>
    /// int型属性测试
    /// </summary>
    public partial class PropertyTestInt32 : Property<Int32>
    {
        public override int Id => 1;

        public override object Clone()
        {
            PropertyTestInt32 v = null;
            if (Application.isPlaying)
            {
                v = Dream.Core.ObjectPool<PropertyTestInt32>.Instance.Allocate();
            }
            else
            {
                v = new PropertyTestInt32();
            }

            if (null != v)
            {
                this.CloneTo(v);
            }

            return v;
        }
    }
}
