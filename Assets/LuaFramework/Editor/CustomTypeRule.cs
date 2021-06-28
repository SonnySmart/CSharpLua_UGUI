

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using static ToLuaMenu;

public static class CustomTypeRule
{
    /// <summary>
    /// 引擎需要过滤的类型:过滤一些报错或者警告类
    /// </summary>
    private static List<string> _unUseEngineTypes = new List<string>() {
        "UnityEngine.UI.BaseVertexEffect",
        "UnityEngine.EventSystems.TouchInputModule"     
    };

    /// <summary>
    /// 框架需要过滤的类型:过滤一些报错或者警告类
    /// </summary>
    private static List<string> _unUseGameFrameworkTypes = new List<string>() {
        "UnityGameFramework.Runtime.BuiltinVersionListSerializer"
    };

    /// <summary>
    /// 这个暂时不知道干嘛用
    /// </summary>
    private static Dictionary<string, System.Type> _useBaseTypes = new Dictionary<string, System.Type>() {
        //{"UnityGameFramework.Runtime.UIFormLogic", typeof(UnityEngine.GameObject)}
    };

    /// <summary>
    /// 设置this扩展类型
    /// </summary>
    private static Dictionary<string, System.Type> _useExtendTypes = new Dictionary<string, System.Type>() {
        //{"UnityEngine.GameObject", typeof(UnityExtension)}
    };

    public static BindType _GT(Type t)
    {
        BindType bind = CustomSettings._GT(t);
        string key = t.FullName;
        if (_useBaseTypes.ContainsKey(key))
        {
            bind.SetBaseType(_useBaseTypes[key]);
        }
        if (_useExtendTypes.ContainsKey(key))
        {
            bind.AddExtendType(_useExtendTypes[key]);
        }
        return bind;
    }

    public static DelegateType _DT(Type t)
    {
        return CustomSettings._DT(t);
    } 

    public static BindType[] GetBindTypes()
    {
        // 这是是ToLua原始的类型
        List<BindType> result = new List<BindType>();
        result.AddRange(CustomSettings.customTypeList_);

        // UnityEngine 引擎
        //GetAssemblyTypesToList(ref result, "UnityEngine");
        // UnityEngine.UI
        GetAssemblyTypesToList(ref result, "UnityEngine.UI");
        // 全局程序集 打上LuaAutoWrap标签即可
        GetAssemblyTypesToList(ref result, "Assembly-CSharp");
        // Redmoon.Protobuf 类库
        //GetAssemblyTypesToList(ref result, "Redmoon.Protobuf");
        // LitJson
        //GetAssemblyTypesToList(ref result, "LitJson");

        // TODO : 游戏里还需要额外添加的加到这里

        return result.ToArray();
    }

    static void GetAssemblyTypesToList(ref List<BindType> result, string assemblyName)
    {
        Assembly assembly = null;
        try {
            assembly = Assembly.Load(assemblyName);
        }
        catch (Exception e) {
            Debug.LogError($"Message => {e.Message} \n StackTrace => {e.StackTrace}");
            return;
        }
        
        Type[] types = assembly.GetExportedTypes();
        foreach (Type type in types)
        {
            bool isFind = false;

            // 全局程序集打了Lua标签的需要加进来
            if (assemblyName.Equals("Assembly-CSharp"))
            {
                if (type.IsDefined(typeof(LuaAutoWrapAttribute), false))
                {
                    result.Add(_GT(type));
                    //continue;
                }
                continue;
            }

            // 引擎忽略类型
            if (assemblyName.Contains("UnityEngine"))
            {
                if (_unUseEngineTypes.Contains(type.FullName))
                    continue;
            }
            
            // 代理 continue
            if (typeof(System.MulticastDelegate).IsAssignableFrom(type))
            {
                continue;
            }

            // 接口 continue
            if (type.IsInterface)
            {
                continue;
            }

            // 枚举继承至byte continue
            if (type.IsEnum)
            {
                /// <summary>
                /// public enum ShutdownType : byte
                /// 这种有问题
                /// </summary>
                /// <returns></returns>
                if (type.IsSubclassOf(typeof(System.ValueType)))
                {
                    continue;
                }
            }

            // 这个是ToLua的规则
            if (type.IsGenericType)
            {
                int pos = type.FullName.IndexOf("[");
                if (pos == -1)
                {
                    continue;
                }
            }

            // 过滤重复的
            isFind = false;
            foreach (BindType bind in result)
            {
                if (bind.type.Equals(type))
                {
                    isFind = true;
                    break;
                }
            }
            if (isFind) continue;

            // 剩余的就进行添加
            result.Add(_GT(type));
        }
    }
}