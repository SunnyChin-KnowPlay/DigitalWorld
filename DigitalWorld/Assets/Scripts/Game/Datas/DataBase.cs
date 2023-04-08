using DigitalWorld.Logic;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DigitalWorld.Game.Datas
{
    [System.Serializable]
    public abstract class DataBase : ISerializable
    {
        #region Construct
        public DataBase()
        {
        }
        #endregion

        #region Serialization
        protected DataBase(SerializationInfo info, StreamingContext context)
        {

        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {

        }
        #endregion
    }
}
