using UnityEngine;
using System.Collections;
using LuaInterface;
using System;

namespace LuaFramework {
    public class LuaManager : Manager {
        private LuaState lua;
        private LuaLoader loader;
        private LuaLooper loop = null;

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

        public void InitStart() {
            InitLuaPath();
            InitLuaBundle();
            this.lua.Start();    //启动LUAVM
            this.StartMain();
            this.StartLooper();
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
            if (AppConst.DebugMode) {
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
            if (loader.beZip) {
                loader.AddBundle("lua/lua.unity3d");
                loader.AddBundle("lua/lua_math.unity3d");
                loader.AddBundle("lua/lua_system.unity3d");
                loader.AddBundle("lua/lua_system_reflection.unity3d");
                loader.AddBundle("lua/lua_unityengine.unity3d");
                loader.AddBundle("lua/lua_common.unity3d");
                loader.AddBundle("lua/lua_logic.unity3d");
                loader.AddBundle("lua/lua_view.unity3d");
                loader.AddBundle("lua/lua_controller.unity3d");
                loader.AddBundle("lua/lua_misc.unity3d");

                loader.AddBundle("lua/lua_protobuf.unity3d");
                loader.AddBundle("lua/lua_3rd_cjson.unity3d");
                loader.AddBundle("lua/lua_3rd_luabitop.unity3d");
                loader.AddBundle("lua/lua_3rd_pbc.unity3d");
                loader.AddBundle("lua/lua_3rd_pblua.unity3d");
                loader.AddBundle("lua/lua_3rd_sproto.unity3d");
            }
        }

        public void DoFile(string filename) {
            lua.DoFile(filename);
        }

        // Update is called once per frame
        public object CallFunction(string funcName, params object[] args) {
            int len = (args == null) ? 0 : args.Length;
            object obj = null;
            LuaFunction func = lua.GetFunction(funcName);
            if (func != null) {
                switch (len) {
                    case 0: { obj = func.Invoke<object>(); break; }
                    case 1: { obj = func.Invoke<object, object>(args[0]); break; }
                    case 2: { obj = func.Invoke<object, object, object>(args[0], args[1]); break; }
                    case 3: { obj = func.Invoke<object, object, object, object>(args[0], args[1], args[2]); break; }
                    case 4: { obj = func.Invoke<object, object, object, object, object>(args[0], args[1], args[2], args[3]); break; }
                    case 5: { obj = func.Invoke<object, object, object, object, object, object>(args[0], args[1], args[2], args[3], args[4]); break; }
                    case 6: { obj = func.Invoke<object, object, object, object, object, object, object>(args[0], args[1], args[2], args[3], args[4], args[5]); break; }
                    case 7: { obj = func.Invoke<object, object, object, object, object, object, object, object>(args[0], args[1], args[2], args[3], args[4], args[5], args[6]); break; }
                    case 8: { obj = func.Invoke<object, object, object, object, object, object, object, object, object>(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7]); break; }
                    case 9: { obj = func.Invoke<object, object, object, object, object, object, object, object, object, object>(args[0], args[1], args[2], args[3], args[4], args[5], args[6], args[7], args[8]); break; }
                }
                // 这个有GC不能用
                //return func.LazyCall(args);
            }
            return obj;
        }

        public void LuaGC() {
            lua.LuaGC(LuaGCOptions.LUA_GCCOLLECT);
        }

        public void Close() {
            if (loop)
            {
                loop.Destroy();
                loop = null;
            }

            lua.Dispose();
            lua = null;
            loader = null;
        }
    }
}