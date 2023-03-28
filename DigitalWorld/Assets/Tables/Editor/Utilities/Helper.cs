using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Xml;
using Assets.Tables.Editor.Templates;

namespace TableGenerator
{
    public class Helper
    {
        private const int titleRowIndex = 1;
        private const int keyRowIndex = 2;
        private const int typeRowIndex = 3;
        private const int contentStartRowIndex = 4;

        public Helper()
        {
            ExcelPackage.LicenseContext = LicenseContext.Commercial;
        }
        #region Convert
        private static void ConvertXmlToExcel(string xmlFullPath, ExcelWorksheet sheet)
        {
            using (FileStream fs = File.Open(xmlFullPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(fs);

                XmlElement root = xmlDocument["table"];
                if (null != root)
                {
                    int rows = sheet.Dimension.Rows;
                    int cols = sheet.Dimension.Columns;

                    int i;
                    for (i = rows; i >= contentStartRowIndex; --i)
                    {
                        sheet.DeleteRow(i);
                    }

                    i = contentStartRowIndex;

                    foreach (XmlElement record in root.ChildNodes)
                    {
                        int j = 1;

                        foreach (XmlAttribute attribute in record.Attributes)
                        {
                            sheet.SetValue(i, j, attribute.Value);
                            j++;
                        }

                        i++;
                    }
                }
            }
        }

        public static void ConvertXmlToExcel(string xmlsPath, string excelsPath, string fileName)
        {
            string xmlFullPath = Path.Combine(xmlsPath, fileName);
            xmlFullPath += ".xml";

            string excelFullPath = Path.Combine(excelsPath, fileName);
            excelFullPath += ".xlsx";

            bool isOpened = false;

            if (File.Exists(excelFullPath))
            {
                List<Process> processes = DigitalWorld.Table.Editor.Utility.GetProcessesLockingFile(excelFullPath);

                if (processes.Count > 0)
                {
                    isOpened = true;
                    for (int i = 0; i < processes.Count; ++i)
                    {
                        processes[i].Kill();
                        processes[i].WaitForExit();
                    }
                }
            }

            using (ExcelPackage package = new ExcelPackage(excelFullPath))
            {
                ExcelWorkbook workbook = package.Workbook;
                ExcelWorksheet sheet = workbook.Worksheets["Data"];
                if (null == sheet)
                {
                    sheet = workbook.Worksheets.Add("Data");
                }

                if (null != sheet)
                {
                    ConvertXmlToExcel(xmlFullPath, sheet);
                }

                package.Save();
            }

            if (isOpened)
            {
                Process process = new Process();
                ProcessStartInfo processStartInfo = new ProcessStartInfo(excelFullPath)
                {
                    UseShellExecute = true
                };
                process.StartInfo = processStartInfo;
                process.Start();
            }
        }

        public static void ConvertXmlsToExcel(string xmlsPath, string excelsPath)
        {
            string[] xmlFiles = Directory.GetFiles(xmlsPath);

            for (int j = 0; j < xmlFiles.Length; ++j)
            {
                string xmlFullPath = xmlFiles[j];

                string xmlFileName = Path.GetFileNameWithoutExtension(xmlFiles[j]);
                string excelFullPath = Path.Combine(excelsPath, xmlFileName);
                excelFullPath += ".xlsx";

                bool isOpened = false;

                if (File.Exists(excelFullPath))
                {
                    List<Process> processes = DigitalWorld.Table.Editor.Utility.GetProcessesLockingFile(excelFullPath);

                    if (processes.Count > 0)
                    {
                        isOpened = true;
                        for (int i = 0; i < processes.Count; ++i)
                        {
                            processes[i].Kill();
                            processes[i].WaitForExit();
                        }
                    }
                }

                using (ExcelPackage package = new ExcelPackage(excelFullPath))
                {
                    ExcelWorkbook workbook = package.Workbook;
                    ExcelWorksheet sheet = workbook.Worksheets["Data"];
                    if (null == sheet)
                    {
                        sheet = workbook.Worksheets.Add("Data");

                        if (null != sheet)
                            workbook.Worksheets.MoveToStart("Data");
                    }

                    if (null != sheet)
                    {
                        ConvertXmlToExcel(xmlFullPath, sheet);
                    }

                    package.Save();
                }

                if (isOpened)
                {
                    Process process = new Process();
                    ProcessStartInfo processStartInfo = new ProcessStartInfo(excelFullPath)
                    {
                        UseShellExecute = true
                    };
                    process.StartInfo = processStartInfo;
                    process.Start();
                }
            }
        }

        private static void ConvertModelToExcel(XmlElement root, string tableFullPath)
        {
            bool isOpened = false;
            if (File.Exists(tableFullPath))
            {
                List<Process> processes = DigitalWorld.Table.Editor.Utility.GetProcessesLockingFile(tableFullPath);

                if (processes.Count > 0)
                {
                    isOpened = true;
                    for (int i = 0; i < processes.Count; ++i)
                    {
                        processes[i].Kill();
                        processes[i].WaitForExit();
                    }
                }
            }

            using (ExcelPackage package = new ExcelPackage(tableFullPath))
            {
                ExcelWorkbook workbook = package.Workbook;

                ExcelWorksheet sheet = workbook.Worksheets["Data"];
                if (null == sheet)
                {
                    sheet = workbook.Worksheets.Add("Data");
                    if (null != sheet)
                        workbook.Worksheets.MoveToStart("Data");
                }

                if (null != sheet)
                {
                    int col = 1;
                    foreach (XmlElement ele in root.ChildNodes)
                    {
                        sheet.SetValue(titleRowIndex, col, ele.GetAttribute("desc"));
                        sheet.SetValue(keyRowIndex, col, ele.GetAttribute("name"));
                        sheet.SetValue(typeRowIndex, col, ele.GetAttribute("type"));
                        col += 1;
                    }
                    sheet.View.FreezePanes(contentStartRowIndex, 2);
                }

                package.Save();
            }

            if (isOpened)
            {
                Process process = new Process();
                ProcessStartInfo processStartInfo = new ProcessStartInfo(tableFullPath)
                {
                    UseShellExecute = true
                };
                process.StartInfo = processStartInfo;

                process.Start();
            }
        }

        private static void ConvertExcelToConfig(ExcelWorksheet sheet, string configFullPath)
        {
            using (FileStream fs = File.Open(configFullPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                fs.Seek(0, SeekOrigin.Begin);
                fs.SetLength(0);

                int rows = sheet.Dimension.Rows;
                int cols = sheet.Dimension.Columns;

                XmlDocument xmlDocument = new XmlDocument();
                XmlDeclaration xmlDeclaration = xmlDocument.CreateXmlDeclaration("1.0", "utf-8", null);
                xmlDocument.AppendChild(xmlDeclaration);

                XmlElement root = xmlDocument.CreateElement("table");

                for (int i = contentStartRowIndex; i <= rows; ++i)
                {
                    XmlElement record = xmlDocument.CreateElement("record");

                    for (int j = 1; j <= cols; ++j)
                    {
                        object head = sheet.GetValue(keyRowIndex, j);
                        object value = sheet.GetValue(i, j);
                        record.SetAttribute(null != head ? head.ToString() : string.Empty, null != value ? value.ToString() : string.Empty);
                    }
                    root.AppendChild(record);
                }

                xmlDocument.AppendChild(root);
                xmlDocument.Save(fs);
                fs.Close();
            }
        }
        #endregion

        #region Generate Tables
        public static void GenerateExcelsFromModel(string modelPath, string excelsPath)
        {
            #region 先清空表格文件夹
            string[] excelFiles = Directory.GetFiles(excelsPath);
            for (int i = 0; i < excelFiles.Length; ++i)
            {
                if (File.Exists(excelFiles[i]))
                {
                    List<Process> processes = DigitalWorld.Table.Editor.Utility.GetProcessesLockingFile(excelFiles[i]);

                    if (processes.Count > 0)
                    {
                        for (int j = 0; j < processes.Count; ++j)
                        {
                            processes[j].Kill();
                            processes[j].WaitForExit();
                        }
                    }
                    File.Delete(excelFiles[i]);
                }
            }
            #endregion

            using (FileStream fs = File.Open(modelPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(fs);
                XmlElement root = xmlDocument["models"];
                if (null != root)
                {
                    foreach (XmlElement ele in root.ChildNodes)
                    {
                        string tableName = ele.GetAttribute("name");
                        string tableFullPath = Path.Combine(excelsPath, tableName);
                        tableFullPath += ".xlsx";

                        ConvertModelToExcel(ele, tableFullPath);
                    }
                }
            }
        }
        #endregion

        #region Generate Codes
        public static void GenerateCodesFromModel(string modelPath, string codePath)
        {
            #region 先清空代码文件夹
            string[] codeFiles = Directory.GetFiles(codePath);
            for (int i = 0; i < codeFiles.Length; ++i)
            {
                File.Delete(codeFiles[i]);
            }
            #endregion

            using (FileStream fs = File.Open(modelPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                List<string> tableNames = new List<string>();
                List<string> classNames = new List<string>();

                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(fs);
                XmlElement root = xmlDocument["models"];
                if (null != root)
                {
                    string namespaceName = root.GetAttribute("namespace");

                    foreach (XmlElement ele in root.ChildNodes)
                    {

                        string tableName = ele.GetAttribute("name");
                        string className = ele.GetAttribute("name");
                        className = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(className);

                        tableNames.Add(tableName);
                        classNames.Add(className);

                        string infoString = ParseInfoXml(className, ele);
                        string tableString = ParseTableXml(className, tableName, ele);

                        string tableCodeFileString = GenerateTableCodeFileString(infoString, tableString, namespaceName);
                        string codeFullPath = Path.Combine(codePath, className);
                        codeFullPath += ".cs";

                        using (FileStream codeFs = File.Open(codeFullPath, FileMode.Create, FileAccess.ReadWrite))
                        {
                            StreamWriter writer = new StreamWriter(codeFs);
                            writer.Write(tableCodeFileString);
                            writer.Flush();
                            writer.Close();
                            codeFs.Close();
                        }
                    }

                    string tableManagerCodeString = GenerateTableManagerCode(namespaceName, tableNames.ToArray(), classNames.ToArray());
                    string tableManagerCodeFullPath = Path.Combine(codePath, "TableManager");
                    tableManagerCodeFullPath += ".cs";

                    using (FileStream codeFs = File.Open(tableManagerCodeFullPath, FileMode.Create, FileAccess.ReadWrite))
                    {
                        StreamWriter writer = new StreamWriter(codeFs);
                        writer.Write(tableManagerCodeString);
                        writer.Flush();
                        writer.Close();
                        codeFs.Close();
                    }
                }
            }
        }


        private static string GenerateTableManagerCode(string namespaceName, string[] tableNames, string[] tableClassNames)
        {
            TableManagerTemplate tmp = new TableManagerTemplate
            {
                Session = new Dictionary<string, object>
                {
                    ["namespaceName"] = namespaceName,
                    ["tableNames"] = tableNames,
                    ["tableClassNames"] = tableClassNames,
                }
            };

            tmp.Initialize();
            string outP = tmp.TransformText();
            return outP;
        }

        private static string GenerateTableCodeFileString(string infoString, string tableString, string namespaceName)
        {
            TableFileTemplate tableFileTemplate = new TableFileTemplate
            {
                Session = new Dictionary<string, object>
                {
                    ["namespaceName"] = namespaceName,
                    ["info"] = infoString,
                    ["table"] = tableString
                }
            };

            tableFileTemplate.Initialize();

            string outP = tableFileTemplate.TransformText();
            return outP;
        }

        private static string ParseTableXml(string className, string tableName, XmlElement ele)
        {
            TableTemplate tableTemplate = new TableTemplate
            {
                Session = new Dictionary<string, object>
                {
                    ["tableName"] = tableName,
                    ["className"] = className,
                    ["describe"] = ele.GetAttribute("desc")
                }
            };

            tableTemplate.Initialize();

            string outP = tableTemplate.TransformText();
            return outP;
        }

        private static string ParseInfoXml(string className, XmlElement ele)
        {
            InfoTemplate infoTemplate = new InfoTemplate
            {
                Session = new Dictionary<string, object>
                {
                    ["name"] = className,
                    ["describe"] = ele.GetAttribute("desc")
                }
            };

            List<string> propertyNames = new List<string>();
            List<string> variableNames = new List<string>();
            List<string> variableTypes = new List<string>();
            List<string> variableDescribes = new List<string>();
            List<string> variableEncodes = new List<string>();
            List<string> variableDecodes = new List<string>();
            List<string> variableCalculates = new List<string>();

            foreach (XmlElement n in ele)
            {
                string name = n.GetAttribute("name");
                variableNames.Add(name);
                propertyNames.Add(name.Substring(0, 1).ToUpper() + name.Substring(1));
                variableTypes.Add(GetTypeName(n.GetAttribute("type")));
                variableDescribes.Add(n.GetAttribute("desc"));
                variableEncodes.Add(GetEncodeFuncName(n.GetAttribute("baseType")));
                variableDecodes.Add(GetDecodeFuncName(n.GetAttribute("baseType")));
                variableCalculates.Add(GetCalculateSizeFuncName(n.GetAttribute("baseType")));
            }

            infoTemplate.Session["propertyNames"] = propertyNames.ToArray();
            infoTemplate.Session["variableNames"] = variableNames.ToArray();
            infoTemplate.Session["variableTypes"] = variableTypes.ToArray();
            infoTemplate.Session["variableDescribes"] = variableDescribes.ToArray();
            infoTemplate.Session["variableEncodes"] = variableEncodes.ToArray();
            infoTemplate.Session["variableDecodes"] = variableDecodes.ToArray();
            infoTemplate.Session["variableCalculates"] = variableCalculates.ToArray();

            infoTemplate.Initialize();

            string outP = infoTemplate.TransformText();
            return outP;
        }
        #endregion

        public static void ConvertExcelToXml(string excelsPath, string xmlsPath, string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new FileNotFoundException(fileName);

            string excelFullPath = Path.Combine(excelsPath, fileName);
            excelFullPath += ".xlsx";

            FileInfo fileInfo = new FileInfo(excelFullPath);
            using (ExcelPackage package = new ExcelPackage(fileInfo))
            {
                ExcelWorkbook workbook = package.Workbook;
                ExcelWorksheet sheet = workbook.Worksheets["Data"];
                if (null != sheet)
                {
                    string configFullPath = Path.Combine(xmlsPath, fileName);
                    configFullPath += ".xml";
                    ConvertExcelToConfig(sheet, configFullPath);
                }
            }
        }

        /// <summary>
        /// 将excel转换导出至xml
        /// 一切都已Excel为基准
        /// </summary>
        /// <param name="tablePath"></param>
        /// <param name="configPath"></param>
        public static void ConvertExcelsToXmls(string excelsPath, string xmlsPath)
        {
            #region 先清空配置文件夹
            string[] xmlFiles = Directory.GetFiles(xmlsPath);
            for (int i = 0; i < xmlFiles.Length; ++i)
            {
                if (File.Exists(xmlFiles[i]))
                {
                    File.Delete(xmlFiles[i]);
                }
            }
            #endregion

            string[] excelFiles = Directory.GetFiles(excelsPath);

            for (int j = 0; j < excelFiles.Length; ++j)
            {
                string excelFullPath = excelFiles[j];
                string fileName = Path.GetFileNameWithoutExtension(excelFullPath);

                if (fileName.Contains("~$"))
                {
                    continue;
                }

                FileInfo fileInfo = new FileInfo(excelFullPath);

                using (ExcelPackage package = new ExcelPackage(fileInfo))
                {
                    ExcelWorkbook workbook = package.Workbook;

                    ExcelWorksheet sheet = workbook.Worksheets["Data"];
                    if (null != sheet)
                    {
                        string configFullPath = Path.Combine(xmlsPath, fileName);
                        configFullPath += ".xml";
                        ConvertExcelToConfig(sheet, configFullPath);
                    }
                }
            }
        }

        #region Common
        public static System.Type GetType(string typeName)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly asm in assemblies)
            {
                Type[] types = asm.GetTypes();
                foreach (Type type in types)
                {
                    if (type.FullName == typeName)
                        return type;
                }
            }

            return null;
        }

        private static string GetTypeName(string typeName)
        {
            string ret = string.Empty;
            System.Type type = GetType(typeName);
            // 如果是泛型
            if (type.IsGenericType)
            {
                switch (type.Name)
                {
                    case "List`1":
                    {
                        ret = string.Format("List<{0}>", type.GenericTypeArguments[0].FullName);
                        break;
                    }
                }
            }
            else
            {
                ret = type.FullName;
            }

            return ret;
        }

        private static string GetEncodeFuncName(string baseType)
        {
            if (baseType.ToLower() == "enum")
            {
                return string.Format("EncodeEnum");
            }
            else
            {
                return string.Format("Encode");
            }
        }

        private static string GetDecodeFuncName(string baseType)
        {
            if (baseType.ToLower() == "enum")
            {
                return string.Format("DecodeEnum");
            }
            else
            {
                return string.Format("Decode");
            }
        }

        private static string GetCalculateSizeFuncName(string baseType)
        {
            if (baseType.ToLower() == "enum")
            {
                return string.Format("CalculateSizeEnum");
            }
            else
            {
                return string.Format("CalculateSize");
            }
        }
        #endregion
    }
}
