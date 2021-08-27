using UnityEngine;
using LuaInterface;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine.Events;
using SUIFW;

namespace LuaFramework {
    public class LuaBehaviour : View {
        private List<LuaFunction> luaCallbacks = new List<LuaFunction>();
        // Cs2Lua
        private static readonly YieldInstruction[] updateYieldInstructions_ = new YieldInstruction[] { null, new WaitForFixedUpdate(), new WaitForEndOfFrame() };
        [HideInInspector]
        public LuaTable Table { get; private set; }
        public string LuaClass;
        [HideInInspector]
        public string SerializeData;
        [HideInInspector]
        public UnityEngine.Object[] SerializeObjects;

        public void Bind(LuaTable table) {
            Table = table;
        }

        public void Bind(LuaTable table, string luaClass) {
            Table = table;
            LuaClass = luaClass;
        }

        public void Bind(string luaClass, string serializeData, UnityEngine.Object[] serializeObjects) {
            LuaClass = luaClass;
            SerializeData = serializeData;
            SerializeObjects = serializeObjects;
        }
        
        public void RegisterUpdate(int instructionIndex, LuaFunction updateFn) {
            StartCoroutine(StartUpdate(updateFn, updateYieldInstructions_[instructionIndex]));
        }

        private IEnumerator StartUpdate(LuaFunction updateFn, YieldInstruction yieldInstruction) {
            while (true) {
                yield return yieldInstruction;
                updateFn.Call(Table);
            }
        }

        private void Start()
        {
#if USE_LUA
            if (Table == null) 
                return;
            using (var fn = Table.GetLuaFunction("Start"))
            {
                if (fn != null)
                    fn.Call(Table);
            }
#endif
        }

        /// <summary>
        /// 调用lua方法
        /// </summary>
        /// <param name="function"> 方法名称 </param>
        protected void CallLuaFunction(string function)
        {
#if USE_LUA
            if (Table == null)
                return;

            using (var fn = Table.GetLuaFunction(function))
            {
                if (fn != null)
                    fn.Call(Table);
            }
#endif
        }

        #region UI 查找相关
        /// <summary>
        /// 查找节点 - 名称
        /// </summary>
        public Transform Find(string n)
        {
            return Prefab.Find(transform, n);
        }

        /// <summary>
        /// 获取节点 - 索引
        /// </summary>
        public Transform GetChild(int index)
        {
            return Prefab.GetChild(transform, index);
        }

        /// <summary>
        /// 点击事件
        /// </summary>
        public void AddClickEventListener(string n, EventTriggerListener.VoidDelegate callback)
        {
            Prefab.AddClickEventListener(transform, n, callback);
        }

        /// <summary>
        /// 点击事件 lua function
        /// </summary>
        public void AddClickEventListener(string n, LuaFunction callback)
        {
            AddClickEventListener(n, (go) => {
                callback.Call(go);
            });
            luaCallbacks.Add(callback);
        }

        /// <summary>
        /// 点击事件
        /// </summary>
        public void AddClickEventListener(Transform transform, EventTriggerListener.VoidDelegate callback)
        {
            Prefab.AddClickEventListenerByTransform(transform, callback);
        }

        /// <summary>
        /// 点击事件 lua function
        /// </summary>
        public void AddClickEventListener(Transform transform, LuaFunction callback)
        {
            AddClickEventListener(transform, (go) => {
                callback.Call(go);
            });
            luaCallbacks.Add(callback);
        }

        /// <summary>
        /// 点击事件
        /// </summary>
        public void AddClickEventListener(GameObject gameObject, EventTriggerListener.VoidDelegate callback)
        {
            Prefab.AddClickEventListenerByGameObject(gameObject, callback);
        }

        /// <summary>
        /// 点击事件 lua function
        /// </summary>
        public void AddClickEventListener(GameObject gameObject, LuaFunction callback)
        {
            AddClickEventListener(gameObject, (go) => {
                callback.Call(go);
            });
            luaCallbacks.Add(callback);
        }

        /// <summary>
        /// 删除点击事件
        /// </summary>
        public void RemoveClickEventListener()
        {
            foreach (var fn in luaCallbacks)
            {
                fn.Dispose();
            }
            luaCallbacks.Clear();
        }
        #endregion

        //-----------------------------------------------------------------
        protected void OnDestroy()
        {
            RemoveClickEventListener();

            Util.ClearMemory();
            Debug.Log("~" + name + " was destroy!");
        }

        public void LuaBehaviourTest()
        {
            Debug.Log("LuaBehaviourTest is call .");
        }
    }
}