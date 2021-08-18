using System;
using LuaInterface;
using UObject = UnityEngine.Object;
using libx;
using UnityEngine;

namespace LuaFramework {

    public class ResourceManager : Manager {

        /// <summary>
        /// 同步加载
        /// </summary>
        public void LoadAsset(string asset, Action<UObject> callback)
        {            
            var request = Assets.LoadAsset(asset, typeof(UObject));
            if (callback != null)
                callback(request.asset);
        }

        /// <summary>
        /// 同步加载
        /// </summary>
        public void LoadAsset(string asset, LuaFunction callback)
        {
            var request = Assets.LoadAsset(asset, typeof(UObject));
            if (callback != null)
            {
                callback.Call<UObject>(request.asset);
                callback.Dispose();
                callback = null;
            }
        }

        /// <summary>
        /// 同步加载
        /// </summary>
        public T LoadAsset<T>(string asset) where T : UObject
        {
            var request = Assets.LoadAsset(asset, typeof(UObject));
            return request.asset as T;
        }

        /// <summary>
        /// 同步加载
        /// </summary>
        [LuaByteBuffer]
        public byte []LoadAsset(string asset)
        {
            TextAsset ta = LoadAsset<TextAsset>(asset);
            return ta == null ? null : ta.bytes;
        }

        /// <summary>
        /// 异步加载
        /// </summary>
        public void LoadAssetAsync(string asset, Action<UObject> callback)
        {
            var request = Assets.LoadAssetAsync(asset, typeof(UObject));
            request.completed += delegate
            {
                if (callback != null)
                    callback(request.asset);
            };
        }

        /// <summary>
        /// 异步加载
        /// </summary>
        public void LoadAssetAsync(string asset, LuaFunction callback)
        {
            var request = Assets.LoadAssetAsync(asset, typeof(UObject));
            request.completed += delegate
            {
                if (callback != null)
                {
                    callback.Call<UObject>(request.asset);
                    callback.Dispose();
                    callback = null;
                }
            };
        }

    }
}