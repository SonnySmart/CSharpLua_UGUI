#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace libx
{
    public class BuildRulesPaths : ScriptableObject
    {
        [Tooltip("Assets文件夹 -> 热更新路径")]
        public string[] assetsFolders = new string[] { 
            "Assets/Lua",
            "Assets/ResHotfix"
        };
    }
}
#endif