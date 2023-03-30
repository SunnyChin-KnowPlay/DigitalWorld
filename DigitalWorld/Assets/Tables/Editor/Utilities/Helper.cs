using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Assets.Tables.Editor.Templates;
using Newtonsoft.Json;
using DigitalWorld.Table.Editor;
using System.Data;
using Dream.Table;
using UnityEditor;
using UnityEngine;

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
        private static void ConvertJsonToExcel(string jsonFullPath, ExcelWorksheet sheet, string tableName)
        {
            using (FileStream fs = File.Open(jsonFullPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (StreamReader sr = new StreamReader(fs))
                {
                    Type tableType = null;

                    string tableManagerFullName = string.Format("{0}.{1}", Application.productName, "Table.TableManager");
                    Type tmType = GetType(tableManagerFullName);
                    MethodInfo getTypeTypeMethod = tmType.GetMethod("GetTableType", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

                    if (null != getTypeTypeMethod)
                    {
                        object[] methodParameters = new object[] { tableName };
                        tableType = getTypeTypeMethod.Invoke(null, methodParameters) as Type;
                    }

                    object obj = JsonConvert.DeserializeObject(sr.ReadToEnd(), tableType);

                    if (obj is ITable table)
                    {
                        DataTable dt = table.GetTable();
                        int rows = dt.Rows.Count;

                        int i;
                        for (i = rows; i >= contentStartRowIndex; --i)
                        {
                            sheet.DeleteRow(i);
                        }

                        i = contentStartRowIndex;

                        foreach (DataRow row in dt.Rows)
                        {
                            int j = 1;
                            foreach (var v in row.ItemArray)
                            {
                                sheet.SetValue(i, j, v);
                                j++;
                            }
                            i++;
                        }
                    }
                }
            }
        }

        public static void ConvertJsonToExcel(string jsonsPath, string excelsPath, string fileName)
        {
            string jsonFullPath = Path.Combine(jsonsPath, fileName);
            jsonFullPath += ".json";

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
                    ConvertJsonToExcel(jsonFullPath, sheet, fileName);
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

        public static void ConvertJsonsToExcel(string jsonsPath, string excelsPath)
        {
            string[] jsonFiles = Directory.GetFiles(jsonsPath);

            for (int j = 0; j < jsonFiles.Length; ++j)
            {
                string jsonFullPath = jsonFiles[j];

                string jsonFileName = Path.GetFileNameWithoutExtension(jsonFiles[j]);
                string excelFullPath = Path.Combine(excelsPath, jsonFileName);
                excelFullPath += ".xlsx";

                bool isOpened = false;
                try
                {
                    EditorUtility.DisplayProgressBar(string.Format("导出json至excel({0}/{1})", j, jsonFiles.Length), jsonFileName, j / (float)jsonFiles.Length);

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
                            ConvertJsonToExcel(jsonFullPath, sheet, jsonFileName);
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
                finally
                {
                    EditorUtility.ClearProgressBar();
                }


            }
        }

        private static void ConvertModelToExcel(NodeModel model, string tableFullPath)
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

                    foreach (NodeField field in model.FieldList)
                    {
                        sheet.SetValue(titleRowIndex, col, field.Description);
                        sheet.SetValue(keyRowIndex, col, field.Name);
                        sheet.SetValue(typeRowIndex, col, field.Type.ToString());
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

        private static void ConvertExcelToConfig(ExcelWorksheet sheet, string configFullPath, string fileName)
        {
            using (FileStream fs = File.Open(configFullPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                fs.Seek(0, SeekOrigin.Begin);
                fs.SetLength(0);

                string jsonString = ExcelWorksheetToJson(sheet, fileName);
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(jsonString);
                }

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

            using FileStream fs = File.Open(modelPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            using StreamReader streamReader = new StreamReader(fs);
            // 将 FileStream 读取为字符串
            string jsonString = streamReader.ReadToEnd();

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                Formatting = Newtonsoft.Json.Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            };
            Model model = JsonConvert.DeserializeObject<Model>(jsonString, settings);
            foreach (NodeModel nodeModel in model.models)
            {
                string tableFullPath = Path.Combine(excelsPath, nodeModel.Name);
                tableFullPath += ".xlsx";
                ConvertModelToExcel(nodeModel, tableFullPath);
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

            using FileStream fs = File.Open(modelPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            List<string> tableNames = new List<string>();
            List<string> classNames = new List<string>();


            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                Formatting = Newtonsoft.Json.Formatting.Indented,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            };

            using StreamReader streamReader = new StreamReader(fs);
            // 将 FileStream 读取为字符串
            string jsonString = streamReader.ReadToEnd();

            Model model = JsonConvert.DeserializeObject<Model>(jsonString, settings);

            string namespaceName = model.NamespaceName;

            foreach (NodeModel node in model.models)
            {
                string tableName = node.Name;
                string className = node.Name;
                className = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(className);

                tableNames.Add(tableName);
                classNames.Add(className);

                string infoString = ParseInfoXml(className, node);
                string tableString = ParseTableXml(className, tableName, node);

                string tableCodeFileString = GenerateTableCodeFileString(infoString, tableString, namespaceName);
                string codeFullPath = Path.Combine(codePath, className);
                codeFullPath += ".cs";

                using FileStream codeFs = File.Open(codeFullPath, FileMode.Create, FileAccess.ReadWrite);
                StreamWriter writer = new StreamWriter(codeFs);
                writer.Write(tableCodeFileString);
                writer.Flush();
                writer.Close();
                codeFs.Close();
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

        private static string ParseTableXml(string className, string tableName, NodeModel model)
        {
            TableTemplate tableTemplate = new TableTemplate
            {
                Session = new Dictionary<string, object>
                {
                    ["tableName"] = tableName,
                    ["className"] = className,
                    ["describe"] = string.IsNullOrEmpty(model.Description) ? "" : model.Description
                }
            };

            tableTemplate.Initialize();

            string outP = tableTemplate.TransformText();
            return outP;
        }

        private static string ParseInfoXml(string className, NodeModel model)
        {
            InfoTemplate infoTemplate = new InfoTemplate
            {
                Session = new Dictionary<string, object>
                {
                    ["name"] = className,
                    ["describe"] = string.IsNullOrEmpty(model.Description) ? "" : model.Description
                }
            };

            List<string> propertyNames = new List<string>();
            List<string> variableNames = new List<string>();
            List<string> variableTypes = new List<string>();
            List<string> variableDescribes = new List<string>();

            foreach (NodeField field in model.FieldList)
            //foreach (XmlElement n in ele)
            {
                string name = field.Name;
                variableNames.Add(name);
                propertyNames.Add(name.Substring(0, 1).ToUpper() + name.Substring(1));
                variableTypes.Add(GetTypeName(field.Type));
                variableDescribes.Add(string.IsNullOrEmpty(field.Description) ? "" : field.Description);

            }

            infoTemplate.Session["propertyNames"] = propertyNames.ToArray();
            infoTemplate.Session["variableNames"] = variableNames.ToArray();
            infoTemplate.Session["variableTypes"] = variableTypes.ToArray();
            infoTemplate.Session["variableDescribes"] = variableDescribes.ToArray();


            infoTemplate.Initialize();

            string outP = infoTemplate.TransformText();
            return outP;
        }
        #endregion

        public static void ConvertExcelToJson(string excelsPath, string jsonsPath, string fileName)
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
                    string configFullPath = Path.Combine(jsonsPath, fileName);
                    configFullPath += ".json";
                    ConvertExcelToConfig(sheet, configFullPath, fileName);
                }
            }
        }

        /// <summary>
        /// 将excel转换导出至xml
        /// 一切都已Excel为基准
        /// </summary>
        /// <param name="tablePath"></param>
        /// <param name="configPath"></param>
        public static void ConvertExcelsToJsons(string excelsPath, string jsonsPath)
        {
            #region 先清空配置文件夹
            string[] xmlFiles = Directory.GetFiles(jsonsPath);
            for (int i = 0; i < xmlFiles.Length; ++i)
            {
                if (File.Exists(xmlFiles[i]))
                {
                    File.Delete(xmlFiles[i]);
                }
            }
            #endregion

            string[] excelFiles = Directory.GetFiles(excelsPath);

            try
            {
                for (int j = 0; j < excelFiles.Length; ++j)
                {
                    string excelFullPath = excelFiles[j];
                    string fileName = Path.GetFileNameWithoutExtension(excelFullPath);

                    if (fileName.Contains("~$"))
                    {
                        continue;
                    }

                    EditorUtility.DisplayProgressBar(string.Format("导出excel至json({0}/{1})", j, excelFiles.Length), fileName, j / (float)excelFiles.Length);

                    FileInfo fileInfo = new FileInfo(excelFullPath);

                    using (ExcelPackage package = new ExcelPackage(fileInfo))
                    {
                        ExcelWorkbook workbook = package.Workbook;

                        ExcelWorksheet sheet = workbook.Worksheets["Data"];
                        if (null != sheet)
                        {
                            string configFullPath = Path.Combine(jsonsPath, fileName);
                            configFullPath += ".json";
                            ConvertExcelToConfig(sheet, configFullPath, fileName);
                        }
                    }
                }
            }
            finally
            {
                EditorUtility.ClearProgressBar();
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

        private static string GetTypeName(Type type)
        {
            string ret = string.Empty;

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

        public static string ExcelWorksheetToJson(ExcelWorksheet worksheet, string tableName)
        {
            DataTable dataTable = new DataTable(tableName);

            // 获取列名
            foreach (var firstRowCell in worksheet.Cells[2, 1, 2, worksheet.Dimension.End.Column])
            {
                dataTable.Columns.Add(firstRowCell.Text);
            }


            // 获取数据
            for (var rowNumber = contentStartRowIndex; rowNumber <= worksheet.Dimension.End.Row; rowNumber++)
            {
                var row = worksheet.Cells[rowNumber, 1, rowNumber, worksheet.Dimension.End.Column];
                var newRow = dataTable.NewRow();
                foreach (var cell in row)
                {
                    newRow[cell.Start.Column - 1] = cell.Text;
                }

                dataTable.Rows.Add(newRow);
            }

            string tableManagerFullName = string.Format("{0}.{1}", Application.productName, "Table.TableManager");
            Type tmType = GetType(tableManagerFullName);
            MethodInfo createTableMethod = tmType.GetMethod("CreateTable", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

            if (null != createTableMethod)
            {
                object[] methodParameters = new object[] { tableName };

                ITable table = createTableMethod.Invoke(null, methodParameters) as ITable;
                table.LoadTable(dataTable);

                // 将 DataTable 转换为 JSON
                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    TypeNameHandling = TypeNameHandling.All,
                    Formatting = Formatting.Indented,
                };
                string json = JsonConvert.SerializeObject(table, settings);

                return json;
            }

            return string.Empty;

        }
        #endregion
    }
}
