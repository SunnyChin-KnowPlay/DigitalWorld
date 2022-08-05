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
        Condition,
        Event,
    }

    public static class Utility
    {
        #region Params
        private const string ConfigsRelativePath = "Configs";
        private const string BehavioursRelativePath = "Behaviours";


        private const string LogicAssetPath = "Assets/Logic";
        public const string ConfigsPath = LogicAssetPath + "/" + ConfigsRelativePath;
        public const string TemplateConfigsPath = ConfigsPath + "/" + "Template";
        public const string GenreatedConfigsPath = ConfigsPath + "/" + "Generated";
        public const string CodesPath = LogicAssetPath + "/" + "Scripts/Generated";
        public const string BehaviourPath = ConfigsPath + "/" + BehavioursRelativePath;

        internal const string LogicExportPath = "Assets/Res/Logic";
        public const string LogicExportBehaviourPath = LogicExportPath + "/" + BehavioursRelativePath;

        public static Dictionary<string, int> KeyDict = new Dictionary<string, int>();

        public readonly static string[] usingNamespaces = new string[2] { "DigitalWorld.Game", "UnityEngine" };
        /// <summary>
        /// 逻辑命名空间
        /// </summary>
        public const string LogicNamespace = "DigitalWorld.Logic";
      
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
            return typeof(System.Object);
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

        public static NodeBase CreateNewCondition(ECondition cond)
        {
            string name = string.Format("{0}.Condition{1}", LogicNamespace, cond);
            System.Type type = GetTemplateType(name);
            if (null == type)
                return null;
            NodeBase bc = System.Activator.CreateInstance(type) as NodeBase;
            return bc;
        }
        #endregion
    }
}
