using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using System.IO;

public class ProtocolEditorWindow : EditorWindow
{
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
        string protocolOutputPath = Path.Combine(Application.dataPath, "Scripts/Network/Protocols/Generated");
        string args = string.Format("{0} {1}", "D:/Projects/DigitalWorld/DigitalWorld-Config/Protocols", protocolOutputPath);
        process.StartInfo.Arguments = args;

        process.Start();
        process.WaitForExit();

        AssetDatabase.Refresh();
    }
}
