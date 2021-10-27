#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Diagnostics;
using LuaFramework;

public static class Tools
{
    public static readonly string PROJECT_PATH = Path.Combine(Application.dataPath, ".."); 
    public static readonly string TOOLS_PATH = Path.Combine(PROJECT_PATH, "Tools");
    // Diff
    public static readonly string TOOLS_DIFF_PATH = Path.Combine(TOOLS_PATH, "Diff");
    public static readonly string TOOLS_DIFF_PY_PATH = Path.Combine(TOOLS_DIFF_PATH, "diff.py");
    // LuaEncoder
    public static readonly string TOOLS_LUAENCODER_PATH = Path.Combine(TOOLS_PATH, "LuaEncoder");

    private static bool UseShellExecute
    {
        get
        {
            if (Application.platform == RuntimePlatform.WindowsEditor)
                return true;
            return false;
        }
    }

    private static void ExecuteProcess(string file, string args)
    {
        ProcessStartInfo info = new ProcessStartInfo();
        info.FileName = file;
        info.Arguments = args;
        info.WindowStyle = ProcessWindowStyle.Hidden;
        info.UseShellExecute = UseShellExecute;
        info.ErrorDialog = true;
        Util.Log(info.FileName + " " + info.Arguments);

        Process process = Process.Start(info);
        process.WaitForExit();
    }

    /// <summary>
    /// 比较文件夹不同,同步文件夹内容
    /// </summary>
    /// <param name="source"></param>
    /// <param name="target"></param>
    public static void Diff(string source, string target)
    {
        string exec = "python";
        string args = string.Format("{0} {1} {2}", TOOLS_DIFF_PY_PATH, source, target);

        ExecuteProcess(exec, args);
    }

    public static void LuaEncoder(string source, string target)
    {
        string exec = string.Empty;
        string args = string.Format("-b -g {0} {1}", source, target);

        exec = Path.Combine(TOOLS_LUAENCODER_PATH, "luajit", "luajit.exe");
        if (Application.platform == RuntimePlatform.OSXEditor)
        {
            exec = Path.Combine(exec, "luajit_mac", "luajit");
        }
        ExecuteProcess(exec, args);
    }
}
#endif