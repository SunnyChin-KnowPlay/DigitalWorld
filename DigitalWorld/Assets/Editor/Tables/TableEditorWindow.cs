using DigitalWorld.Utilities.Editor;
using Dream.TableHelper;
using System.IO;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class TableEditorWindow : EditorWindow
{
    private const string outputCodeKey = "Table.OutputCode";
    private const string defaultOutPutCodePath = "Scripts/Tables/Generated";

    private const string modelKey = "Table.Model";
    private const string defaultModelPath = "D:/Projects/DigitalWorld/DigitalWorld-Config/Models/models.xml";

    static TableEditorWindow()
    {
        Utility.SetDefaultString(outputCodeKey, defaultOutPutCodePath);
        Utility.SetDefaultString(modelKey, defaultModelPath);
    }

    [MenuItem("Table/Generate Codes")]
    private static void GenerateCodes()
    {
        string codePath = Path.Combine(Application.dataPath, Utility.GetString(outputCodeKey));
        string modelPath = Utility.GetString(modelKey);

        Generator.GenerateCodesWithModel(modelPath, codePath);

        AssetDatabase.Refresh();
    }
}
