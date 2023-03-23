using Assets.Logic.Editor.Templates;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using UnityEditor;
using UnityEngine;

namespace DigitalWorld.Logic.Editor
{
    public class NodeController
    {
        #region Params
        public Dictionary<EItemType, List<NodeItem>> items = new Dictionary<EItemType, List<NodeItem>>();
        private readonly Dictionary<EItemType, bool> itemsEditings = new Dictionary<EItemType, bool>();



        private bool dirty = false;
        public bool IsDirty
        {
            get { return dirty; }
        }
        #endregion

        public NodeController()
        {

        }

        #region Common
        public void GenerateNodesCode()
        {
            ClearAllCodeFiles();

            var text = Utility.LoadTemplateConfig("Events");
            XmlDocument eventDoc = new XmlDocument();
            eventDoc.LoadXml(text.text);

            text = Utility.LoadTemplateConfig("Actions");
            XmlDocument actionDoc = new XmlDocument();
            actionDoc.LoadXml(text.text);

            text = Utility.LoadTemplateConfig("Properties");
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
            tmp.Session["tips"] = Logic.Utility.GeneratedTips;
            tmp.Session["namespaceName"] = Utility.LogicNamespace;


            tmp.Initialize();
            string data = tmp.TransformText();

            string fileName = "Defined.cs";
            string targetPath = Path.Combine(Utility.GeneratedScriptPath, fileName);
            Utility.SaveDataToFile(data, targetPath);
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
            List<string> descs = new List<string>();

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
                descs.Add(e.GetAttribute("desc"));

            }
            tmp.Session["actionNames"] = names.ToArray();
            tmp.Session["actionEnums"] = ids.ToArray();
            tmp.Session["actionDescs"] = descs.ToArray();

            if (null != property)
            {
                names.Clear();
                ids.Clear();
                descs.Clear();

                root = property["data"];
                foreach (var node in root.ChildNodes)
                {
                    e = (XmlElement)node;

                    string name = e.GetAttribute("name");
                    string fullName = Logic.Utility.CombineName(Logic.Utility.PropertyName, Logic.Utility.CombineName(Logic.Utility.GetNamespaceName(name), Logic.Utility.GetSelfName(name)));

                    names.Add(fullName);
                    ids.Add(e.GetAttribute("id"));
                    descs.Add(e.GetAttribute("desc"));
                }
                tmp.Session["tips"] = Logic.Utility.GeneratedTips;
                tmp.Session["propertyNames"] = names.ToArray();
                tmp.Session["propertyEnums"] = ids.ToArray();
                tmp.Session["propertyDescs"] = descs.ToArray();
            }

            if (null != ev)
            {
                descs.Clear();
                ids.Clear();

                root = ev["data"];
                foreach (var node in root.ChildNodes)
                {
                    e = (XmlElement)node;

                    descs.Add(e.GetAttribute("desc"));
                    ids.Add(e.GetAttribute("id"));
                }
                tmp.Session["eventEnums"] = ids.ToArray();
                tmp.Session["eventDescs"] = descs.ToArray();
                tmp.Session["namespaceName"] = Utility.LogicNamespace;
            }

            tmp.Initialize();
            string data = tmp.TransformText();

            string fileName = string.Format("{0}.cs", className, ".cs");
            string targetPath = System.IO.Path.Combine(Logic.Utility.GeneratedScriptPath, fileName);
            Logic.Utility.SaveDataToFile(data, targetPath);
        }

        private static void GenerateEvent(XmlDocument xmlDocument)
        {
            XmlElement root = xmlDocument["data"];

            string fileName = null;

            EventTemplate tmp;
            foreach (var node in root.ChildNodes)
            {
                XmlElement element = (XmlElement)node;
                tmp = new EventTemplate
                {
                    Session = new Dictionary<string, object>
                    {
                        ["tips"] = Logic.Utility.GeneratedTips,
                        ["eventName"] = Utility.GetStandardizationEnumName(element.GetAttribute("name")),
                        ["namespaceName"] = Logic.Utility.LogicEventNamespace,
                        ["desc"] = element.GetAttribute("desc"),
                    }
                };
                tmp.Initialize();
                string data = tmp.TransformText();

                fileName = System.IO.Path.Combine(Logic.Utility.EventName, Utility.GetDirectoryFileName(element.GetAttribute("name"))) + ".cs";

                string targetPath = System.IO.Path.Combine(Logic.Utility.GeneratedScriptPath, fileName);
                Logic.Utility.SaveDataToFile(data, targetPath);
            }
        }

        private static void GenerateProperties(XmlDocument xmlDocument)
        {
            XmlElement root = xmlDocument["data"];

            List<string> names = new List<string>();
            List<string> types = new List<string>();
            List<string> descs = new List<string>();

            List<string> serializeFuncs = new List<string>();
            List<string> deserializeFuncs = new List<string>();
            List<string> calculateFuncs = new List<string>();

            Dictionary<string, List<string>> properties = new Dictionary<string, List<string>>();

            string fileName;

            foreach (var node in root.ChildNodes)
            {
                XmlElement element = (XmlElement)node;

                names.Clear();
                types.Clear();

                descs.Clear();
                serializeFuncs.Clear();
                deserializeFuncs.Clear();
                calculateFuncs.Clear();

                XmlElement fieldElement = element["fields"];
                if (null != fieldElement)
                {
                    foreach (var a in fieldElement.ChildNodes)
                    {
                        XmlElement attr = (XmlElement)a;
                        names.Add(attr.GetAttribute("name"));
                        types.Add(attr.GetAttribute("typeName"));
                        descs.Add(attr.GetAttribute("desc"));

                        System.Type baseType = Type.GetType(attr.GetAttribute("baseTypeName"));
                        serializeFuncs.Add(baseType == typeof(Enum) ? "EncodeEnum" : "Encode");
                        deserializeFuncs.Add(baseType == typeof(Enum) ? "DecodeEnum" : "Decode");
                        calculateFuncs.Add(baseType == typeof(Enum) ? "CalculateSizeEnum" : "CalculateSize");
                    }
                }

                fileName = Path.Combine(Logic.Utility.PropertyName, Logic.Utility.GetDirectoryFileName(element.GetAttribute("name"))) + ".cs";
                string className = Utility.GetSelfName(element.GetAttribute("name"));
                string valueTypeName = element.GetAttribute("valueType");

                properties.TryGetValue(valueTypeName, out List<string> list);
                if (null == list)
                {
                    list = new List<string>();
                    properties.Add(valueTypeName, list);
                }
                if (null != list)
                {
                    list.Add(element.GetAttribute("name"));
                }

                PropertyTemplate tmp = new PropertyTemplate
                {
                    Session = new Dictionary<string, object>
                    {
                        ["tips"] = Logic.Utility.GeneratedTips,
                        ["id"] = int.Parse(element.GetAttribute("id")),
                        ["className"] = className,
                        ["desc"] = element.GetAttribute("desc"),
                        ["valueType"] = valueTypeName,

                        ["types"] = types.ToArray(),
                        ["varNames"] = names.ToArray(),
                        ["descripts"] = descs.ToArray(),
                        ["usingNamespaces"] = Logic.Utility.usingNamespaces,
                        ["serializeFuncs"] = serializeFuncs.ToArray(),
                        ["deserializeFuncs"] = deserializeFuncs.ToArray(),
                        ["calculateFuncs"] = calculateFuncs.ToArray(),
                        ["namespaceName"] = Logic.Utility.CombineName(Logic.Utility.LogicPropertyNamespace, Logic.Utility.GetNamespaceName(element.GetAttribute("name"))),
                    }
                };

                tmp.Initialize();
                string data = tmp.TransformText();

                string targetPath = System.IO.Path.Combine(Logic.Utility.GeneratedScriptPath, fileName);
                Logic.Utility.SaveDataToFile(data, targetPath);

                // 这里是生成实现文件的模板 首先判断一下 实现文件是否已经存在 没有的情况下利用模板进行生成
                string implementFullPath = Path.Combine(Logic.Utility.ImplementScriptPath, fileName);
                if (!File.Exists(implementFullPath))
                {
                    PropertyImplementTemplate implementTemplate = new PropertyImplementTemplate
                    {
                        Session = new Dictionary<string, object>
                        {
                            ["className"] = className,
                            ["valueType"] = valueTypeName,
                            ["namespaceName"] = Logic.Utility.CombineName(Logic.Utility.LogicPropertyNamespace, Logic.Utility.GetNamespaceName(element.GetAttribute("name"))),
                        }
                    };

                    implementTemplate.Initialize();
                    data = implementTemplate.TransformText();

                    Logic.Utility.SaveDataToFile(data, implementFullPath);
                }
            }

            PropertyHelperTemplate helperTmp = new PropertyHelperTemplate
            {
                Session = new Dictionary<string, object>
                {
                    ["properties"] = properties,
                    ["namespaceName"] = Utility.LogicNamespace,
                }
            };

            helperTmp.Initialize();
            string helperData = helperTmp.TransformText();

            fileName = string.Format("Properties/{0}.cs", "PropertyHelper");
            string helperTargetPath = System.IO.Path.Combine(Logic.Utility.GeneratedScriptPath, fileName);
            Logic.Utility.SaveDataToFile(helperData, helperTargetPath);

        }

        private static void GenerateActions(XmlDocument xmlDocument)
        {

            XmlElement root = xmlDocument["data"];

            List<string> names = new List<string>();
            List<string> capitalNames = new List<string>();

            List<string> standardTypes = new List<string>();
            List<string> types = new List<string>();
            List<string> descs = new List<string>();
            List<string> values = new List<string>();
            List<string> propertyTypes = new List<string>();
            List<string> propertyNames = new List<string>();

            string fileName;
            ActionTemplate generatedTemplate;

            ActionImplementTemplate implementTemplate;

            foreach (var node in root.ChildNodes)
            {
                XmlElement element = (XmlElement)node;

                names.Clear();
                capitalNames.Clear();
                types.Clear();
                standardTypes.Clear();

                descs.Clear();
                values.Clear();
               
                propertyTypes.Clear();
                propertyNames.Clear();

                XmlElement fieldElement = element["fields"];
                if (null != fieldElement)
                {
                    foreach (var a in fieldElement.ChildNodes)
                    {
                        XmlElement attr = (XmlElement)a;
                        names.Add(attr.GetAttribute("name"));
                        capitalNames.Add(attr.GetAttribute("name").ToUpperFirst());
                        types.Add(attr.GetAttribute("typeName"));
                        standardTypes.Add(Utility.GetStandardizationEnumName(attr.GetAttribute("typeName")));
                        descs.Add(attr.GetAttribute("desc"));
                        values.Add(string.Format("default({0})", attr.GetAttribute("typeName")));
                    }
                }

                XmlElement propertyElement = element["properties"];
                if (null != propertyElement)
                {
                    foreach (var a in propertyElement.ChildNodes)
                    {
                        XmlElement attr = (XmlElement)a;

                        propertyNames.Add("Property_" + attr.GetAttribute("name"));
                        propertyTypes.Add(string.Format("{0}", attr.GetAttribute("valueType")));
                    }
                }


                fileName = Path.Combine(Logic.Utility.ActionName, Logic.Utility.GetDirectoryFileName(element.GetAttribute("name"))) + ".cs";

                generatedTemplate = new ActionTemplate
                {
                    Session = new Dictionary<string, object>
                    {
                        ["tips"] = Logic.Utility.GeneratedTips,
                        ["id"] = int.Parse(element.GetAttribute("id")),
                        ["className"] = Logic.Utility.GetSelfName(element.GetAttribute("name")),
                        ["namespaceName"] = Logic.Utility.CombineName(Logic.Utility.LogicActionNamespace, Logic.Utility.GetNamespaceName(element.GetAttribute("name"))),
                        ["desc"] = element.GetAttribute("desc"),

                        ["standardTypes"] = standardTypes.ToArray(),
                        ["types"] = types.ToArray(),
                        ["capitalVarNames"] = capitalNames.ToArray(),
                        ["varNames"] = names.ToArray(),
                        ["descripts"] = descs.ToArray(),
                        ["defaultValues"] = values.ToArray(),
                        ["usingNamespaces"] = Logic.Utility.usingNamespaces,
                        ["propertyNames"] = propertyNames.ToArray(),
                        ["propertyTypes"] = propertyTypes.ToArray(),
                    }
                };

                generatedTemplate.Initialize();
                string data = generatedTemplate.TransformText();

                string targetPath = System.IO.Path.Combine(Logic.Utility.GeneratedScriptPath, fileName);
                Logic.Utility.SaveDataToFile(data, targetPath);

                // 这里是生成实现文件的模板 首先判断一下 实现文件是否已经存在 没有的情况下利用模板进行生成
                string implementFullPath = Path.Combine(Logic.Utility.ImplementScriptPath, fileName);
                if (!File.Exists(implementFullPath))
                {
                    implementTemplate = new ActionImplementTemplate
                    {
                        Session = new Dictionary<string, object>
                        {
                            ["className"] = Logic.Utility.GetSelfName(element.GetAttribute("name")),
                            ["namespaceName"] = Logic.Utility.CombineName(Logic.Utility.LogicActionNamespace, Logic.Utility.GetNamespaceName(element.GetAttribute("name"))),
                        }
                    };

                    implementTemplate.Initialize();
                    data = implementTemplate.TransformText();

                    Logic.Utility.SaveDataToFile(data, implementFullPath);
                }
            }
        }

        private static void ClearAllCodeFiles()
        {
            Utility.ClearDirectory(Logic.Utility.GeneratedScriptPath);
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

        public void Save()
        {
            SaveAllItems();
            AssetDatabase.Refresh();
        }

        public void LoadAllItems()
        {
            items.Clear();
            foreach (EItemType type in Enum.GetValues(typeof(EItemType)))
            {
                string name = GetItemFileName(type);
                if (!string.IsNullOrEmpty(name))
                {
                    var text = Logic.Utility.LoadTemplateConfig(name);
                    LoadItems(type, this.GetItems(type), text);
                }
            }
        }

        public void ClearItems()
        {
            items.Clear();
        }

        public void SaveAllItems()
        {
            foreach (var kvp in items)
            {
                this.WriteItems(kvp.Key, this.GetItems(kvp.Key));
            }
        }

        private int OnSortItemByName(NodeItem l, NodeItem r)
        {
            return l.Name.CompareTo(r.Name);
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

                list.Sort(OnSortItemByName);
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
            string directoryName = Path.GetDirectoryName(fullPath);
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }

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

            items.Sort(OnSortItemByName);
        }

        private string GetFilePath(EItemType item)
        {
            string filePath = Path.Combine(Logic.Utility.TemplateConfigsPath, this.GetItemFileName(item));
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
            this.items.TryGetValue(item, out List<NodeItem> list);
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
