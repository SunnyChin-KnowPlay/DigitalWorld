using System.Collections.Generic;
using System.Xml;
using UnityEditor;
using UnityEngine;
using System;

namespace DigitalWorld.Logic.Editor
{
    internal class NodeField : NodeBase
    {
        #region Params
        /*
         *  <field name="unitKey" baseClassT="" classT="int" desc="单位key">0</field>
         */
        public string baseClassT;
        public string classT;
        public string desc;

        private static string[] baseClassArray = null;
        private static string[] valueTypeClassArray = null;
        private static List<Type> enumTypes = null;
        private static string[] enumTypeClassArray = null;
        private static string[] enumTypeShowingClassArray = null;
        #endregion

        #region Save & Load
        public override void Decode(XmlElement node)
        {
            if (node.HasAttribute("name"))
                name = node.GetAttribute("name");
            if (node.HasAttribute("baseClassT"))
                baseClassT = node.GetAttribute("baseClassT");
            if (node.HasAttribute("classT"))
                classT = node.GetAttribute("classT");
            if (node.HasAttribute("desc"))
                desc = node.GetAttribute("desc");
        }

        public override void Encode(XmlElement node)
        {
            node.SetAttribute("name", name);
            node.SetAttribute("baseClassT", baseClassT);
            node.SetAttribute("classT", classT);
            node.SetAttribute("desc", desc);
        }
        #endregion

        #region GUI
        public override void OnGUIBody()
        {
            base.OnGUIBody();

            name = EditorGUILayout.TextField("name", name);
            baseClassT = EditorGUILayout.TextField("baseClassT", baseClassT);
            classT = EditorGUILayout.TextField("classT", classT);
            desc = EditorGUILayout.TextField("desc", desc);
        }
        #endregion

        #region Common
        public override string GetTitle()
        {
            return "字段";
        }

        public override object Clone()
        {
            NodeField v = new NodeField();
            this.CloneTo(v);
            return v;
        }

        public override T CloneTo<T>(T obj)
        {
            if (base.CloneTo(obj) is NodeField v)
            {
                v.baseClassT = this.baseClassT;
                v.classT = this.classT;
                v.desc = this.desc;
            }

            return obj;
        }

        public static string BaseClassValueType = "ValueType";
        public static string BaseClassEnum = "Enum";

        private static List<Type> EnumTypes
        {
            get
            {
                if (null == enumTypes)
                {
                    enumTypes = Logic.Utility.GetPublicEnumTypes(enumTypes);
                }
                return enumTypes;
            }
        }

        public static string[] EnumTypeShowingArray
        {
            get
            {
                if (null == enumTypeShowingClassArray)
                {
                    string namespaceName = Logic.Utility.ProjectNamespace + ".";
                    List<Type> enumTypes = EnumTypes;

                    enumTypeShowingClassArray = new string[enumTypes.Count];
                    for (int i = 0; i < enumTypes.Count; ++i)
                    {
                        string typeText = enumTypes[i].ToString();
                        if (!string.IsNullOrEmpty(typeText))
                        {
                            typeText = typeText.Replace(namespaceName, "");
                            typeText = typeText.Replace('.', '/');

                            enumTypeShowingClassArray[i] = typeText;
                        }
                    }
                }
                return enumTypeShowingClassArray;
            }
        }

        public static string[] EnumTypeArray
        {
            get
            {
                if (null == enumTypeClassArray)
                {
                    string namespaceName = Logic.Utility.ProjectNamespace + ".";
                    List<Type> enumTypes = EnumTypes;

                    enumTypeClassArray = new string[enumTypes.Count];
                    for (int i = 0; i < enumTypes.Count; ++i)
                    {
                        string typeText = enumTypes[i].ToString();
                        if (!string.IsNullOrEmpty(typeText))
                        {
                            typeText = typeText.Replace(namespaceName, "");

                            enumTypeClassArray[i] = typeText;
                        }
                    }
                }

                return enumTypeClassArray;
            }
        }

        public static string FindEnumTypeText(int index)
        {
            if (index < 0 || index >= EnumTypeArray.Length)
                return EnumTypeArray[0];

            return EnumTypeArray[index];
        }

        public static int FindEnumTypeIndex(string v)
        {
            int index = 0;
            for (int i = 0; i < EnumTypeArray.Length; ++i)
            {
                if (EnumTypeArray[i] == v)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

        public static string[] BaseClassArray
        {
            get
            {
                if (null == baseClassArray)
                {
                    baseClassArray = new string[2]
                    {
                    BaseClassValueType,
                    BaseClassEnum,
                    };
                }

                return baseClassArray;
            }
        }

        public static string[] ValueTypeClassArray
        {
            get
            {
                if (null == valueTypeClassArray)
                {
                    valueTypeClassArray = new string[12]
                    {
                        "int",
                        "float",
                        "string",
                        "bool",
                        "Color",
                        "FixVector3",
                        "int16",
                        "uint16",
                        "int64",
                        "uint64",
                        "byte",
                        "sbyte"
                    };
                }
                return valueTypeClassArray;
            }
        }

        public static string FindValueTypeText(int index)
        {
            if (index < 0 || index >= ValueTypeClassArray.Length)
                return ValueTypeClassArray[0];

            return ValueTypeClassArray[index];
        }

        public static int FindValueTypeIndex(string v)
        {
            int index = 0;
            for (int i = 0; i < ValueTypeClassArray.Length; ++i)
            {
                if (ValueTypeClassArray[i] == v)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

        public static string[] GetBaseClassArray()
        {
            return BaseClassArray;
        }

        public static string FindBaseClassText(int index)
        {
            if (index < 0 || index >= BaseClassArray.Length)
                return BaseClassArray[0];

            return BaseClassArray[index];
        }

        public static int FindBaseClassIndex(string v)
        {
            int index = 0;
            for (int i = 0; i < BaseClassArray.Length; ++i)
            {
                if (BaseClassArray[i] == v)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }
        #endregion
    }
}
