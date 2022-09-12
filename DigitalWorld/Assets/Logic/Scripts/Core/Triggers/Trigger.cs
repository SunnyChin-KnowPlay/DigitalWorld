using System.Collections.Generic;
using System.Xml;

namespace DigitalWorld.Logic
{
    public enum ETestBBB
    {
        None = 0,
        First = 1,
    }
    /// <summary>
    /// 触发器类
    /// </summary>
    public partial class Trigger : NodeState
    {
        #region Params
        public override int Id
        {
            get => 0;
        }

        /// <summary>
        /// 节点类型
        /// </summary>
        public override ENodeType NodeType => ENodeType.Trigger;
        /// <summary>
        /// 要求词典 所有的子节点都可以向其注入结果以供查询
        /// </summary>
        protected Dictionary<int, bool> requirements = new Dictionary<int, bool>();
        #endregion

        #region Pool
        public override void OnAllocate()
        {
            base.OnAllocate();
        }

        public override void OnRecycle()
        {
            base.OnRecycle();

            this.requirements.Clear();
        }
        #endregion

        #region Logic
        protected void SetRequirement(int index, bool v)
        {
            if (this.requirements.ContainsKey(index))
            {
                this.requirements[index] = v;
            }
            else
            {
                this.requirements.Add(index, v);
            }
        }

        public bool GetRequirement(int index)
        {
            this.requirements.TryGetValue(index, out bool ret);
            return ret;
        }

        public bool GetRequirement(string key)
        {
            NodeBase node = this.FindChild(key);
            if (null != node)
                return GetRequirement(node.Index);
            return false;
        }

        /// <summary>
        /// 遍历所有子节点时，先记录其requirement，以供后续查询
        /// </summary>
        /// <param name="delta"></param>
        protected override void UpdateChildren(float delta)
        {
            for (int i = 0; i < this._children.Count; ++i)
            {
                NodeBase child = this._children[i];
                if (child.Enabled)
                {
                    this.SetRequirement(child.Index, child.CheckRequirement());
                    child.Update(delta);
                }
                else
                {
                    this.SetRequirement(child.Index, true);
                }
            }
        }
        #endregion

        #region Proto
        protected override void OnEncode(XmlElement element)
        {
            base.OnEncode(element);
        }

        protected override void OnDecode(XmlElement element)
        {
            base.OnDecode(element);
        }

        protected override void OnCalculateSize()
        {
            base.OnCalculateSize();


        }
        #endregion
    }
}
