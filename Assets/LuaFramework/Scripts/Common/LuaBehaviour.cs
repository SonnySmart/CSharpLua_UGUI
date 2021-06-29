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
        private Dictionary<string, LuaFunction> buttons = new Dictionary<string, LuaFunction>();
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

        /// <summary>
        /// lua通过对象绑定点击事件
        /// </summary>
        public void AddClick(GameObject gameObj, LuaFunction callback)
        {
            if (gameObj == null || callback == null)
                return;
            buttons.Add(gameObj.name, callback);
            AddClick(gameObj, (obj) => {
                callback.Call(obj);
            });
        }

        /// <summary>
        /// lua通过名称绑定点击事件
        /// </summary>
        public void AddClick(string objString, LuaFunction callback)
        {
            Transform transform = UnityHelper.FindTheChild(this.gameObject, objString);
            if (transform == null)
                return;
            AddClick(transform.gameObject, callback);
        }

        public void AddClick(GameObject gameObj, EventTriggerListener.VoidDelegate callback)
        {
            if (gameObj == null || callback == null)
                return;
            EventTriggerListener.Get(gameObj).onClick = callback;
        }

        public void AddClick(string objString, EventTriggerListener.VoidDelegate callback)
        {
            Transform transform = UnityHelper.FindTheChild(this.gameObject, objString);
            if (transform == null)
                return;
            AddClick(transform.gameObject, callback);
        }

        /// <summary>
        /// 删除单击事件
        /// </summary>
        /// <param name="go"></param>
        public void RemoveClick(GameObject gameObj)
        {
            if (gameObj == null)
                return;
            LuaFunction luafunc = null;
            if (buttons.TryGetValue(gameObj.name, out luafunc))
            {
                luafunc.Dispose();
                luafunc = null;
                buttons.Remove(gameObj.name);
            }
        }

        /// <summary>
        /// 清除单击事件
        /// </summary>
        public void ClearClick()
        {
            foreach (var de in buttons) {
                if (de.Value != null) {
                    de.Value.Dispose();
                }
            }
            buttons.Clear();
        }

        //-----------------------------------------------------------------
        protected void OnDestroy()
        {
            ClearClick();

            Util.ClearMemory();
            Debug.Log("~" + name + " was destroy!");
        }

        public void LuaBehaviourTest()
        {
            Debug.Log("LuaBehaviourTest is call .");
        }
    }
}