using System;
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

            string assembly = className + ",Assembly-CSharp";
#if USE_LUA
            var luaState = LuaHelper.GetLuaManager().GetMainState();
            using (var fn = luaState.GetFunction("UnityEngine.addComponent"))
            {
                fn.Call(gameObject, assembly);
            }
#else
            gameObject.AddComponent(Type.GetType(className));
#endif
        }
    }
}