using DigitalWorld.Proto.Game;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalWorld.Game
{
    public class ControlAttribute : ControlLogic
    {
        private Dictionary<EAttributeType, AttributeValue> attributes = new Dictionary<EAttributeType, AttributeValue>();


        #region Setup
        public override void Setup(UnitInfo info)
        {
            base.Setup(info);

            if (null == attributes)
                attributes = new Dictionary<EAttributeType, AttributeValue>();
            else
                attributes.Clear();

            AttributeValue av;
            av = new AttributeValue(0, info.hp, info.hp);
            attributes.Add(EAttributeType.Hp, av);

            av = new AttributeValue(0, int.MaxValue, info.attack);
            attributes.Add(EAttributeType.Attack, av);

            av = new AttributeValue(1, int.MaxValue, 1);
            attributes.Add(EAttributeType.Level, av);
        }
        #endregion

        #region Get
        public AttributeValue GetValue(EAttributeType type)
        {
            this.attributes.TryGetValue(type, out AttributeValue value);
            return value;
        }

        public AttributeValue Hp { get { return GetValue(EAttributeType.Hp); } }

        public AttributeValue Attack { get { return GetValue(EAttributeType.Attack); } }

        public AttributeValue Level { get { return GetValue(EAttributeType.Level); } }
        #endregion
    }
}
