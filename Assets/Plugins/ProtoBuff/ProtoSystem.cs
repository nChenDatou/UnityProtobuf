using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System;
using UnityEditor;
using UnityEngine;

public class ProtoToClass
{
    //����protoc.exe·��
    [MenuItem("LeedenTool/ChangePath/ProtocPath")]
    public static void SetProtocPath()
    {
        string protocPath = EditorUtility.OpenFolderPanel("ѡ��ProtocĿ¼", Application.dataPath, "");
        if (!string.IsNullOrEmpty(protocPath))
        {
            EditorPrefs.SetString(Application.identifier + "-ProtoCPath", protocPath);
            UnityEngine.Debug.Log("Protoc.exe·�����óɹ��� " + protocPath);
        }
    }

    //����protoClass�ļ�·��
    [MenuItem("LeedenTool/ChangePath/ProtoClassOutPath")]
    public static void ChangeProtoOutPath()
    {
        string outPath = EditorUtility.OpenFolderPanel("ѡ��ProtoClass�洢Ŀ¼", Application.dataPath, "");
        if (!string.IsNullOrEmpty(outPath))
        {
            EditorPrefs.SetString(Application.identifier + "-ProtoClassOutPath", outPath);
            UnityEngine.Debug.Log("ProtoClass���·�����óɹ��� " + outPath);
        }
    }

    [MenuItem("LeedenTool/Proto To Class")]
    public static void GenerateClassFromProto()
    {
        string rootPath = Environment.CurrentDirectory;
        string protoPath = Path.Combine(rootPath, "Proto\\");

        //����protoc.exe·��
        string protocPath = EditorPrefs.GetString(Application.identifier + "-ProtoCPath");
        if (!string.IsNullOrEmpty(protocPath))
        {
            EditorPrefs.SetString(Application.identifier + "-ProtoCPath", protocPath);
        }
        string protoc = Path.Combine(protocPath,
            RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "protoc.exe" : "protoc");

        //����protoClass�ļ�·��
        string outPath = EditorPrefs.GetString(Application.identifier + "-ProtoClassOutPath");
        if (string.IsNullOrEmpty(outPath))
        {
            outPath = EditorUtility.OpenFolderPanel("ѡ��ProtoClass�洢Ŀ¼", Application.dataPath, "");
            if (!string.IsNullOrEmpty(outPath))
            {
                EditorPrefs.SetString(Application.identifier + "-ProtoClassOutPath", outPath);
            }
        }

        string[] protoFiles = Directory.GetFiles(protoPath, "*.proto");
        foreach (var variable in protoFiles)
        {
            string argument = $"--csharp_out={outPath} --proto_path={protoPath} {variable}";
            Run(protoc, argument);
        }
        UnityEngine.Debug.Log("����ProtoClass�ɹ�");
        AssetDatabase.Refresh();
    }

    public static Process Run(string exe, string arguments)
    {
        ProcessStartInfo info = new ProcessStartInfo
        {
            FileName = exe,
            Arguments = arguments,
            CreateNoWindow = true,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
        };

        Process process = Process.Start(info);
        process.WaitForExit();
        if (process.ExitCode != 0)
        {
            UnityEngine.Debug.LogError($"Failed to Run {arguments}. Exit code: " + process.ExitCode);
        }

        return process;
    }
}