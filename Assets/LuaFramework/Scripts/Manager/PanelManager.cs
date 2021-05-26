using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using LuaInterface;
using System;

namespace LuaFramework {
    public class PanelManager : Manager {
        private Transform parent;

        Transform Parent {
            get {
                if (parent == null) {
                    GameObject go = GameObject.FindWithTag("GuiCamera");
                    if (go != null) parent = go.transform;
                }
                return parent;
            }
        }

        /// <summary>
        /// ������壬������Դ������
        /// </summary>
        /// <param name="type"></param>
        public void CreatePanel(string name, LuaFunction func = null) {
            CreatePanel(name, (go) => {
                if (func != null) func.Call(go);
            });
        }

        public void CreatePanel(string name, Action<GameObject> func) {
            string assetName = name + "Panel";
            string abName = name.ToLower() + AppConst.ExtName;
            if (Parent.Find(name) != null) return;

#if ASYNC_MODE
            ResManager.LoadPrefab(abName, assetName, delegate(UnityEngine.Object[] objs) {
                if (objs.Length == 0) return;
                GameObject prefab = objs[0] as GameObject;
                if (prefab == null) return;

                GameObject go = Instantiate(prefab) as GameObject;
                go.name = assetName;
                go.layer = LayerMask.NameToLayer("Default");
                go.transform.SetParent(Parent);
                go.transform.localScale = Vector3.one;
                go.transform.localPosition = Vector3.zero;
                AddComponent(go, assetName);

                if (func != null) func.Invoke(go);
                Debug.LogWarning("CreatePanel::>> " + name + " " + prefab);
            });
#else
            GameObject prefab = ResManager.LoadAsset<GameObject>(name, assetName);
            if (prefab == null) return;

            GameObject go = Instantiate(prefab) as GameObject;
            go.name = assetName;
            go.layer = LayerMask.NameToLayer("Default");
            go.transform.SetParent(Parent);
            go.transform.localScale = Vector3.one;
            go.transform.localPosition = Vector3.zero;
            AddComponent(go, assetName);

            if (func != null) func.Invoke(go);
            Debug.LogWarning("CreatePanel::>> " + name + " " + prefab);
#endif
        }

        /// <summary>
        /// �ر����
        /// </summary>
        /// <param name="name"></param>
        public void ClosePanel(string name) {
            var panelName = name + "Panel";
            var panelObj = Parent.Find(panelName);
            if (panelObj == null) return;
            Destroy(panelObj.gameObject);
        }

        private void AddComponent(GameObject gameObject, string assetName)
        {
#if USE_LUA
            //gameObject.AddComponent<LuaBehaviour>();
            var luaState = LuaHelper.GetLuaManager().GetMainState();
            using (var fn = luaState.GetFunction("UnityEngine.addComponent")) {
                string assembly = assetName + ",Assembly-CSharp";
                fn.Call(gameObject, assembly);
            }
#else
            gameObject.AddComponent(Type.GetType(assetName));
#endif
        }
    }
}