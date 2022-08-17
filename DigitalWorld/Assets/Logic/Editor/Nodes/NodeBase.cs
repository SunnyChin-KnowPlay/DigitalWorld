using System;
using System.Collections.Generic;
using System.Xml;

namespace DigitalWorld.Logic.Editor
{
    internal abstract class NodeBase : ISerialization
    {
        #region Event
        public delegate void OnDirtyChangedHandle(bool dirty);
        public event OnDirtyChangedHandle OnDirtyChanged;
        #endregion

        #region Params
        public virtual bool IsDirty
        {
            get
            { return dirty; }
        }
        protected bool dirty = false;

        protected int id;

        public virtual int Id
        {
            get { return id; }
            set
            {
                if (id == value)
                    return;

                id = value;
                SetDirty();
            }
        }

        /// <summary>
        /// Ãû×Ö
        /// </summary>
        public string Name
        {
            get { return name; }
            set
            {
                if (name == value)
                    return;
                name = value;
                SetDirty();
            }
        }
        protected string name;

        public bool Editing
        {
            get
            {
                return editing;
            }
            set
            {
                editing = value;
            }
        }
        protected bool editing = true;

        protected static System.Type[] typeArray = null;
        protected static string[] typeNamesArray = null;
        protected static string[] typeDisplayArray = null;

        #endregion

        #region Dirty
        public virtual void SetDirty(bool isNeedNotify = true)
        {
            dirty = true;
            if (isNeedNotify && null != OnDirtyChanged)
            {
                OnDirtyChanged.Invoke(dirty);
            }
        }

        public virtual void ResetDirty()
        {
            dirty = false;
            if (null != OnDirtyChanged)
            {
                OnDirtyChanged.Invoke(dirty);
            }
        }
        #endregion

        #region Save & Load
        public virtual void Encode(XmlElement node)
        {
            node.SetAttribute("id", this.id.ToString());
            node.SetAttribute("name", this.name);
        }

        public virtual void Decode(XmlElement node)
        {
            if (node.HasAttribute("id"))
                this.id = int.Parse(node.GetAttribute("id"));
            if (node.HasAttribute("name"))
                this.name = node.GetAttribute("name");
        }
        #endregion

        #region GUI
        public virtual void OnGUITitle()
        {

        }

        public virtual void OnGUIBody()
        {

        }
        #endregion

        #region Common
        public abstract string GetTitle();

        public virtual void Save()
        {

        }

        public abstract object Clone();

        public virtual T CloneTo<T>(T obj) where T : NodeBase
        {
            obj.id = this.id;
            obj.name = this.name;
            obj.OnDirtyChanged = this.OnDirtyChanged;

            return obj;
        }

        protected static List<Type> EnumTypes
        {
            get
            {
                List<Type> list = new List<Type>();

                list = Logic.Utility.GetPublicEnumTypes(list);

                return list;
            }
        }

        public static Type[] TypeArray
        {
            get
            {
                if (null == typeArray)
                {
                    List<Type> typeList = new List<Type>();
                    typeList.AddRange(Enum.GetValues(typeof(ETypeCode)) as Type[]);

                    List<Type> enumTypes = EnumTypes;
                    foreach (Type type in enumTypes)
                    {
                        typeList.Add(type);
                    }

                    typeArray = typeList.ToArray();
                }

                return typeArray;
            }
        }

        public static string[] TypeNamesArray
        {
            get
            {
                if (null == typeNamesArray)
                {
                    List<string> classList = new List<string>();
                    classList.AddRange(Enum.GetNames(typeof(ETypeCode)));

                    List<Type> enumTypes = EnumTypes;
                    foreach (Type type in enumTypes)
                    {
                        string v = type.ToString();
                        classList.Add(v);
                    }

                    typeNamesArray = classList.ToArray();
                }
                return typeNamesArray;
            }
        }

        public static string[] TypeDisplayArray
        {
            get
            {
                if (null == typeDisplayArray)
                {
                    List<string> list = new List<string>();
                    string[] names = Enum.GetNames(typeof(ETypeCode));
                    for (int i = 0; i < names.Length; ++i)
                    {
                        list.Add(names[i].Replace('.', '.'));
                    }

                    List<Type> enumTypes = EnumTypes;
                    foreach (Type type in enumTypes)
                    {
                        string v = String.Format("{0}.{1}", "Enum", type.ToString());
                        list.Add(v.Replace('.', '/'));
                    }

                    typeDisplayArray = list.ToArray();
                }
                return typeDisplayArray;
            }
        }

        public static Type FindType(int index)
        {
            if (index < 0 || index >= TypeArray.Length)
                return TypeArray[0];

            return TypeArray[index];
        }

        public static string FindTypeName(int index)
        {
            if (index < 0 || index >= TypeNamesArray.Length)
                return TypeNamesArray[0];

            return TypeNamesArray[index];
        }

        public static int FindTypeIndex(string v)
        {
            int index = 0;
            for (int i = 0; i < TypeNamesArray.Length; ++i)
            {
                if (TypeNamesArray[i] == v)
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
