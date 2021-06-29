using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using LuaInterface;
using UObject = UnityEngine.Object;
using UnityEngine.Networking;
using libx;

public class AssetBundleInfo {
    public AssetBundle m_AssetBundle;
    public int m_ReferencedCount;

    public AssetBundleInfo(AssetBundle assetBundle) {
        m_AssetBundle = assetBundle;
        m_ReferencedCount = 0;
    }
}

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