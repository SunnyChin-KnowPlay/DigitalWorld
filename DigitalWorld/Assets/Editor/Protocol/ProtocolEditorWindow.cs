using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using System.IO;
using DigitalWorld.Utilities;

[InitializeOnLoad]
public class ProtocolEditorWindow : EditorWindow
{
    private const string outputKey = "Protocol.Output";
    private const string defaultOutPutPath = "Scripts/Protocols/Generated";

    private const string srcKey = "Protocol.Src";
    private const string defaultSrcPath = "D:/Projects/DigitalWorld/DigitalWorld-Config/Protocols";

    static ProtocolEditorWindow()
    {
        //PlayerPrefs.DeleteAll();

        //Utility.SetDefaultString(outputKey, defaultOutPutPath);
        //Utility.SetDefaultString(srcKey, defaultSrcPath);
    }

    [MenuItem("Protocol/Generate Protocols")]
    private static void GenerateProtocols()
    {
        string fileName = "ProtocolGenerator.exe";
        string workingDirectory = Path.Combine(Application.dataPath, "Editor/Protocol");

        Process process = new Process();
        process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
        process.StartInfo.ErrorDialog = true;
        process.StartInfo.FileName = fileName;
        process.StartInfo.WorkingDirectory = workingDirectory;
        string protocolOutputPath = Path.Combine(Application.dataPath, Utility.GetString(outputKey, defaultOutPutPath));
        string args = string.Format("{0} {1}", Utility.GetString(srcKey, defaultSrcPath), protocolOutputPath);
        process.StartInfo.Arguments = args;

        process.Start();
        process.WaitForExit();
        process.Close();

        AssetDatabase.Refresh();
    }
}
