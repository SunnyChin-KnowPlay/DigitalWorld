using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace DigitalWorld.Logic
{
    public abstract partial class Effect : NodeState
    {
        #region Params
        public Behaviour Behaviour
        {
            get
            {
                return this._parent as Behaviour;
            }
        }

        public ECheckLogic CheckLogic
        {
            get => _checkLogic;
            set => _checkLogic = value;
        }
        protected ECheckLogic _checkLogic;

        public Dictionary<string, bool> Requirements => _requirements;
        protected Dictionary<string, bool> _requirements = new Dictionary<string, bool>();
        #endregion

        #region Pool
        public override void OnAllocate()
        {
            base.OnAllocate();

            _checkLogic = ECheckLogic.And;
        }

        public override void OnRecycle()
        {
            base.OnRecycle();

            _requirements.Clear();
        }
        #endregion

        #region Logic
        public virtual bool GetRequirement()
        {
            Behaviour behaviour = this.Behaviour;
            if (null == behaviour)
                return CheckRequirement();

            return behaviour.GetRequirement(this.Index);
        }

        public override bool CheckRequirement()
        {
            Behaviour behaviour = this.Behaviour;
            if (this._requirements.Count < 1 || null == behaviour)
                return base.CheckRequirement();

            bool result = true;
            switch (_checkLogic)
            {
                case ECheckLogic.And:
                {
                    foreach (KeyValuePair<string, bool> item in this._requirements)
                    {
                        bool ret = this.Behaviour.GetRequirement(item.Key);
                        if (ret != item.Value)
                        {
                            result = false;
                            break;
                        }
                    }
                    break;
                }
                case ECheckLogic.Or:
                {
                    result = false;
                    foreach (KeyValuePair<string, bool> item in this._requirements)
                    {
                        bool ret = this.Behaviour.GetRequirement(item.Key);
                        if (ret == item.Value)
                        {
                            result = true;
                            break;
                        }
                    }
                    break;
                }
            }

            return result;
        }
        #endregion

        #region Serialization
        protected override void OnDecode()
        {
            base.OnDecode();
        }

        protected override void OnDecode(XmlElement element)
        {
            base.OnDecode(element);

            XmlElement requirementsEle = element["_requirements"];
            if (null != requirementsEle)
            {
                foreach (object node in requirementsEle.ChildNodes)
                {
                    XmlElement requirementEle = node as XmlElement;
                    string key = requirementEle.GetAttribute("key");
                    bool.TryParse("value", out bool value);
                    this._requirements.Add(key, value);
                }
            }
        }

        protected override void OnEncode()
        {
            base.OnEncode();
        }

        protected override void OnEncode(XmlElement element)
        {
            base.OnEncode(element);

            XmlDocument doc = element.OwnerDocument;

            XmlElement requirementsEle = doc.CreateElement("_requirements");
            requirementsEle.SetAttribute("_checkLogic", this._checkLogic.ToString());
            foreach (KeyValuePair<string, bool> item in this._requirements)
            {
                XmlElement requirementEle = doc.CreateElement("requirement");
                requirementEle.SetAttribute("key", item.Key);
                requirementEle.SetAttribute("value", item.Value.ToString());
                requirementsEle.AppendChild(requirementEle);
            }
            element.AppendChild(requirementsEle);
        }
        #endregion
    }
}
