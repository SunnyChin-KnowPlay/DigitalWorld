using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using System.Xml;
using DigitalWorld.Logic.Editor;
using DigitalWorld.Logic.Properties;

namespace DigitalWorld.Logic.Editor
{
    internal class NodeAction : NodeItem
    {
        #region Params
        protected readonly List<NodePropertyItem> items = new List<NodePropertyItem>();
        protected readonly ReorderableList itemList;
        #endregion

        #region Common
        internal NodeAction()
        {
            itemList = new ReorderableList(items, typeof(NodePropertyItem))
            {
                drawElementCallback = OnDrawPropertyItemElement,
                onAddCallback = OnAddProperty,
                onRemoveCallback = OnRemoveProperty,
                drawHeaderCallback = OnDrawPropertyItemHead,
                draggable = true,
            };
        }

        public override string GetTitle()
        {
            return "Action";
        }

        public override object Clone()
        {
            NodeAction v = new NodeAction();
            this.CloneTo(v);
            return v;
        }
        #endregion

        #region GUI
        public override void OnGUIBody()
        {
            base.OnGUIBody();

            itemList.displayAdd = PropertyHelper.RootTypeDisplayArray.Length > 0;
            itemList.DoLayoutList();
        }

        protected virtual void OnDrawPropertyItemElement(Rect rect, int index, bool selected, bool focused)
        {
            float width = rect.width;
            if (index < items.Count)
            {
                NodePropertyItem item = items[index];

                rect.y += 2;
                rect.height = EditorGUIUtility.singleLineHeight;

                rect.xMax = rect.xMin + width * 0.2f;
                item.Name = EditorGUI.TextField(rect, item.Name);

                rect.xMin = rect.xMax + 4;
                rect.xMax = rect.xMin + width * 0.4f;
                item.ValueType = PropertyHelper.FindRootType(EditorGUI.Popup(rect, PropertyHelper.FindRootTypeIndex(item.ValueType), PropertyHelper.RootTypeDisplayArray));

                rect.xMin = rect.xMax + 4;
                rect.xMax = width;

                item.desc = EditorGUI.TextField(rect, item.desc);
            }
            else
            {
                items.RemoveAt(index);
            }
        }

        private void OnDrawPropertyItemHead(Rect rect)
        {
            EditorGUI.LabelField(rect, "Properties");
        }

        protected virtual void OnAddProperty(ReorderableList list)
        {
            list.list.Add(new NodePropertyItem());
        }

        protected virtual void OnRemoveProperty(ReorderableList list)
        {
            list.list.RemoveAt(itemList.index);
        }

        #endregion

        #region Clone
        public override T CloneTo<T>(T obj)
        {
            if (base.CloneTo(obj) is NodeAction v)
            {
                v.desc = this.desc;

                v.items.Clear();
                for (int i = 0; i < items.Count; ++i)
                {
                    v.items.Add(items[i].Clone() as NodePropertyItem);
                }
            }

            return obj;
        }
        #endregion

        #region Save & Load
        public override void Decode(XmlElement node)
        {
            base.Decode(node);

            XmlElement propertyElement = node["properties"];
            if (null != propertyElement)
            {
                this.items.Clear();

                for (int i = 0; i < propertyElement.ChildNodes.Count; ++i)
                {
                    XmlElement ele = propertyElement.ChildNodes[i] as XmlElement;
                    NodePropertyItem fn = new NodePropertyItem();
                    fn.Decode(ele);
                    this.items.Add(fn);
                }
            }
        }

        public override void Encode(XmlElement node)
        {
            base.Encode(node);

            XmlElement propertyElement = node.OwnerDocument.CreateElement("properties");
            node.AppendChild(propertyElement);

            for (int i = 0; i < items.Count; ++i)
            {
                XmlElement propertyEle = node.OwnerDocument.CreateElement("property");
                this.items[i].Encode(propertyEle);
                propertyElement.AppendChild(propertyEle);
            }
        }
        #endregion
    }
}
