using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using DigitalWorld.Logic;
using Dream.FixMath;

namespace DigitalWorld.Game.Datas
{
    [Serializable]
    public class GridData : DataBase
    {
        #region Params
        /// <summary>
        /// 索引号
        /// </summary>
        public int index;
        /// <summary>
        /// 位置
        /// </summary>
        public FixVector3 position;
        #endregion

        #region Construct
        public GridData(int index, FixVector3 position)
        {
            this.index = index;
            this.position = position;
        }
        #endregion

        #region Serialization
        public GridData(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            this.index = (int)info.GetValue("index", typeof(int));
            this.position = (FixVector3)info.GetValue("position", typeof(FixVector3));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("index", this.index);
            info.AddValue("position", this.position);
        }
        #endregion
    }
}
