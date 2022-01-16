using DigitalWorld.Proto.Game;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalWorld.Game
{
    public class ControlProperty : ControlLogic
    {
        private Dictionary<EPropertyType, PropertyValue> attributes = new Dictionary<EPropertyType, PropertyValue>();

        #region Setup
        public override void Setup(UnitData data)
        {
            base.Setup(data);

            if (null == attributes)
                attributes = new Dictionary<EPropertyType, PropertyValue>();
            else
                attributes.Clear();

            PropertyValue av;
            av = new PropertyValue(0, data.Hp, data.Hp);
            attributes.Add(EPropertyType.Hp, av);

            av = new PropertyValue(0, int.MaxValue, data.Attack);
            attributes.Add(EPropertyType.Attack, av);

            av = new PropertyValue(1, int.MaxValue, 1);
            attributes.Add(EPropertyType.Level, av);
        }
        #endregion

        #region Get
        public PropertyValue GetValue(EPropertyType type)
        {
            this.attributes.TryGetValue(type, out PropertyValue value);
            return value;
        }

        public PropertyValue Hp { get { return GetValue(EPropertyType.Hp); } }

        public PropertyValue Attack { get { return GetValue(EPropertyType.Attack); } }

        public PropertyValue Level { get { return GetValue(EPropertyType.Level); } }
        #endregion
    }
}
