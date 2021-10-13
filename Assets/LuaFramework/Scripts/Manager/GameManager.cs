using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using LuaInterface;
using System.Reflection;
using System.IO;
using UnityEngine.Networking;
using libx;

namespace LuaFramework {
    public class GameManager : Manager {
        protected static bool initialize = false;
        private List<string> downloadFiles = new List<string>();

        /// <summary>
        /// 初始化游戏管理器
        /// </summary>
        void Awake() {
            Util.CalcTime("热更新启动开始");
            Init();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        void Init() {
            DontDestroyOnLoad(gameObject);  //防止销毁自己

            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            Application.targetFrameRate = AppConst.GameFrameRate;

            NetworkMonitor.Instance.onReachabilityChanged += OnReachablityChanged; 

            StartUpdate();
        }

        private void OnReachablityChanged(NetworkReachability reachability)
        {
            if (reachability == NetworkReachability.NotReachable)
            { 
                OnMessage("网络错误");
            } 
        }

        private void OnProgress(float progress)
        {
            //progressBar.value = progress;
        }

        private void OnMessage(string msg)
        {
            //progressText.text = msg;
            Debug.Log(msg);
        }

        public void StartUpdate()
        {
#if UNITY_EDITOR
            if (Assets.development)
            {
                StartCoroutine(OnResourceInited());
                return;
            }
#endif
            OnMessage("正在获取版本信息...");
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                MessageBox.Show("提示", "请检查网络连接状态", retry =>
                {
                    if (retry)
                    {
                        StartUpdate();
                    }
                    else
                    {
                        Quit();
                    }
                }, "重试", "退出");
            }
            else
            {
                DownloadVersions();
            }
        }

        void DownloadVersions()
        {
            Assets.DownloadVersions(error =>
            {
                if (!string.IsNullOrEmpty(error))
                {
                    MessageBox.Show("提示", string.Format("获取服务器版本失败：{0}", error), retry =>
                    {
                        if (retry)
                        {
                            StartUpdate();
                        }
                        else
                        {
                            Quit();
                        }
                    });
                }
                else
                {
                    DownloadAll();
                }
            });
        }

        void DownloadAll()
        {
            Downloader handler;
            // 按分包下载版本更新，返回true的时候表示需要下载，false的时候，表示不需要下载
            if (Assets.DownloadAll(Assets.patches4Init, out handler))
            {
                var totalSize = handler.size;
                var tips = string.Format("发现内容更新，总计需要下载 {0} 内容", Downloader.GetDisplaySize(totalSize));
                MessageBox.Show("提示", tips, download =>
                {
                    if (download)
                    {
                        handler.onUpdate += delegate(long progress, long size, float speed)
                        {
                            //刷新界面
                            OnMessage(string.Format("下载中...{0}/{1}, 速度：{2}",
                                Downloader.GetDisplaySize(progress),
                                Downloader.GetDisplaySize(size),
                                Downloader.GetDisplaySpeed(speed)));
                            OnProgress(progress * 1f / size);
                        };
                        handler.onFinished += OnComplete;
                        handler.Start();
                    }
                    else
                    {
                        Quit();
                    }
                }, "下载", "退出");
            }
            else
            {
                OnComplete(); 
            }
        }

        void OnComplete()
        {
            OnProgress(1);
            //version.text = Assets.currentVersions.ver;
            OnMessage("更新完成");
            StartCoroutine(OnResourceInited());
        }

        /// <summary>
        /// 资源初始化结束
        /// </summary>
        IEnumerator OnResourceInited() {
            yield return null;
            Util.CalcTime("热更新启动完成");
            this.OnInitialize();
        }

        void OnInitialize() {
#if USE_LUA
            Util.CalcTime("lua启动开始");
            LuaManager.InitStart(); // use 2.1s时间...这里太久
            Util.CalcTime("lua启动完成");
#endif
            // 这里启动最终逻辑
            gameObject.AddLuaComponent(typeof(StartUpBehaviour));
            
            initialize = true;
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        void OnDestroy() {
            // MessageBox 销毁
            MessageBox.Dispose();
            if (NetManager != null) {
                NetManager.Unload();
            }
            if (LuaManager != null) {
                LuaManager.Close();
            }
            Debug.Log("~GameManager was destroyed");
        }

        void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}