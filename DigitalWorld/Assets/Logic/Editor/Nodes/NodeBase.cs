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
        protected bool editing;
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
        #endregion
    }
}
