using Dream.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEditor;
using UnityEngine;

namespace DigitalWorld.Logic.Editor
{
    internal class NodeController : Singleton<NodeController>
    {
        #region Params
        public Dictionary<EItemType, Dictionary<int, NodeItem>> items = new Dictionary<EItemType, Dictionary<int, NodeItem>>();
        private Dictionary<EItemType, bool> itemsEditings = new Dictionary<EItemType, bool>();

        public bool editing = false;
        public bool Editing
        {
            get { return editing; }
        }

        private bool dirty = false;
        public bool IsDirty
        {
            get { return dirty; }
        }
        #endregion

        #region Common
        public void SetDirty()
        {
            dirty = true;
        }

        public bool GetItemsEditing(EItemType type)
        {
            if (!itemsEditings.ContainsKey(type))
            {
                itemsEditings.Add(type, false);
            }

            return itemsEditings[type];
        }

        public void SetItemsEditing(EItemType type, bool editing)
        {
            if (this.itemsEditings.ContainsKey(type))
            {
                itemsEditings[type] = editing;
            }
            else
            {
                itemsEditings.Add(type, editing);
            }

        }

        public void StartEdit()
        {
            if (false == editing)
            {
                LoadAllItems();
                editing = true;
            }
        }

        public void StopEdit()
        {
            if (editing)
            {
                //this.items.Clear();
                editing = false;
                dirty = false;
            }
        }

        public void Save()
        {
            if (editing)
            {
                SaveAllItems();

                if (Utility.AutoRefresh)
                {
                    AssetDatabase.Refresh();
                }

                this.StopEdit();
            }
        }

        public void LoadAllItems()
        {
            items.Clear();
            foreach (EItemType type in Enum.GetValues(typeof(EItemType)))
            {
                string name = GetItemFileName(type);
                if (!string.IsNullOrEmpty(name))
                {
                    var text = Utility.LoadTemplateConfig(name);
                    LoadItems(type, this.GetItems(type), text);
                }
            }
        }

        public void SaveAllItems()
        {
            foreach (var kvp in items)
            {
                this.WriteItems(kvp.Key, this.GetItems(kvp.Key));
            }
        }

        //public void SortItems(ItemTypeEnum type)
        //{
        //    var list = this.GetItems(type);
        //    if (null != list)
        //    {

        //        list.Sort(OnSortItem);
        //    }
        //}

        public NodeItem CreateItem(EItemType type, bool isListen = true)
        {
            NodeItem node = null;
            switch (type)
            {
                case EItemType.Action:
                    node = new NodeAction();
                    break;
                case EItemType.Condition:
                    node = new NodeCondition();
                    break;
                case EItemType.Event:
                    node = new NodeEvent();
                    break;
            }

            if (null != node && isListen)
            {
                node.OnDirtyChanged += OnDirtyChanged;
            }
            return node;
        }

        private void LoadItems(EItemType type, Dictionary<int, NodeItem> list, TextAsset asset)
        {
            list.Clear();

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(asset.text);


            XmlElement root = doc["data"];

            foreach (var node in root.ChildNodes)
            {
                XmlElement element = (XmlElement)node;

                NodeItem item = this.CreateItem(type);
                item.Decode(element);
                list.Add(item.Id, item);
            }

        }

        public void WriteItems(EItemType type, Dictionary<int, NodeItem> dict)
        {
            XmlDocument doc = new XmlDocument();

            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(dec);

            XmlElement root = doc.CreateElement("data");

            foreach (var kvp in dict)
            {
                string eleName = GetXmlElementName(type);
                XmlElement ele = doc.CreateElement(eleName);

                kvp.Value.Encode(ele);
                root.AppendChild(ele);
            }

            doc.AppendChild(root);

            string fullPath = GetFilePath(type);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
            doc.Save(fullPath);
        }

        public void AddItem(EItemType type, NodeItem item)
        {
            var items = this.GetItems(type);
            items.Add(item.Id, item);
        }

        public void RemoveItem(EItemType type, NodeItem item)
        {
            var items = this.GetItems(type);
            items.Remove(item.Id);
        }

        private string GetFilePath(EItemType item)
        {
            string filePath = Path.Combine(Utility.TemplateConfigsPath, this.GetItemFileName(item));
            filePath += ".xml";
            string fullPath = Path.Combine(Utility.GetProjectDataPath(), filePath);

            return fullPath;
        }

        private string GetItemFileName(EItemType item)
        {
            switch (item)
            {
                case EItemType.Action:
                    return "Actions";
                case EItemType.Condition:
                    return "Conditions";
                case EItemType.Event:
                    return "Events";
                default:
                    return null;
            }
        }

        private string GetXmlElementName(EItemType item)
        {
            switch (item)
            {
                case EItemType.Action:
                    return "action";
                case EItemType.Condition:
                    return "condition";
                case EItemType.Event:
                    return "event";
                default:
                    return null;
            }
        }

        public Dictionary<int, NodeItem> GetItems(EItemType item)
        {
            Dictionary<int, NodeItem> dict = null;
            this.items.TryGetValue(item, out dict);
            if (null == dict)
            {
                dict = new Dictionary<int, NodeItem>();
                this.items.Add(item, dict);
            }
            return dict;
        }

        private bool CheckIdCanUse(Dictionary<int, NodeItem> dict, int id)
        {
            return !dict.ContainsKey(id);
        }

        public int GetNewId(EItemType type)
        {
            var list = this.GetItems(type);
            if (null == list || list.Count < 1)
                return 1;

            int c = 1;

            for (c = 1; c < int.MaxValue; ++c)
            {
                bool ret = CheckIdCanUse(list, c);
                if (ret)
                {
                    return c;
                }
            }

            return -1;
        }

        public bool CheckItemExits(EItemType type, int id, ref NodeItem node)
        {
            if (id <= 0)
                return true;

            var list = this.GetItems(type);

            if (null == list || list.Count < 1)
                return false;

            foreach (var kvp in list)
            {
                if (kvp.Value.Id == id)
                {
                    node = kvp.Value;
                    return true;
                }
            }

            return false;
        }

        public bool CheckItemExits(EItemType type, string name, ref NodeItem node)
        {
            var list = this.GetItems(type);

            if (null == list || list.Count < 1)
                return false;

            foreach (var kvp in list)
            {
                if (kvp.Value.Name == name)
                {
                    node = kvp.Value;
                    return true;
                }
            }

            return false;
        }

       
        public static string GetTitleWithType(EItemType item)
        {
            switch (item)
            {
                case EItemType.Action:
                    return "行动";
                case EItemType.Condition:
                    return "条件";
                case EItemType.Event:
                    return "事件";
                default:
                    return null;
            }
        }
        #endregion

        #region Listen
        private void OnDirtyChanged(bool dirty)
        {
            if (dirty)
                this.dirty = true;
        }

        
        #endregion

       
    }
}
