using UnityEngine;

namespace LuaFramework {
    public class AppConst {
        /// <summary>
        /// 编辑器模式
        /// 发布模式配置 XAsset/Runtime/Initializer 组件面板中修改
        /// </summary>
        public static bool development;                             //调试模式-用于内部测试
        /// <summary>
        /// lua字节码,不会用于运行模式
        /// 发布模式配置
        /// </summary>
        public static bool luajit = false;                           //Lua字节码模式-默认关闭 - 安卓测试有问题
        /// <summary>
        /// 判断lua是否从AssetBundle中读取,一般与 '编辑器模式' 相反
        /// 发布模式配置 XAsset/Runtime/Initializer 组件面板中修改
        /// </summary>
        public static bool luaBundle = true;                        //Lua代码AssetBundle模式
        /// <summary>
        /// lua最终启动实例的名称,业务逻辑的启动在这里
        /// Assets\Scripts\Compiled\Game.cs
        /// 发布模式配置 XAsset/Runtime/Initializer 组件面板中修改
        /// </summary>
        public static string luaEntry;

        public const int TimerInterval = 1;
        public const int GameFrameRate = 30;                        //游戏帧频
        public const string AppName = "LuaFramework";               //应用程序名称
        public const string LuaTempDir = "Lua/";                    //临时目录
        public const string AppPrefix = AppName + "_";              //应用程序前缀
        public const string ExtName = ".unity3d";                   //素材扩展名
        public const string Bundles = "Bundles/";
        public static string AssetDir = Bundles + PlatformName;     //素材目录 
        public static string WebUrl = "http://localhost:6688/";     //测试更新地址
        public static int SocketPort = 0;                           //Socket服务器端口
        public static string SocketAddress = string.Empty;          //Socket服务器地址

        public static string FrameworkRoot
        {
            get { return Application.dataPath + "/" + AppName; }
        }

        /// <summary>
        /// 平台名称
        /// </summary>
        public static string PlatformName
        {
            get
            {
#if UNITY_ANDROID
                return "Android";
#elif UNITY_IPHONE
                return "iOS";
#elif UNITY_WEBGL
                return "WebGL";
#elif UNITY_STANDALONE_WIN
                return "Windows";
#elif UNITY_STANDALONE_OSX
                return "OSX";
#else
                return "None";
#endif
            }
        }
    }
}