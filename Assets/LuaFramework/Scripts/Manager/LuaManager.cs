using UnityEngine;
using System.Collections;
using LuaInterface;
using System;
using System.IO;
using libx;
using System.Collections.Generic;
using System.Diagnostics;

namespace LuaFramework {
    public class LuaManager : Manager {
        private LuaState lua;
        private LuaLoader loader;
        private LuaLooper loop = null;
        // Cs2Lua
        private LuaFunction bindFn_;
        private LuaFunction isIEnumeratorFn_;

        // Use this for initialization
        void Awake() {
            loader = new LuaLoader();
            lua = new LuaState();
            this.OpenLibs();
            lua.LuaSetTop(0);

            LuaBinder.Bind(lua);
            DelegateFactory.Init();
            LuaCoroutine.Register(lua, this);
        }

        [Conditional("USE_LUA")]
        public void InitStart() {
            Util.CalcTime("lua启动开始");
            InitLuaPath();
            InitLuaBundle();
            this.lua.Start();    //启动LUAVM
            this.StartDebug(); // 这里使用了2s发布模式关闭掉 AppConst.development
            this.StartMain();
            this.StartLooper();
            Util.CalcTime("lua启动完成");
        }

        void StartLooper() {
            loop = gameObject.AddComponent<LuaLooper>();
            loop.luaState = lua;
        }

        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int LuaOpen_Socket_Core(IntPtr L)
        {        
            return LuaDLL.luaopen_socket_core(L);
        }

        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int LuaOpen_Mime_Core(IntPtr L)
        {
            return LuaDLL.luaopen_mime_core(L);
        }

        //cjson 比较特殊，只new了一个table，没有注册库，这里注册一下
        protected void OpenCJson() {
            lua.LuaGetField(LuaIndexes.LUA_REGISTRYINDEX, "_LOADED");
            lua.OpenLibs(LuaDLL.luaopen_cjson);
            lua.LuaSetField(-2, "cjson");

            lua.OpenLibs(LuaDLL.luaopen_cjson_safe);
            lua.LuaSetField(-2, "cjson.safe");
        }

        /// <summary>
        /// 开启socket
        /// </summary>
        protected void OpenLuaSocket()
        {
            LuaConst.openLuaSocket = true;
            lua.BeginPreLoad();
            lua.RegFunction("socket.core", LuaOpen_Socket_Core);
            lua.EndPreLoad();
        }

        /// <summary>
        /// 启动调试
        /// </summary>
        void StartDebug() {
            if (!AppConst.development)
                return;
            LuaConst.openLuaDebugger = true;
            lua.DoFile("LuaDebugjit.lua");
            LuaFunction main = lua.GetFunction("StartDebug");
            main.Call("localhost", 7003);
            main.Dispose();
            main = null; 
        }

        void StartMain() {
            lua.DoFile("Main.lua");
            LuaFunction main = lua.GetFunction("Main");
            main.Call();
            main.Dispose();
            main = null;    
        }
        
        /// <summary>
        /// 初始化加载第三方库
        /// </summary>
        void OpenLibs() {
            lua.OpenLibs(LuaDLL.luaopen_pb);      
            lua.OpenLibs(LuaDLL.luaopen_sproto_core);
            lua.OpenLibs(LuaDLL.luaopen_protobuf_c);
            lua.OpenLibs(LuaDLL.luaopen_lpeg);
            lua.OpenLibs(LuaDLL.luaopen_bit);
            lua.OpenLibs(LuaDLL.luaopen_socket_core);

            //luaide socket 开启
            this.OpenLuaSocket();
            this.OpenCJson();
        }

        /// <summary>
        /// 初始化Lua代码加载路径
        /// </summary>
        void InitLuaPath() {
            if (AppConst.development) {
                string rootPath = AppConst.FrameworkRoot;
                lua.AddSearchPath(rootPath + "/Lua");
                lua.AddSearchPath(rootPath + "/ToLua/Lua");
            } else {
                lua.AddSearchPath(Util.DataPath + "lua");
            }
        }

        /// <summary>
        /// 初始化LuaBundle
        /// </summary>
        void InitLuaBundle() {
            if (!loader.beZip)
                return;
            
            List<string> bundles;
            if (Assets.GetLuaAssetBundles(out bundles))
            {
                foreach (var bundle in bundles)
                {
                    loader.AddBundle(bundle);
                }
            }
            else
            {
                UnityEngine.Debug.LogError("没有加载Lua Assetbundle请检查环境配置");
            }
        }

        public void DoFile(string filename) {
            lua.DoFile(filename);
        }

        private object Invoke(LuaFunction fn, params object[] args)
        {
            object obj = null;
            int len = (args == null) ? 0 : args.Length;
            if (fn != null)
            {
                switch (len) {
                    case 0: { obj = fn.Invoke<object>(); break; }
                    case 1: { obj = fn.Invoke<object, object>(args[0]); break; }
                    case 2: { obj = fn.Invoke<object, object, object>(args[0], args[1]); break; }
                    case 3: { obj = fn.Invoke<object, object, object, object>(args[0], args[1], args[2]); break; }
                    case 4: { obj = fn.Invoke<object, object, object, object, object>(args[0], args[1], args[2], args[3]); break; }
                    case 5: { obj = fn.Invoke<object, object, object, object, object, object>(args[0], args[1], args[2], args[3], args[4]); break; }
                    case 6: { obj = fn.Invoke<object, object, object, object, object, object, object>(args[0], args[1], args[2], args[3], args[4], args[5]); break; }
                    case 7: { obj = fn.Invoke<object, object, object, object, object, object, object, object>(args[0], args[1], args[2], args[3], args[4], args[5], args[6]); break; }
                    case 8: { obj = fn.Invoke<object, object, object, object, object, object, object, object, object>(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7]); break; }
                    case 9: { obj = fn.Invoke<object, object, object, object, object, object, object, object, object, object>(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8]); break; }
                }
            }
            return obj;
        }

        private void ObjectCall(LuaTable table, LuaFunction fn, params object[] args)
        {
            int len = (args == null) ? 0 : args.Length;
            if (fn != null)
            {
                switch (len) {
                    case 0: { fn.Call(table); break; }
                    case 1: { fn.Call(table, args[0]); break; }
                    case 2: { fn.Call(table, args[0], args[1]); break; }
                    case 3: { fn.Call(table, args[0], args[1], args[2]); break; }
                    case 4: { fn.Call(table, args[0], args[1], args[2], args[3]); break; }
                    case 5: { fn.Call(table, args[0], args[1], args[2], args[3], args[4]); break; }
                    case 6: { fn.Call(table, args[0], args[1], args[2], args[3], args[4], args[5]); break; }
                    case 7: { fn.Call(table, args[0], args[1], args[2], args[3], args[4], args[5], args[6]); break; }
                    case 8: { fn.Call(table, args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7]); break; }
                }
            }
        }

        // Update is called once per frame
        public object Invoke(string function, params object[] args)
        {
            int len = (args == null) ? 0 : args.Length;
            object obj = null;
            using (var fn = lua.GetFunction(function))
            {
                obj = Invoke(fn, args);
            }
            return obj;
        }

        // Table对象调用方法
        public void ObjectCall(LuaTable table, string function, params object[] args)
        {
            int len = (args == null) ? 0 : args.Length;   

            if (table == null)
                return;
                
            using (var fn = table.GetLuaFunction(function))
            {
                ObjectCall(table, fn, args);
            }
        }

        public void LuaGC() {
            if (lua != null)
                lua.LuaGC(LuaGCOptions.LUA_GCCOLLECT);
        }

        public void Close() {
            if (loop != null) {
                loop.Destroy();
                loop = null;
            }

            LuaGC();
            lua.Dispose();
            lua = null;

            loader.Dispose();
            loader = null;

            if (bindFn_ != null) {
                bindFn_.Dispose();
                bindFn_ = null;
            }
            if (isIEnumeratorFn_ != null) {
                isIEnumeratorFn_.Dispose();
                isIEnumeratorFn_ = null;
            }
        }

        public LuaState GetMainState()
        {
            return lua;
        }

        public LuaLooper GetLooper()
        {
            return loop;
        }

        internal LuaTable BindLua(LuaBehaviour bridgeMonoBehaviour) {
            if (bindFn_ == null) {
                bindFn_ = lua.GetFunction("UnityEngine.bind");
                if (bindFn_ == null) {
                    throw new InvalidProgramException();
                }
            }
            return bindFn_.Invoke<LuaBehaviour, string, string, UnityEngine.Object[], LuaTable>(
                bridgeMonoBehaviour,
                bridgeMonoBehaviour.LuaClass,
                bridgeMonoBehaviour.SerializeData,
                bridgeMonoBehaviour.SerializeObjects);
        }

        internal bool IsLuaIEnumerator(LuaTable t) {
            if (isIEnumeratorFn_ == null) {
                isIEnumeratorFn_ = lua.GetFunction("System.IsIEnumerator");
                if (isIEnumeratorFn_ == null) {
                throw new InvalidProgramException();
                }
            }
            return isIEnumeratorFn_.Invoke<LuaTable, bool>(t);
        }

        /// <summary>
        /// 获取lua文本
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string ReadFile(string fileName)
        {
            string content = "";
            var bytes = loader.ReadFile(fileName);
            content = System.Text.Encoding.UTF8.GetString(bytes);
            return content;
        }
    }
}