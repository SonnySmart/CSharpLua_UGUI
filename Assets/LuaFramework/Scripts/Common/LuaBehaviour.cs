using UnityEngine;
using LuaInterface;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine.Events;

namespace LuaFramework {
    public class LuaBehaviour : View {
        private string data = null;
        private Dictionary<string, LuaFunction> buttons = new Dictionary<string, LuaFunction>();

        public LuaTable Table { get; private set; }
        public string LuaClass;

        public void Bind(LuaTable table) {
            Table = table;
        }

        public void Bind(LuaTable table, string luaClass) {
            Table = table;
            LuaClass = luaClass;
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