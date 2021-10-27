#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using LuaFramework;
using LuaFramework.Editor;

public class Packager 
{
    public static readonly string ASSETS_PATH = Application.dataPath;
    public static readonly string ASSETS_LUA_PATH = Path.Combine(ASSETS_PATH, "Lua");
    public static readonly string ASSETS_LUAFRAMEWORK_LUA_PATH = LuaConst.luaDir;
    public static readonly string ASSETS_LUAFRAMEWORK_TOLUA_LUA_PATH = LuaConst.toluaDir;

    [MenuItem("LuaFramework/Export Lua Files", false, 100)]
    public static void ExportLuaFiles()
    {
        //Gen UI Config
        FormResConfigEditor.GenUIFormConfigJson();
        
#if USE_LUA
        // C# to Lua
        Compiler.Compile();
#endif

        CopyLuaBundle();
    }

    static void CheckDirectory(string directory)
    {
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);
    }


    /// <summary>
    /// 处理Lua代码包
    /// </summary>
    static void CopyLuaBundle()
    {
        var sources = new List<string>() { 
            ASSETS_LUAFRAMEWORK_TOLUA_LUA_PATH,
            ASSETS_LUAFRAMEWORK_LUA_PATH
        };
        var target = ASSETS_LUA_PATH;
        
        CheckDirectory(target);

        foreach (var source in sources)
        {
            Tools.Diff(source, target);
        }

        AssetDatabase.Refresh();
    }

    public static void EncodeLuaFile(string source, string target)
    {
        if (!source.ToLower().EndsWith(".lua")) {
            File.Copy(source, target, true);
            return;
        }
        Tools.LuaEncoder(source, target);
    }
}
#endif