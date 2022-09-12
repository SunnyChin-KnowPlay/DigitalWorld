using DigitalWorld.Defined.Game;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalWorld.Game
{
    public class ControlProperty : ControlLogic
    {
        #region Params
        private readonly Dictionary<EPropertyType, PropertyValue> attributes = new Dictionary<EPropertyType, PropertyValue>();

        private const int speedStandValue = 1000;
        #endregion

        #region Setup
        public override void Setup(ControlUnit unit, UnitData data)
        {
            base.Setup(unit, data);

            attributes.Clear();

            PropertyValue av;
            av = new PropertyValue(0, data.Hp, data.Hp, data.Hp);
            attributes.Add(EPropertyType.Hp, av);

            av = new PropertyValue(0, int.MaxValue, data.Attack, data.Attack);
            attributes.Add(EPropertyType.Attack, av);

            av = new PropertyValue(0, int.MaxValue, data.MoveSpeed, speedStandValue);
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
