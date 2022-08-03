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

        public List<Requirement> Requirements => _requirements;
        protected List<Requirement> _requirements = new List<Requirement>();
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

        #region Relation
        protected override void OnChanged()
        {
            base.OnChanged();


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
                    foreach (Requirement requirement in _requirements)
                    {
                        bool ret = this.Behaviour.GetRequirement(requirement.nodeName);
                        if (ret != requirement.isRequirement)
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
                    foreach (Requirement requirement in _requirements)
                    {
                        bool ret = this.Behaviour.GetRequirement(requirement.nodeName);
                        if (ret == requirement.isRequirement)
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
                if (requirementsEle.HasAttribute("_checkLogic"))
                {
                    string checkLogicStr = requirementsEle.GetAttribute("_checkLogic");
                    Enum.TryParse(checkLogicStr, true, out _checkLogic);
                }

                int index = 0;
                foreach (object node in requirementsEle.ChildNodes)
                {
                    XmlElement requirementEle = node as XmlElement;
                    string key = requirementEle.GetAttribute("key");
                    bool.TryParse("value", out bool value);
                    Requirement requirement = new Requirement()
                    {
                        index = index,
                        nodeName = key,
                        isRequirement = value,
                    };
                    this._requirements.Add(requirement);
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
            foreach (Requirement requirement in this._requirements)
            {
                XmlElement requirementEle = doc.CreateElement("requirement");
                requirementEle.SetAttribute("key", requirement.nodeName);
                requirementEle.SetAttribute("value", requirement.isRequirement.ToString());
                requirementsEle.AppendChild(requirementEle);
            }
            element.AppendChild(requirementsEle);
        }
        #endregion
    }
}
