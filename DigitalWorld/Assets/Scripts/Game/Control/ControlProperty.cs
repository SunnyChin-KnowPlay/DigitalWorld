using DigitalWorld.Proto.Game;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalWorld.Game
{
    public class ControlProperty : ControlLogic
    {
        private readonly Dictionary<EPropertyType, PropertyValue> attributes = new Dictionary<EPropertyType, PropertyValue>();

        #region Setup
        public override void Setup(ControlUnit unit, UnitData data)
        {
            base.Setup(unit, data);

            attributes.Clear();

            PropertyValue av;
            av = new PropertyValue(0, data.Hp, data.Hp);
            attributes.Add(EPropertyType.Hp, av);

            av = new PropertyValue(0, int.MaxValue, data.Attack);
            attributes.Add(EPropertyType.Attack, av);

            av = new PropertyValue(0, int.MaxValue, data.MoveSpeed);
            attributes.Add(EPropertyType.MoveSpeed, av);
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

        public PropertyValue MoveSpeed { get { return GetValue(EPropertyType.MoveSpeed); } }
        #endregion
    }
}
