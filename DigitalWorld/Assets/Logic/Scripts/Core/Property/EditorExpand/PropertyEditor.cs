using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DigitalWorld.Logic
{
    public partial class PropertyBase
    {
#if UNITY_EDITOR
        #region GUI
        public override void OnGUIField(ref Rect rect, Rect parentRect)
        {
            base.OnGUIField(ref rect, parentRect);

            Type type = this.GetType();
            // 这里获取到了所有的字段 把他们一个一个的显示出来
            var fields = type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);

           
            for (int i = 0; i < fields.Length; ++i)
            {
                FieldInfo field = fields[i];
                if (field.IsFamily)
                    continue;


                this.OnGUIEditingField(field);


            }
            
        }
        #endregion
#endif
    }
}
