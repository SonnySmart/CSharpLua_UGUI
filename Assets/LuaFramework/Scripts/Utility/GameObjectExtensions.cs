using System;
using System.Reflection;
using UnityEngine;

namespace LuaFramework 
{
    public static class GameObjectExtensions
    {
        /// <summary>
        /// 添加Lua组件
        /// </summary>
        public static T AddLuaComponent<T>(this GameObject gameObject, string className)
        {
            AddLuaComponent(gameObject, className);
            return gameObject.GetComponent<T>();
        }

        /// <summary>
        /// 添加Lua组件
        /// </summary>
        public static void AddLuaComponent(this GameObject gameObject, Type componentType)
        {
            string fullName = componentType.FullName;
            AddLuaComponent(gameObject, fullName);
        }

        /// <summary>
        /// 添加Lua组件
        /// </summary>
        public static void AddLuaComponent(this GameObject gameObject, string className)
        {
            if (gameObject == null)
                return;

            if (string.IsNullOrEmpty(className))
                return;

            string assembly = className;
            if (!assembly.Contains(","))
                assembly += ",Assembly-CSharp";
#if USE_LUA
            var luaState = LuaHelper.GetLuaManager().GetMainState();
            using (var fn = luaState.GetFunction("UnityEngine.addComponent"))
            {
                fn.Call(gameObject, assembly);
            }
#else
            try {
                Type type = Type.GetType(assembly);
                if (type == null)
                {
                    type = Assembly.GetExecutingAssembly().GetType(className);
                }
                gameObject.AddComponent(type);
            }
            catch (Exception ex) {
                Debug.LogException(ex);
            }
#endif
        }
    }
}