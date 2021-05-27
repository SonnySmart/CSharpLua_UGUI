using UnityEngine;
using LuaInterface;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine.Events;

namespace LuaFramework {
    public class LuaBehaviour : View {
        private Dictionary<string, LuaFunction> buttons = new Dictionary<string, LuaFunction>();
        // Cs2Lua
        private static readonly YieldInstruction[] updateYieldInstructions_ = new YieldInstruction[] { null, new WaitForFixedUpdate(), new WaitForEndOfFrame() };
        public LuaTable Table { get; private set; }
        public string LuaClass;
        public string SerializeData;
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

        /* 不对头这里挂载的时候对象都没初始化完成
        private void Awake() {
            if (!string.IsNullOrEmpty(LuaClass)) {
                if (Table == null) {
                    Table = LuaHelper.GetLuaManager().BindLua(this);
                } else {
                    using (var fn = Table.GetLuaFunction("Awake")) {
                        if (fn != null) fn.Call(Table);
                    }
                }
            }
        }
        */

        private void Start() {
            using (var fn = Table.GetLuaFunction("Start")) {
                if (fn != null) fn.Call(Table);
            }
        }

        /*
        protected void Awake() {
            Util.CallMethod(name, "Awake", gameObject);
        }

        protected void Start() {
            Util.CallMethod(name, "Start");
        }

        protected void OnClick() {
            Util.CallMethod(name, "OnClick");
        }

        protected void OnClickEvent(GameObject go) {
            Util.CallMethod(name, "OnClick", go);
        }
        */

        /// <summary>
        /// 添加单击事件
        /// </summary>
        public void AddClick(GameObject go, LuaFunction luafunc) {
            if (go == null || luafunc == null) return;
            buttons.Add(go.name, luafunc);
            AddClick(go, (GameObject obj) => {
                luafunc.Call(obj);
            });
        }

        public void AddClick(GameObject go, UnityAction<GameObject> luafunc) {
            if (go == null || luafunc == null) return;
            go.GetComponent<Button>().onClick.AddListener(
                delegate() {
                    luafunc.Invoke(go);
                }
            );
        }

        /// <summary>
        /// 删除单击事件
        /// </summary>
        /// <param name="go"></param>
        public void RemoveClick(GameObject go) {
            if (go == null) return;
            LuaFunction luafunc = null;
            if (buttons.TryGetValue(go.name, out luafunc)) {
                luafunc.Dispose();
                luafunc = null;
                buttons.Remove(go.name);
            }
        }

        /// <summary>
        /// 清除单击事件
        /// </summary>
        public void ClearClick() {
            foreach (var de in buttons) {
                if (de.Value != null) {
                    de.Value.Dispose();
                }
            }
            buttons.Clear();
        }

        //-----------------------------------------------------------------
        protected void OnDestroy() {
            ClearClick();
#if ASYNC_MODE
            string abName = name.ToLower().Replace("panel", "");
            ResManager.UnloadAssetBundle(abName + AppConst.ExtName);
#endif
            Util.ClearMemory();
            Debug.Log("~" + name + " was destroy!");
        }
    }
}