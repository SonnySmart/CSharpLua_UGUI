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
                if (func != null) {
                    func.Call(go);
                    func.Dispose();
                    func = null;
                }
            });
        }

        public void CreatePanel(string name, Action<GameObject> func) {
            string assetName = name;
            string abName = name.ToLower() + AppConst.ExtName;
            if (Parent.Find(name) != null) return;

            GameObject prefab = ResManager.LoadAsset<GameObject>(R.GetPrefab(assetName));
            if (prefab == null) return;

            GameObject go = Instantiate(prefab) as GameObject;
            go.name = assetName;
            go.layer = LayerMask.NameToLayer("Default");
            go.transform.SetParent(Parent);
            go.transform.localScale = Vector3.one;
            go.transform.localPosition = Vector3.zero;
            AddComponent(go, assetName);

            if (func != null) func.Invoke(go);
            Debug.Log("CreatePanel::>> " + name + " " + prefab);
        }

        /// <summary>
        /// �ر����
        /// </summary>
        /// <param name="name"></param>
        public void ClosePanel(string name) {
            var panelName = name + "Form";
            var panelObj = Parent.Find(panelName);
            if (panelObj == null) return;
            Destroy(panelObj.gameObject);
        }

        /// <summary>
        /// 根据运行环境添加Form组件
        /// </summary>
        public T AddComponent<T>(GameObject gameObject, string component)
        {
            AddComponent(gameObject, component);
            return gameObject.GetComponent<T>();
        }

        /// <summary>
        /// 根据运行环境添加Form组件
        /// </summary>
        public void AddComponent(GameObject gameObject, Type component)
        {
            string fullName = component.FullName;
            AddComponent(gameObject, fullName);
        }

        /// <summary>
        /// 根据运行环境添加Form组件
        /// </summary>
        public void AddComponent(GameObject gameObject, string component)
        {
            if (gameObject == null)
                return;

            if (string.IsNullOrEmpty(component))
                return;

            string assembly = component + ",Assembly-CSharp";
#if USE_LUA
            var luaState = LuaHelper.GetLuaManager().GetMainState();
            using (var fn = luaState.GetFunction("UnityEngine.addComponent")) {
                fn.Call(gameObject, assembly);
            }
#else
            gameObject.AddComponent(Type.GetType(component));
#endif
        }
    }
}