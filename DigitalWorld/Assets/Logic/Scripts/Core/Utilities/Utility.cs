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
        public const string ScriptPath = LogicAssetPath + "/" + "Scripts";
        public const string GeneratedScriptPath = ScriptPath + "/" + "Generated";
        public const string ImplementScriptPath = ScriptPath + "/" + "Implements";
        public const string BehaviourPath = ConfigsPath + "/" + BehavioursRelativePath;

        public const string GeneratedTips = @"/**
 * 该文件通过代码生成器生成
 * 请不要修改这些代码
 * 当然，修改了也没什么用，如果你有兴趣你可以试试。
 */";

        internal const string LogicExportPath = "Assets/Res/Logic";
        public const string LogicExportBehaviourPath = LogicExportPath + "/" + BehavioursRelativePath;

        public static Dictionary<string, int> KeyDict = new Dictionary<string, int>();

        public readonly static string[] usingNamespaces = new string[5] { ProjectNamespace + ".Game", "UnityEngine", "System", "Dream", "Dream.Core" };
        /// <summary>
        /// 逻辑命名空间
        /// </summary>
        public const string LogicNamespace = ProjectNamespace + ".Logic";

        public const string ActionName = "Actions";
        public const string PropertyName = "Properties";
        public const string EventName = "Events";

        public const string LogicActionNamespace = LogicNamespace + "." + ActionName;
        public const string LogicEventNamespace = LogicNamespace + "." + EventName;
        public const string LogicPropertyNamespace = LogicNamespace + "." + PropertyName;

        private static Assembly csharpAss = null;

        public static Color kSplitLineColor;
        #endregion

        #region Common
        static Utility()
        {
            kSplitLineColor = new Color(0.12f, 0.12f, 0.12f, 0.62f);
        }

#if UNITY_EDITOR
        /// <summary>
        /// 拷贝节点缓冲器
        /// </summary>
        public static NodeBase copyNodeBuffer = null;

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

        public static void SaveDataToFile(string data, string filePath, FileMode mode = FileMode.Create)
        {
            string folderPath = System.IO.Path.GetDirectoryName(filePath);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            FileStream stream = File.Open(filePath, mode);
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(data);
            writer.Flush();
            writer.Close();
            stream.Close();
        }

        /// <summary>
        /// 写入byte流到文件
        /// </summary>
        /// <param name="data"></param>
        /// <param name="size"></param>
        /// <param name="filePath"></param>
        /// <param name="mode"></param>
        public static void SaveDataToFile(byte[] data, int size, string filePath, FileMode mode = FileMode.Create)
        {
            string folderPath = System.IO.Path.GetDirectoryName(filePath);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            FileStream stream = File.Open(filePath, mode);
            BinaryWriter writer = new BinaryWriter(stream);
            writer.Write(data, 0, size);
            writer.Flush();
            writer.Close();
            stream.Close();
        }

        /// <summary>
        /// 获取自己的名字 就是说 取最后一个.后面的名字
        /// </summary>
        /// <param name="fullName"></param>
        /// <returns></returns>
        public static string GetSelfName(string fullName)
        {
            if (string.IsNullOrEmpty(fullName))
                return fullName;
            if (!fullName.Contains('.'))
                return fullName;

            return fullName[(fullName.LastIndexOf('.') + 1)..];
        }

        /// <summary>
        /// 获取标准化枚举命名
        /// 将所有的 . 以及 / 替换成 _ 
        /// 因为枚举是只可以使用 _ 符号的
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetStandardizationEnumName(string name)
        {
            name = name.Replace('.', '_');
            name = name.Replace('/', '_');
            return name;
        }

        /// <summary>
        /// 获取标准化的命名空间名字
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string GetStandardizationNamespaceName(string name)
        {
            name = name.Replace('_', '.');
            name = name.Replace('/', '.');
            return name;
        }

        /// <summary>
        /// 获取基于文件夹的名字
        /// </summary>
        /// <param name="name">标准命名空间的名字</param>
        /// <returns></returns>
        public static string GetDirectoryFileName(string name)
        {
            return name.Replace('.', '/');
        }

        /// <summary>
        /// 通过.来组合命名 用在命名空间上
        /// </summary>
        /// <param name="name1">命名空间前缀</param>
        /// <param name="name2">相对自身的命名空间</param>
        /// <returns></returns>
        public static string CombineName(string name1, string name2)
        {
            string ret = "";
            if (!string.IsNullOrEmpty(name1))
            {
                ret += name1;
            }

            if (!string.IsNullOrEmpty(name1) && !string.IsNullOrEmpty(name2))
            {
                ret += '.';
            }

            if (!string.IsNullOrEmpty(name2))
            {

                ret += name2;
            }

            return ret;
        }



        /// <summary>
        /// 获取命名空间的名字
        /// 即最后一个点之前的名字
        /// </summary>
        /// <param name="fullName"></param>
        /// <returns></returns>
        public static string GetNamespaceName(string fullName)
        {
            if (string.IsNullOrEmpty(fullName))
                return fullName;
            if (!fullName.Contains('.'))
                return string.Empty;

            return fullName[..fullName.LastIndexOf('.')];
        }

        public static NodeBase CreateNewAction(EAction action)
        {
            string name = string.Format("{0}.{1}", LogicActionNamespace, GetStandardizationNamespaceName(action.ToString()));
            System.Type type = GetTemplateType(name);
            if (null == type)
                return null;
            NodeBase node = System.Activator.CreateInstance(type) as NodeBase;
            return node;
        }


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
#endif
        #endregion
    }
}
