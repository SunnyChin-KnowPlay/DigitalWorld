using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalWorld.Logic
{
    /// <summary>
    /// 行为基类
    /// </summary>
    public partial class Behaviour : NodeState
    {
        #region Params
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
            }
        }

       
        #endregion
    }
}
