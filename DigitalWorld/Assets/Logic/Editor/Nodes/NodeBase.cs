using System;
using System.Collections.Generic;
using System.Xml;

namespace DigitalWorld.Logic.Editor
{
    public abstract class NodeBase : ISerialization
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
        public virtual void OnGUIBody()
        {

        }
        #endregion

        #region Common
        public abstract string GetTitle();

        public abstract object Clone();

        public virtual T CloneTo<T>(T obj) where T : NodeBase
        {
            obj.id = this.id;
            obj.name = this.name;
            obj.OnDirtyChanged = this.OnDirtyChanged;

            return obj;
        }

        public static Type[] TypeArray
        {
            get
            {
                if (null == typeArray)
                {
                    List<Type> typeList = new List<Type>();
                    typeList = Logic.Utility.GetUnderlyingTypes(typeList);
                    typeList = Logic.Utility.GetPublicEnumTypes(typeList);
                    typeArray = typeList.ToArray();
                }
                return typeArray;
            }
        }

        public static string[] TypeDisplayArray
        {
            get
            {
                if (null == typeDisplayArray)
                {
                    Type[] types = TypeArray;
                    typeDisplayArray = new string[types.Length];
                    for (int i = 0; i < types.Length; ++i)
                    {
                        typeDisplayArray[i] = types[i].FullName.Replace('.', '/');
                    }
                }
                return typeDisplayArray;
            }
        }

        public static string FindTypeName(int index)
        {
            Type type = FindType(index);
            if (null == type)
                return String.Empty;

            return type.FullName;
        }

        public static Type FindType(int index)
        {
            if (index < 0 || index >= TypeArray.Length)
                return TypeArray[0];

            return TypeArray[index];
        }

        public static int FindTypeIndex(string v)
        {
            int index = 0;

            Type[] types = TypeArray;

            for (int i = 0; i < types.Length; ++i)
            {
                if (types[i].FullName == v)
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
