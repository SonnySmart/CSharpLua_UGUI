using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LuaFramework;
using LuaInterface;
using SUIFW;

namespace LuaFramework
{
    public class Prefab
    {
        /// <summary>
        /// 查找节点 - 名称
        /// 正常查找 Find(transform, "aa") Find(transform, "aa/bb")
        /// 递归查找 Find(transform, "aa") Find(transform, "aa/bb")
        /// </summary>
        public static Transform Find(Transform transform, string n)
        {
            Transform t = null;
            if (transform == null)
                return t;
            // 正常查找
            t = transform.Find(n);
            // 递归查找
            if (t == null)
            {
                int c = transform.childCount;
                for (int i = 0; i < c; i++)
                {
                    Transform local = transform.GetChild(i);
                    t = Find(local, n);
                    if (t != null)
                        break;
                }
            }
            return t;
        }

        /// <summary>
        /// 查找节点 - 名称
        /// </summary>
        public static T Find<T>(Transform transform, string n) where T : UnityEngine.Object
        {
            Transform t = Find(transform, n);
            if (t == null)
                return default(T);
            return t.GetComponent<T>();
        }

        /// <summary>
        /// 获取节点 - 索引
        /// </summary>
        public static Transform GetChild(Transform transform, int index)
        {
            Transform t = null;
            if (transform == null)
                return t;
            t = transform.GetChild(index);
            return t;
        }

        /// <summary>
        /// 获取节点 - 索引
        /// </summary>
        public static T GetChild<T>(Transform transform, int index) where T : UnityEngine.Object
        {
            Transform t = GetChild(transform, index);
            if (t == null)
                return default(T);
            return t.GetComponent<T>();
        }

        /// <summary>
        /// 点击事件
        /// </summary>
        public static void AddClickEventListener(Transform transform, string n, EventTriggerListener.VoidDelegate callback)
        {
            Transform t = Find(transform, n);
            if (t == null)
                return;
            AddClickEventListenerByTransform(t, callback);
        }

        /// <summary>
        /// 点击事件
        /// </summary>
        public static void AddClickEventListenerByTransform(Transform transform, EventTriggerListener.VoidDelegate callback)
        {
            if (transform == null)
                return;
            GameObject gameObject = transform.gameObject;
            EventTriggerListener.Get(gameObject).onClick = callback;
        }

        /// <summary>
        /// 点击事件
        /// </summary>
        public static void AddClickEventListenerByGameObject(GameObject gameObject, EventTriggerListener.VoidDelegate callback)
        {
            if (gameObject == null)
                return;
            EventTriggerListener.Get(gameObject).onClick = callback;
        }
    }
}
