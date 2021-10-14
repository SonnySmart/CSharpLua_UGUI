﻿using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace libx
{
    public class Initializer : MonoBehaviour
    {
        public bool splash;
        public bool loggable;
        public VerifyBy verifyBy = VerifyBy.CRC;
        public string downloadURL;
        public bool development;
        public bool dontDestroyOnLoad = true;
        public string launchScene;
        public string luaEntry = "Game";
        public string[] searchPaths;
        public string[] patches4Init;
        public bool updateAll;
        private void Start()
        {
            // 启动时间
            LuaFramework.Util.CalcTime("程序启动");

            if (dontDestroyOnLoad)
            {
                DontDestroyOnLoad(gameObject);
            }
            EditorInit();  
            Init();
            Assets.updateAll = updateAll;
            Assets.downloadURL = downloadURL;
            Assets.verifyBy = verifyBy;
            Assets.searchPaths = searchPaths;
            Assets.patches4Init = patches4Init;
            Assets.Initialize(error =>
            {
                if (!string.IsNullOrEmpty(error))
                {
                    Debug.LogError(error);
                    return;
                }

                if (splash)
                {
                    Assets.LoadSceneAsync(R.GetScene("Splash"));
                }
                else
                {
                    Assets.LoadSceneAsync(R.GetScene(launchScene)); 
                }
            });   
        }  

        [Conditional("UNITY_EDITOR")] 
        private void EditorInit()
        {
            Assets.development = development; 
            Assets.loggable = loggable;

            //AppConst
            LuaFramework.AppConst.development = development;
            LuaFramework.AppConst.luaBundle = !development;
            LuaFramework.AppConst.luaEntry = luaEntry;
        }

        // PLATFORM        
        [Conditional("UNITY_STANDALONE")]
        [Conditional("UNITY_IOS")]
        [Conditional("UNITY_ANDROID")]
        private void Init()
        {
            Assets.development = development; 
            Assets.loggable = loggable;

            //AppConst
            LuaFramework.AppConst.development = development;
            LuaFramework.AppConst.luaBundle = true;
            LuaFramework.AppConst.luaEntry = luaEntry;
        }

        [Conditional("UNITY_EDITOR")] 
        private void Update()
        {
            Assets.loggable = loggable; 
        }
    }
}
