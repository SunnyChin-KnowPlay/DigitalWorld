using Assets.Logic.Editor.Templates;
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
        public Dictionary<EItemType, List<NodeItem>> items = new Dictionary<EItemType, List<NodeItem>>();
        private readonly Dictionary<EItemType, bool> itemsEditings = new Dictionary<EItemType, bool>();

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
        public void GenerateNodesCode()
        {
            ClearAllCodeFiles();

            var text = DigitalWorld.Logic.Utility.LoadTemplateConfig("Events");
            XmlDocument eventDoc = new XmlDocument();
            eventDoc.LoadXml(text.text);

            text = DigitalWorld.Logic.Utility.LoadTemplateConfig("Actions");
            XmlDocument actionDoc = new XmlDocument();
            actionDoc.LoadXml(text.text);

            text = DigitalWorld.Logic.Utility.LoadTemplateConfig("Properties");
            XmlDocument propertyDoc = new XmlDocument();
            propertyDoc.LoadXml(text.text);

            GenerateEvent(eventDoc);
            GenerateProperties(propertyDoc);
            GenerateActions(actionDoc);

            GenerateEnums(eventDoc, actionDoc);
            GenerateHelper(eventDoc, actionDoc, propertyDoc);

            AssetDatabase.Refresh();
        }

        private static string GetWriteXmlText(string baseType, string type, string name)
        {
            System.Type bt = DigitalWorld.Logic.Utility.GetBaseType(baseType);
            if (bt == typeof(Enum))
            {
                return string.Format("{0}.ToString()", name);
            }
            else if (bt == typeof(ValueType))
            {
                System.Type tp = DigitalWorld.Logic.Utility.GetValueType(type);
                if (tp == typeof(int))
                {
                    return string.Format("{0}.ToString()", name);
                }
                else if (tp == typeof(uint))
                {
                    return string.Format("{0}.ToString()", name);
                }
                else if (tp == typeof(string))
                {
                    return name;
                }
                else if (tp == typeof(float))
                {
                    return string.Format("{0}.ToString()", name);
                }
                else if (tp == typeof(bool))
                {
                    return string.Format("{0}.ToString()", name);
                }
                else if (tp == typeof(Color))
                {
                    return String.Format("{0}.ToString()", name);
                }
            }

            return string.Empty;
        }

        private static void GenerateEnums(XmlDocument ev, XmlDocument act)
        {
            DefinedTemplate tmp = new DefinedTemplate
            {
                Session = new Dictionary<string, object>()
            };

            List<string> names = new List<string>();
            List<string> values = new List<string>();
            List<string> descs = new List<string>();
            XmlElement root = ev["data"];

            XmlElement e;
            foreach (var node in root.ChildNodes)
            {
                e = (XmlElement)node;
                names.Add(e.GetAttribute("name"));
                values.Add(e.GetAttribute("id"));
                descs.Add(e.GetAttribute("desc"));
            }
            tmp.Session["eventNames"] = names.ToArray();
            tmp.Session["eventValues"] = values.ToArray();
            tmp.Session["eventDescs"] = descs.ToArray();

            names.Clear();
            values.Clear();
            descs.Clear();

            root = act["data"];
            foreach (var node in root.ChildNodes)
            {
                e = (XmlElement)node;
                names.Add(e.GetAttribute("name"));
                values.Add(e.GetAttribute("id"));
                descs.Add(e.GetAttribute("desc"));
            }
            tmp.Session["actionNames"] = names.ToArray();
            tmp.Session["actionValues"] = values.ToArray();
            tmp.Session["actionDescs"] = descs.ToArray();


            tmp.Initialize();
            string data = tmp.TransformText();

            string fileName = "Defined.cs";
            string targetPath = Path.Combine(DigitalWorld.Logic.Utility.CodesPath, fileName);
            DigitalWorld.Logic.Utility.SaveDataToFile(data, targetPath);
        }

        private static void GenerateHelper(XmlDocument ev, XmlDocument act, XmlDocument property)
        {
            LogicHelperTemplate tmp = null;
            tmp = new LogicHelperTemplate();
            tmp.Session = new Dictionary<string, object>();

            string name = "LogicHelper";
            tmp.Session["className"] = name;

            List<string> names = new List<string>();
            List<string> ids = new List<string>();

            XmlElement e = null;

            XmlElement root = ev["data"];
            foreach (var node in root.ChildNodes)
            {
                e = (XmlElement)node;
                names.Add(e.GetAttribute("name"));
                ids.Add(e.GetAttribute("id"));

            }
            tmp.Session["eventNames"] = names.ToArray();
            tmp.Session["eventIds"] = ids.ToArray();

            names.Clear();
            ids.Clear();

            root = act["data"];
            foreach (var node in root.ChildNodes)
            {
                e = (XmlElement)node;
                names.Add(e.GetAttribute("name"));
                ids.Add(e.GetAttribute("id"));

            }
            tmp.Session["actionNames"] = names.ToArray();
            tmp.Session["actionEnums"] = ids.ToArray();

            if (null != property)
            {
                names.Clear();
                ids.Clear();

                root = property["data"];
                foreach (var node in root.ChildNodes)
                {
                    e = (XmlElement)node;
                    names.Add(e.GetAttribute("name"));
                    ids.Add(e.GetAttribute("id"));

                }
                tmp.Session["propertyNames"] = names.ToArray();
                tmp.Session["propertyEnums"] = ids.ToArray();
            }

            tmp.Initialize();
            string data = tmp.TransformText();

            string fileName = string.Format("{0}.cs", name, ".cs");
            string targetPath = System.IO.Path.Combine(DigitalWorld.Logic.Utility.CodesPath, fileName);
            DigitalWorld.Logic.Utility.SaveDataToFile(data, targetPath);
        }

        private static void GenerateEvent(XmlDocument xmlDocument)
        {
            XmlElement root = xmlDocument["data"];

            string fileName = null;

            EventTemplate tmp = null;
            foreach (var node in root.ChildNodes)
            {
                XmlElement element = (XmlElement)node;
                tmp = new EventTemplate();
                tmp.Session = new Dictionary<string, object>();

                tmp.Session["eventName"] = element.GetAttribute("name");

                tmp.Initialize();
                string data = tmp.TransformText();

                fileName = "Event" + element.GetAttribute("name") + ".cs";

                string targetPath = System.IO.Path.Combine(DigitalWorld.Logic.Utility.CodesPath, fileName);
                DigitalWorld.Logic.Utility.SaveDataToFile(data, targetPath);
            }
        }

        private static void GenerateProperties(XmlDocument xmlDocument)
        {
            XmlElement root = xmlDocument["data"];

            string fileName = null;
            PropertyTemplate tmp = null;

            foreach (var node in root.ChildNodes)
            {
                XmlElement element = (XmlElement)node;


                fileName = "Property" + element.GetAttribute("name") + ".cs";

                tmp = new PropertyTemplate
                {
                    Session = new Dictionary<string, object>
                    {
                        ["id"] = int.Parse(element.GetAttribute("id")),
                        ["className"] = element.GetAttribute("name"),
                        ["desc"] = element.GetAttribute("desc"),
                        ["valueType"] = element.GetAttribute("valueType"),
                        ["usingNamespaces"] = DigitalWorld.Logic.Utility.usingNamespaces,
                    }
                };


                tmp.Initialize();
                string data = tmp.TransformText();

                string targetPath = System.IO.Path.Combine(DigitalWorld.Logic.Utility.CodesPath, fileName);
                DigitalWorld.Logic.Utility.SaveDataToFile(data, targetPath);
            }
        }

        private static void GenerateActions(XmlDocument xmlDocument)
        {

            XmlElement root = xmlDocument["data"];

            List<string> names = new List<string>();

            List<string> types = new List<string>();
            List<string> descs = new List<string>();
            List<string> values = new List<string>();
            List<string> serializeFuncs = new List<string>();
            List<string> deserializeFuncs = new List<string>();
            List<string> calculateFuncs = new List<string>();

            string fileName = null;

            ActionTemplate tmp = null;

            foreach (var node in root.ChildNodes)
            {
                XmlElement element = (XmlElement)node;

                names.Clear();
                types.Clear();

                descs.Clear();
                values.Clear();
                serializeFuncs.Clear();
                deserializeFuncs.Clear();
                calculateFuncs.Clear();

                foreach (var a in element.ChildNodes)
                {
                    XmlElement attr = (XmlElement)a;
                    names.Add(attr.GetAttribute("name"));
                    types.Add(attr.GetAttribute("type"));

                    descs.Add(attr.GetAttribute("desc"));
                    values.Add(string.Format("default({0})", attr.GetAttribute("type")));
                    serializeFuncs.Add(attr.GetAttribute("baseClassT") == "Enum" ? "EncodeEnum" : "Encode");
                    deserializeFuncs.Add(attr.GetAttribute("baseClassT") == "Enum" ? "DecodeEnum" : "Decode");
                    calculateFuncs.Add(attr.GetAttribute("baseClassT") == "Enum" ? "CalculateSizeEnum" : "CalculateSize");
                }
                fileName = "Action" + element.GetAttribute("name") + ".cs";

                tmp = new ActionTemplate();
                tmp.Session = new Dictionary<string, object>();

                tmp.Session["id"] = int.Parse(element.GetAttribute("id"));
                tmp.Session["className"] = element.GetAttribute("name");
                tmp.Session["desc"] = element.GetAttribute("desc");

                tmp.Session["types"] = types.ToArray();
                tmp.Session["varNames"] = names.ToArray();
                tmp.Session["descripts"] = descs.ToArray();
                tmp.Session["defaultValues"] = values.ToArray();
                tmp.Session["usingNamespaces"] = DigitalWorld.Logic.Utility.usingNamespaces;
                tmp.Session["serializeFuncs"] = serializeFuncs.ToArray();
                tmp.Session["deserializeFuncs"] = deserializeFuncs.ToArray();
                tmp.Session["calculateFuncs"] = calculateFuncs.ToArray();

                tmp.Initialize();
                string data = tmp.TransformText();

                string targetPath = System.IO.Path.Combine(DigitalWorld.Logic.Utility.CodesPath, fileName);
                DigitalWorld.Logic.Utility.SaveDataToFile(data, targetPath);
            }
        }

        private static void ClearAllCodeFiles()
        {
            DigitalWorld.Logic.Utility.DeleteAllFiles(DigitalWorld.Logic.Utility.CodesPath);
        }

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
                editing = false;
                dirty = false;
            }
        }

        public void Save()
        {
            if (editing)
            {
                SaveAllItems();

                AssetDatabase.Refresh();

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
                    var text = DigitalWorld.Logic.Utility.LoadTemplateConfig(name);
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

        private int OnSortItem(NodeItem l, NodeItem r)
        {
            return l.Name.CompareTo(r.Name);
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
                case EItemType.Property:
                    node = new NodeProperty();
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

        private void LoadItems(EItemType type, List<NodeItem> list, TextAsset asset)
        {
            list.Clear();

            if (null != asset)
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(asset.text);

                XmlElement root = doc["data"];

                foreach (var node in root.ChildNodes)
                {
                    XmlElement element = (XmlElement)node;

                    NodeItem item = this.CreateItem(type);
                    item.Decode(element);
                    list.Add(item);
                }
            }
        }

        public void WriteItems(EItemType type, List<NodeItem> list)
        {
            XmlDocument doc = new XmlDocument();

            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(dec);

            XmlElement root = doc.CreateElement("data");

            foreach (var node in list)
            {
                string eleName = GetXmlElementName(type);
                XmlElement ele = doc.CreateElement(eleName);

                node.Encode(ele);
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
            items.Add(item);

            items.Sort(OnSortItem);
        }

        public void RemoveItem(EItemType type, NodeItem item)
        {
            List<NodeItem> items = this.GetItems(type);
            for (int i = 0; i < items.Count; ++i)
            {
                if (items[i].Id == item.Id)
                {
                    items.RemoveAt(i);
                    break;
                }
            }
        }

        private string GetFilePath(EItemType item)
        {
            string filePath = Path.Combine(DigitalWorld.Logic.Utility.TemplateConfigsPath, this.GetItemFileName(item));
            filePath += ".xml";
            string fullPath = Path.Combine(DigitalWorld.Logic.Utility.GetProjectDataPath(), filePath);

            return fullPath;
        }

        private string GetItemFileName(EItemType item)
        {
            switch (item)
            {
                case EItemType.Action:
                    return "Actions";
                case EItemType.Property:
                    return "Properties";
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
                case EItemType.Property:
                    return "property";
                case EItemType.Event:
                    return "event";
                default:
                    return null;
            }
        }

        public List<NodeItem> GetItems(EItemType item)
        {
            List<NodeItem> list = null;
            this.items.TryGetValue(item, out list);
            if (null == list)
            {
                list = new List<NodeItem>();
                this.items.Add(item, list);
            }
            return list;
        }

        private bool CheckIdCanUse(List<NodeItem> list, int id)
        {
            foreach (NodeItem item in list)
            {
                if (item.Id == id)
                    return false;
            }
            return true;
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

            foreach (NodeItem item in list)
            {
                if (item.Id == id)
                {
                    node = item;
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

            foreach (NodeItem item in list)
            {
                if (item.Name == name)
                {
                    node = item;
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
                    return "Action";

                case EItemType.Property:
                    return "Property";

                case EItemType.Event:
                    return "Event";

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
