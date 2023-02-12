using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;

namespace DigitalWorld.Logic.Properties
{
    public static partial class PropertyHelper
    {
        private static Dictionary<Type, Type[]> properties = null;

        public static Dictionary<Type, Type[]> Properties
        {
            get
            {
                if (null == properties)
                {
                    
                }

                return properties;
            }
        }

        public static Type[] GetPropertyTypes(Type valueBaseType)
        {
            Properties.TryGetValue(valueBaseType, out Type[] result);
            return result;
        }
    }

}
