#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Text;

using UnityEditor;
using UnityEngine;

namespace LuaFramework.Editor {
  public static class Compiler {
    private sealed class CompiledFail : Exception {
      public CompiledFail(string message) : base(message) {
      }
    }

#if UNITY_EDITOR_WIN
    private const string kDotnet = "dotnet";
#else
    private const string kDotnet = "/usr/local/share/dotnet/dotnet";
#endif

    private static readonly string frameworkDir_ = Application.dataPath + "/LuaFramework";
    private static readonly string outLuaDir_ = $"{frameworkDir_}/Lua";
    private static readonly string compiledScriptDir_ = Application.dataPath + "/Scripts/Compiled";
    private static readonly string outDir_ = $"{frameworkDir_}/Lua/Compiled";
    private static readonly string toolDir = Application.dataPath + "/../Tools";
    private static readonly string csharpToolsDir_ = $"{toolDir}/CSharpLua";
    private static readonly string protonToolsDir_ = $"{toolDir}/Proton";
    private static readonly string csharpLua_ = $"{csharpToolsDir_}/CSharp.lua/CSharp.lua.Launcher.dll";
    private static readonly string genProtobuf = $"{toolDir}/ProtobufGen/protogen.bat";
    private static readonly string kCompiledFrameworkScripts = "LuaFramework.Runtime";//"Compiled";
    private static readonly string kCompiledScripts = "Assembly-CSharp";//"Compiled";

    [MenuItem("LuaFramework/Compile C#2Lua", false, 80)]
    public static void Compile() {
      // 程序集编译
      Compile(compiledScriptDir_, outDir_);
      // 框架编译
      string compiledScriptDir = $"{frameworkDir_}/Scripts/Compiled";
      string outDir = $"{outLuaDir_}/LuaFramework/Scripts";
      Compile(compiledScriptDir, outDir);
      // 拷贝pb.lua文件
      CopyLuaGenerator();
    }

    /// <summary>
    /// 拷贝protobuf pb文件
    /// </summary>
    private static void CopyLuaGenerator()
    {
      string bat = $"{protonToolsDir_}/__export_proto_copylua.bat";
      var info = new ProcessStartInfo() {
        WorkingDirectory = protonToolsDir_,
        FileName = bat,
        UseShellExecute = false,
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        StandardOutputEncoding = Encoding.UTF8,
        StandardErrorEncoding = Encoding.UTF8,        
      };
      using (var p = Process.Start(info)) {
        var output = new StringBuilder();
        var error = new StringBuilder();
        p.OutputDataReceived += (sender, eventArgs) => output.AppendLine(eventArgs.Data);
        p.ErrorDataReceived += (sender, eventArgs) => error.AppendLine(eventArgs.Data);
        p.BeginOutputReadLine();
        p.BeginErrorReadLine();
        p.WaitForExit();
        if (p.ExitCode == 0) {
          UnityEngine.Debug.Log(output);
        } else {
          throw new CompiledFail($"Compile fail, {error}\n{output}");
        }
      }
    }

    /// <summary>
    /// Cs2Lua 编译
    /// </summary>
    /// <param name="compiledScriptDir"> 编译脚本目录 </param>
    /// <param name="outDir"> 输出目录 </param>
    private static void Compile(string compiledScriptDir, string outDir) {
      if (!CheckDotnetInstall()) {
        return;
      }

      if (!File.Exists(csharpLua_)) {
        throw new InvalidProgramException($"{csharpLua_} not found");
      }

      var outDirectoryInfo = new DirectoryInfo(outDir);
      if (outDirectoryInfo.Exists) {
        foreach (var luaFile in outDirectoryInfo.EnumerateFiles("*.lua", SearchOption.AllDirectories)) {
          luaFile.Delete();
        }
      }

      HashSet<string> libs = new HashSet<string>();
      FillUnityLibraries(libs);
      FillFrameworkLibraries(libs);
      AssemblyName assemblyName = new AssemblyName(kCompiledScripts);
      Assembly assembly = Assembly.Load(assemblyName);
      foreach (var referenced in assembly.GetReferencedAssemblies()) {
        if (referenced.Name != "mscorlib" && !referenced.Name.StartsWith("System")) {
          string libPath = Assembly.Load(referenced).Location;
          libs.Add(libPath);
        }
      }
      // add Assembly-CSharp
      libs.Add(assembly.Location);

      string[] metas = new string[] {
          $"{csharpToolsDir_}/UnityEngine.xml"
      };
      string lib = string.Join(";", libs.ToArray());
      string meta = string.Join(";", metas);
      string args = $"{csharpLua_}  -s \"{compiledScriptDir}\" -d \"{outDir}\" -l \"{lib}\" -m {meta} -c";
      string definesString = GetScriptingDefineSymbolsForGroup();
      if (!string.IsNullOrEmpty(definesString)) {
        args += $" -csc -define:{definesString}";
      }

      UnityEngine.Debug.Log($"{kDotnet} {args}");

      var info = new ProcessStartInfo() {
        FileName = kDotnet,
        Arguments = args,
        UseShellExecute = false,
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        StandardOutputEncoding = Encoding.UTF8,
        StandardErrorEncoding = Encoding.UTF8,
      };
      using (var p = Process.Start(info)) {
        var output = new StringBuilder();
        var error = new StringBuilder();
        p.OutputDataReceived += (sender, eventArgs) => output.AppendLine(eventArgs.Data);
        p.ErrorDataReceived += (sender, eventArgs) => error.AppendLine(eventArgs.Data);
        p.BeginOutputReadLine();
        p.BeginErrorReadLine();
        p.WaitForExit();
        if (p.ExitCode == 0) {
          UnityEngine.Debug.Log(output);
        } else {
          throw new CompiledFail($"Compile fail, {error}\n{output}\n{kDotnet} {args}");
        }
      }
    }

    private static string GetScriptingDefineSymbolsForGroup()
    {
      string definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
      StringBuilder sb = new StringBuilder();
      sb.Append("UNITY_5_1_OR_NEWER;");
      sb.Append("UNITY_5_2_OR_NEWER;");
      sb.Append("UNITY_5_3_OR_NEWER;");
      sb.Append("UNITY_5_4_OR_NEWER;");
      sb.Append("UNITY_5_5_OR_NEWER;");
      sb.Append("UNITY_5_6_OR_NEWER;");
      sb.Append("UNITY_2017_1_OR_NEWER;");
      sb.Append("UNITY_2017_2_OR_NEWER;");
      sb.Append("UNITY_2017_3_OR_NEWER;");
      sb.Append("UNITY_2017_4_OR_NEWER;");
      sb.Append("UNITY_2018_1_OR_NEWER;");
      sb.Append("UNITY_2018_2_OR_NEWER;");
      sb.Append("UNITY_2018_3_OR_NEWER;");
      sb.Append("UNITY_2018_4_OR_NEWER;");
      if (!definesString.Equals(""))
      {
        sb.Append(definesString);
      }
      return sb.ToString();
    }

    private static void FillUnityLibraries(HashSet<string> libs) {
      string unityObjectPath = typeof(UnityEngine.Object).Assembly.Location;
      string baseDir = Path.GetDirectoryName(unityObjectPath);
      foreach (string path in Directory.EnumerateFiles(baseDir, "*.dll")) {
        libs.Add(path);
      }
    }

    private static void FillFrameworkLibraries(HashSet<string> libs) {
      AssemblyName assemblyName = new AssemblyName(kCompiledFrameworkScripts);
      Assembly assembly = Assembly.Load(assemblyName);
      foreach (var referenced in assembly.GetReferencedAssemblies()) {
        if (referenced.Name != "mscorlib" && !referenced.Name.StartsWith("System")) {
          string libPath = Assembly.Load(referenced).Location;
          libs.Add(libPath);
        }
      }
    }

    private static bool CheckDotnetInstall() {
      bool has = InternalCheckDotnetInstall();
      if (!has) {
        UnityEngine.Debug.LogWarning("not found dotnet");
        if (EditorUtility.DisplayDialog(".NET未安装", "未安装.NET 5.0运行环境，点击确定前往安装", "确定", "取消")) {
          Application.OpenURL("https://dotnet.microsoft.com/download/dotnet/5.0");
        }
      }
      return has;
    }

    private static bool InternalCheckDotnetInstall() {
      var info = new ProcessStartInfo() {
        FileName = kDotnet,
        Arguments = "--version",
        UseShellExecute = false,
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        CreateNoWindow = true,
        StandardOutputEncoding = Encoding.UTF8,
        StandardErrorEncoding = Encoding.UTF8,
      };
      try {
        using (var p = Process.Start(info)) {
          p.WaitForExit();
          if (p.ExitCode == 0) {
            string version = p.StandardOutput.ReadToEnd();
            UnityEngine.Debug.LogFormat("found dotnet {0}", version);
            int major = version[0] - '0';
            if (major >= 5) {
              return true;
            } else {
              UnityEngine.Debug.LogErrorFormat("dotnet verson {0} must >= 5.0", version);
            }
          }
          return false;
        }
      } catch (Exception e) {
        UnityEngine.Debug.LogException(e);
        return false;
      }
    }

#if USE_LUA
        [MenuItem("LuaFramework/执行 C# 脚本 (当前Lua)", false, 81)]
#else
        [MenuItem("LuaFramework/执行 Lua 脚本 (当前C#)", false, 81)]
#endif
        static void Switch()
        {
            const string symbol = "USE_LUA";
#if USE_LUA
            ScriptingDefineSymbols.RemoveScriptingDefineSymbol(symbol);
#else
            ScriptingDefineSymbols.AddScriptingDefineSymbol(symbol);
#endif
            AssetDatabase.Refresh();
        }
  }
}
#endif