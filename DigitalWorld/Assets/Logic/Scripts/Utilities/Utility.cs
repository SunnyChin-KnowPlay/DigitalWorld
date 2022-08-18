using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

#if UNITY_EDITOR
using System.IO;
using UnityEditor;
#endif


namespace DigitalWorld.Logic
{
    public enum EItemType
    {
        Action,
        Property,
        Event,
    }

    public static class Utility
    {
        #region Params
        private const string ConfigsRelativePath = "Configs";
        private const string BehavioursRelativePath = "Behaviours";

        public const string ProjectNamespace = "DigitalWorld";

        private const string LogicAssetPath = "Assets/Logic";
        public const string ConfigsPath = LogicAssetPath + "/" + ConfigsRelativePath;
        public const string TemplateConfigsPath = ConfigsPath + "/" + "Template";
        public const string GenreatedConfigsPath = ConfigsPath + "/" + "Generated";
        public const string CodesPath = LogicAssetPath + "/" + "Scripts/Generated";
        public const string BehaviourPath = ConfigsPath + "/" + BehavioursRelativePath;

        internal const string LogicExportPath = "Assets/Res/Logic";
        public const string LogicExportBehaviourPath = LogicExportPath + "/" + BehavioursRelativePath;

        public static Dictionary<string, int> KeyDict = new Dictionary<string, int>();

        public readonly static string[] usingNamespaces = new string[5] { ProjectNamespace + ".Game", "UnityEngine", "System", "Dream", "Dream.Core" };
        /// <summary>
        /// 逻辑命名空间
        /// </summary>
        public const string LogicNamespace = ProjectNamespace + ".Logic";

        private static Assembly csharpAss = null;

        public static Color kSplitLineColor;
        #endregion

        #region Common
        static Utility()
        {
            kSplitLineColor = new Color(0.12f, 0.12f, 0.12f, 0.62f);
        }

#if UNITY_EDITOR



        public static TextAsset LoadTemplateConfig(string path)
        {
            if (string.IsNullOrEmpty(path))
                return null;

            string folderPath = TemplateConfigsPath;
            if (!Directory.Exists(folderPath))
                return null;
            string fullPath = folderPath + "/" + path + ".xml";

            return (TextAsset)AssetDatabase.LoadAssetAtPath(fullPath, typeof(TextAsset));
        }

        public static TextAsset LoadGeneratedConfig(string path)
        {
            if (string.IsNullOrEmpty(path))
                return null;

            string folderPath = GenreatedConfigsPath;
            if (!Directory.Exists(folderPath))
                return null;
            string fullPath = folderPath + "/" + path + ".xml";

            return (TextAsset)AssetDatabase.LoadAssetAtPath(fullPath, typeof(TextAsset));
        }

        /// <summary>
        /// 获取项目文件路径 Application.dataPath移除"/Assets"
        /// </summary>
        /// <returns></returns>
        public static string GetProjectDataPath()
        {
            string p = Application.dataPath.Replace("/Assets", "");
            return p;
        }

        public static void DeleteAllFiles(string path)
        {
            var files = System.IO.Directory.GetFiles(path);
            for (int i = 0; i < files.Length; ++i)
            {
                System.IO.File.Delete(files[i]);
            }

            var folders = Directory.GetDirectories(path);
            for (int i = 0; i < folders.Length; ++i)
            {
                DeleteAllFiles(folders[i]);
            }
        }

        public static void SaveDataToFile(string data, string filePath, FileMode mode = FileMode.Create)
        {
            FileStream stream = File.Open(filePath, mode);
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(data);
            writer.Flush();
            writer.Close();
            stream.Close();
        }
#endif

        public static Type GetTemplateType(string name)
        {
            Type t = null;

            if (null != csharpAss)
            {
                Type tt = csharpAss.GetType(name);
                if (tt != null)
                {
                    t = tt;
                }
            }
            if (null == t)
            {
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                foreach (var asm in assemblies)
                {
                    Type tt = asm.GetType(name);
                    if (tt != null)
                    {
                        csharpAss = asm;
                        t = tt;
                        break;
                    }
                }
            }

            return t;
        }

        /// <summary>
        /// 在CurrentDomain下获取所有的枚举类型数组
        /// </summary>
        /// <returns></returns>
        public static List<Type> GetPublicEnumTypes(List<Type> list)
        {
            if (null == list)
                list = new List<Type>();

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly asm in assemblies)
            {
                Type[] types = asm.GetTypes();
                foreach (Type type in types)
                {
                    if (type.IsEnum && type.IsPublic)
                    {
                        string namespaceName = type.Namespace;
                        if (!string.IsNullOrEmpty(namespaceName) && namespaceName.Contains(ProjectNamespace))
                        {
                            list.Add(type);
                        }
                    }
                }
            }

            return list;
        }

        private static bool CheckIsUnderlyingType(Type type)
        {
            foreach (ETypeCode c in System.Enum.GetValues(typeof(ETypeCode)))
            {
                if (type.Name == c.ToString())
                    return true;
            }
            return false;
        }

        public static List<Type> GetUnderlyingTypes(List<Type> list)
        {
            if (null == list)
                list = new List<Type>();

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly asm in assemblies)
            {
                Type[] types = asm.GetTypes();
                foreach (Type type in types)
                {
                    if (type.IsPublic && CheckIsUnderlyingType(type))
                    {
                        list.Add(type);

                    }
                }
            }

            return list;
        }

        /// <summary>
        /// 获取该类型的所有派生类的队列
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static List<Type> GetDerivedTypesFromType(List<Type> list, Type type)
        {
            if (null == list)
                list = new List<Type>();

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly asm in assemblies)
            {
                Type[] types = asm.GetTypes();
                foreach (Type t in types)
                {
                    if (t.IsPublic)
                    {
                        if (t.IsSubclassOf(type))
                        {
                            string namespaceName = type.Namespace;
                            if (!string.IsNullOrEmpty(namespaceName) && namespaceName.Contains(ProjectNamespace))
                            {
                                list.Add(t);
                            }
                        }
                    }
                }
            }

            return list;
        }

        public static System.Type GetBaseType(string t)
        {
            if (t == "Enum")
            {
                return typeof(Enum);
            }
            else if (t == "ValueType")
            {
                return typeof(ValueType);
            }
            else
            {
                return typeof(ValueType);
            }
        }

        public static System.Type GetValueType(string t)
        {
            return Type.GetType(t);

            //if (t == "int")
            //{
            //    return typeof(int);
            //}
            //else if (t == "int32")
            //{
            //    return typeof(int);
            //}
            //else if (t == "uint")
            //{
            //    return typeof(uint);
            //}
            //else if (t == "uint32")
            //{
            //    return typeof(uint);
            //}
            //else if (t == "float")
            //{
            //    return typeof(float);
            //}
            //else if (t == "bool")
            //{
            //    return typeof(bool);
            //}
            //else if (t == "string")
            //{
            //    return typeof(string);
            //}
            //else if (t == "Vector3")
            //{
            //    return typeof(Vector3);
            //}
            //else if (t == "Color")
            //{
            //    return typeof(Color);
            //}
        }

        public static NodeBase CreateNewAction(EAction action)
        {
            string name = string.Format("{0}.Action{1}", LogicNamespace, action);
            System.Type type = GetTemplateType(name);
            if (null == type)
                return null;
            NodeBase bc = System.Activator.CreateInstance(type) as NodeBase;
            return bc;
        }
        #endregion
    }
}
