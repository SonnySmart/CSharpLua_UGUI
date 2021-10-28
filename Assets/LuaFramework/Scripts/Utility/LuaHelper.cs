using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using LuaInterface;
using System;

namespace LuaFramework {
    public static class LuaHelper {

        /// <summary>
        /// getType
        /// </summary>
        /// <param name="classname"></param>
        /// <returns></returns>
        public static System.Type GetType(string classname) {
            Assembly assb = Assembly.GetExecutingAssembly();  //.GetExecutingAssembly();
            System.Type t = null;
            t = assb.GetType(classname); ;
            if (t == null) {
                t = assb.GetType(classname);
            }
            return t;
        }

        /// <summary>
        /// 面板管理器
        /// </summary>
        public static PanelManager GetPanelManager() {
            return AppFacade.Instance.GetManager<PanelManager>(ManagerName.Panel);
        }

        /// <summary>
        /// 资源管理器
        /// </summary>
        public static ResourceManager GetResManager() {
            return AppFacade.Instance.GetManager<ResourceManager>(ManagerName.Resource);
        }

        /// <summary>
        /// 网络管理器
        /// </summary>
        public static NetworkManager GetNetManager() {
            return AppFacade.Instance.GetManager<NetworkManager>(ManagerName.Network);
        }

        /// <summary>
        /// 音乐管理器
        /// </summary>
        public static SoundManager GetSoundManager() {
            return AppFacade.Instance.GetManager<SoundManager>(ManagerName.Sound);
        }

        /// <summary>
        /// Lua管理器
        /// </summary>
        public static LuaManager GetLuaManager() {
            return AppFacade.Instance.GetManager<LuaManager>(ManagerName.Lua);
        }

        /// <summary>
        /// 状态机管理器
        /// </summary>
        public static FsmManager GetFsmManager() {
            return AppFacade.Instance.GetManager<FsmManager>(ManagerName.Fsm);
        }

        /// <summary>
        /// pbc/pblua函数回调
        /// </summary>
        /// <param name="func"></param>
        public static void OnCallLuaFunc(LuaByteBuffer data, LuaFunction func) {
            if (func != null) func.Call(data);
            Debug.LogWarning("OnCallLuaFunc length:>>" + data.buffer.Length);
        }

        /// <summary>
        /// cjson函数回调
        /// </summary>
        /// <param name="data"></param>
        /// <param name="func"></param>
        public static void OnJsonCallFunc(string data, LuaFunction func) {
            Debug.LogWarning("OnJsonCallback data:>>" + data + " lenght:>>" + data.Length);
            if (func != null) func.Call(data);
        }

        /// <summary>
        /// 调用lua方法
        /// </summary>
        /// <param name="function"> 方法名称 </param>
        /// <param name="args"> 参数 </param>
        /// <returns> 返回值 </returns>
        public static object Invoke(string function, params object[] args) {
            object obj = null;
#if USE_LUA
            obj = GetLuaManager().Invoke(function, args);
#endif
            return obj;
        }

        public static object InvokeModule(string module, string function, params object[] args) {
            object obj = null;
#if USE_LUA
            string fn = string.Format("{0}.{1}", module, function);
            obj = Invoke(fn, args);
#endif
            return obj;
        }

        /// <summary>
        /// 调用lua对象方法
        /// </summary>
        /// <param name="table"> lua对象 </param>
        /// <param name="function"> 方法名称 </param>
        /// <param name="args"> 参数 </param>
        /// <returns> 返回值 </returns>
        public static object ObjectInvoke(LuaTable table, string function, params object[] args) {
            throw new NotImplementedException("ObjectInvoke not imp");
        }

        public static void ObjectCall(LuaTable table, string function, params object[] args) {
#if USE_LUA
            GetLuaManager().ObjectCall(table, function, args);
#endif
        }
    }
}