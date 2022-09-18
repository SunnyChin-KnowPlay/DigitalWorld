using DigitalWorld.Defined.Game;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalWorld.Game
{
    public class ControlProperty : ControlLogic
    {
        #region Params
        private readonly Dictionary<EPropertyType, PropertyValue> properties = new Dictionary<EPropertyType, PropertyValue>();

        private const int speedStandValue = 1000;
        #endregion

        #region Setup
        public override void Setup(ControlUnit unit, UnitData data)
        {
            base.Setup(unit, data);

            properties.Clear();

            PropertyValue av;
            av = new PropertyValue(EPropertyType.Hp, 0, data.Hp, data.Hp, data.Hp);
            properties.Add(EPropertyType.Hp, av);

            av = new PropertyValue(EPropertyType.Attack, 0, int.MaxValue, data.Attack, data.Attack);
            properties.Add(EPropertyType.Attack, av);

            av = new PropertyValue(EPropertyType.MoveSpeed, 0, int.MaxValue, data.MoveSpeed, speedStandValue);
            properties.Add(EPropertyType.MoveSpeed, av);

            foreach (KeyValuePair<EPropertyType, PropertyValue> kvp in properties)
            {
                kvp.Value.OnPropertyChanged += OnPropertyChanged;
            }
        }
        #endregion

        #region Get
        public PropertyValue GetValue(EPropertyType type)
        {
            this.properties.TryGetValue(type, out PropertyValue value);
            return value;
        }

        public PropertyValue Hp { get { return GetValue(EPropertyType.Hp); } }

        public PropertyValue Attack { get { return GetValue(EPropertyType.Attack); } }

        public PropertyValue MoveSpeed { get { return GetValue(EPropertyType.MoveSpeed); } }
        #endregion

        #region Listener
        private void OnPropertyChanged(EPropertyType type, int oldValue, int newerValue, int expectChangeValue)
        {
            switch (type)
            {
                case EPropertyType.Hp:
                {
                    OnPropertyHpChanged(oldValue, newerValue, expectChangeValue);
                    break;
                }
            }
        }

        private void OnPropertyHpChanged(int oldValue, int newerValue, int expectChangeValue)
        {
            if (newerValue <= 0)
            {
                this.Unit.ApplyDead();
            }
        }
        #endregion
    }
}
