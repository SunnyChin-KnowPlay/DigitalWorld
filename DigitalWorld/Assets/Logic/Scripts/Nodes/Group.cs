using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalWorld.Logic
{
    public partial class Group : BaseNode
    {
        #region Params
        /// <summary>
        /// 组名字
        /// </summary>
        private string name;
        public override string Name
        {
            get
            {
                return name;
            }
        }

        protected ECheckLogic checkLogic;
        public ECheckLogic CheckLogic
        {
            get
            {
                return checkLogic;
            }
            set
            {
                if (checkLogic != value)
                {
                    SetDirty();
                    checkLogic = value;
                }
            }
        }
        #endregion

        #region Pool
        public override void OnAllocate()
        {
            base.OnAllocate();

            this.checkLogic = ECheckLogic.And;
        }
        #endregion

        #region Relation
        public override void ResetChildrenIndex()
        {
            base.ResetChildrenIndex();
        }
        #endregion
    }
}
