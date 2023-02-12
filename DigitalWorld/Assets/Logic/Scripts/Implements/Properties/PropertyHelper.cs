using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalWorld.Logic.Properties
{
    public static partial class PropertyHelper
    {
#if UNITY_EDITOR
        private static System.Type[] rootTypeArray = null;
        private static string[] rootTypeDisplayArray = null;
        public static Type[] RootTypeArray
        {
            get
            {
                if (null == rootTypeArray)
                {
                    List<Type> typeList = new List<Type>();

                    var keys = Properties.Keys;
                    foreach (var v in keys)
                    {
                        typeList.Add(v);
                    }

                    rootTypeArray = typeList.ToArray();
                }
                return rootTypeArray;
            }
        }

        public static string[] RootTypeDisplayArray
        {
            get
            {
                if (null == rootTypeDisplayArray)
                {
                    Type[] types = RootTypeArray;
                    rootTypeDisplayArray = new string[types.Length];
                    for (int i = 0; i < types.Length; ++i)
                    {
                        rootTypeDisplayArray[i] = types[i].FullName.Replace('.', '/');
                    }
                }
                return rootTypeDisplayArray;
            }
        }

        public static Type FindRootType(int index)
        {
            if (index < 0 || index >= RootTypeArray.Length)
                return RootTypeArray[0];

            return RootTypeArray[index];
        }

        public static int FindRootTypeIndex(Type v)
        {
            int index = 0;

            Type[] types = RootTypeArray;

            for (int i = 0; i < types.Length; ++i)
            {
                if (types[i] == v)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

        public static string[] GetPropertyLocalTypeNames(Type rootType)
        {
            Properties.TryGetValue(rootType, out Type[] value);
            if (null != value && value.Length > 0)
            {
                string[] list = new string[value.Length];
                for (int i = 0; i < value.Length; ++i)
                {
                    list[i] = Utility.GetDirectoryFileName(Utility.GetPropertyLocalName(value[i].FullName));
                }
                return list;
            }
            return null;
        }

        public static int GetTypeIndex(Type rootType, string typeName)
        {
            Properties.TryGetValue(rootType, out Type[] value);
            if (null != value && value.Length > 0)
            {
                for (int i = 0; i < value.Length; ++i)
                {
                    if (value[i].FullName == typeName)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        public static int GetTypeIndex(Type rootType, Type type)
        {
            Properties.TryGetValue(rootType, out Type[] value);
            if (null != value && value.Length > 0)
            {
                for (int i = 0; i < value.Length; ++i)
                {
                    if (value[i] == type)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        public static Type GetPropertyType(Type rootType, int index)
        {
            Properties.TryGetValue(rootType, out Type[] value);
            if (null != value && value.Length > 0)
            {
                if (index >= 0 && index < value.Length)
                    return value[index];
            }
            return null;
        }
#endif
    }
}
