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
                names.Add(Logic.Utility.GetStandardizationEnumName(e.GetAttribute("name")));
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
                names.Add(Logic.Utility.GetStandardizationEnumName(e.GetAttribute("name")));
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
            tmp = new LogicHelperTemplate
            {
                Session = new Dictionary<string, object>()
            };

            string className = "LogicHelper";
            tmp.Session["className"] = className;

            List<string> names = new List<string>();
            List<string> ids = new List<string>();

            XmlElement e = null;

            names.Clear();
            ids.Clear();

            XmlElement root = act["data"];
            foreach (var node in root.ChildNodes)
            {
                e = (XmlElement)node;

                string name = e.GetAttribute("name");
                string fullName = Logic.Utility.CombineName(Logic.Utility.ActionName, Logic.Utility.CombineName(Logic.Utility.GetNamespaceName(name), Logic.Utility.GetSelfName(name)));

                names.Add(fullName);
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

                    string name = e.GetAttribute("name");
                    string fullName = Logic.Utility.CombineName(Logic.Utility.PropertyName, Logic.Utility.CombineName(Logic.Utility.GetNamespaceName(name), Logic.Utility.GetSelfName(name)));

                    names.Add(fullName);
                    ids.Add(e.GetAttribute("id"));

                }
                tmp.Session["propertyNames"] = names.ToArray();
                tmp.Session["propertyEnums"] = ids.ToArray();
            }

            tmp.Initialize();
            string data = tmp.TransformText();

            string fileName = string.Format("{0}.cs", className, ".cs");
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
                tmp = new EventTemplate
                {
                    Session = new Dictionary<string, object>
                    {
                        ["eventName"] = element.GetAttribute("name"),
                        ["namespaceName"] = Logic.Utility.CombineName(Logic.Utility.LogicEventNamespace, Logic.Utility.GetNamespaceName(element.GetAttribute("name"))),
                        ["desc"] = element.GetAttribute("desc"),
                    }
                };
                tmp.Initialize();
                string data = tmp.TransformText();

                fileName = System.IO.Path.Combine(Logic.Utility.EventName, element.GetAttribute("name")) + ".cs";

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

                fileName = System.IO.Path.Combine(Logic.Utility.PropertyName, element.GetAttribute("name")) + ".cs";

                tmp = new PropertyTemplate
                {
                    Session = new Dictionary<string, object>
                    {
                        ["id"] = int.Parse(element.GetAttribute("id")),
                        ["className"] = element.GetAttribute("name"),
                        ["desc"] = element.GetAttribute("desc"),
                        ["valueType"] = element.GetAttribute("valueType"),
                        ["usingNamespaces"] = DigitalWorld.Logic.Utility.usingNamespaces,
                        ["namespaceName"] = Logic.Utility.CombineName(Logic.Utility.LogicPropertyNamespace, Logic.Utility.GetNamespaceName(element.GetAttribute("name"))),
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
                    types.Add(attr.GetAttribute("typeName"));
                    descs.Add(attr.GetAttribute("desc"));
                    values.Add(string.Format("default({0})", attr.GetAttribute("typeName")));

                    System.Type baseType = Type.GetType(attr.GetAttribute("baseTypeName"));
                    serializeFuncs.Add(baseType == typeof(Enum) ? "EncodeEnum" : "Encode");
                    deserializeFuncs.Add(baseType == typeof(Enum) ? "DecodeEnum" : "Decode");
                    calculateFuncs.Add(baseType == typeof(Enum) ? "CalculateSizeEnum" : "CalculateSize");
                }
                fileName = System.IO.Path.Combine(Logic.Utility.ActionName, element.GetAttribute("name")) + ".cs";

                tmp = new ActionTemplate
                {
                    Session = new Dictionary<string, object>
                    {
                        ["id"] = int.Parse(element.GetAttribute("id")),
                        ["className"] = Logic.Utility.GetSelfName(element.GetAttribute("name")),
                        ["namespaceName"] = Logic.Utility.CombineName(Logic.Utility.LogicActionNamespace, Logic.Utility.GetNamespaceName(element.GetAttribute("name"))),
                        ["desc"] = element.GetAttribute("desc"),

                        ["types"] = types.ToArray(),
                        ["varNames"] = names.ToArray(),
                        ["descripts"] = descs.ToArray(),
                        ["defaultValues"] = values.ToArray(),
                        ["usingNamespaces"] = DigitalWorld.Logic.Utility.usingNamespaces,
                        ["serializeFuncs"] = serializeFuncs.ToArray(),
                        ["deserializeFuncs"] = deserializeFuncs.ToArray(),
                        ["calculateFuncs"] = calculateFuncs.ToArray()
                    }
                };

                tmp.Initialize();
                string data = tmp.TransformText();

                string targetPath = System.IO.Path.Combine(DigitalWorld.Logic.Utility.CodesPath, fileName);
                DigitalWorld.Logic.Utility.SaveDataToFile(data, targetPath);
            }
        }

        private static void ClearAllCodeFiles()
        {
            DigitalWorld.Logic.Utility.DeleteAllFilesAndDirectories(Logic.Utility.CodesPath);
        }

        public void SetDirty()
        {
            dirty = true;
        }

        public bool GetItemsEditing(EItemType type)
        {
            if (!itemsEditings.ContainsKey(type))
            {
                itemsEditings.Add(type, true);
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
            return l.Id.CompareTo(r.Id);
        }

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
                return 0;

            for (int c = 0; c < int.MaxValue; ++c)
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
            if (id < 0)
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
