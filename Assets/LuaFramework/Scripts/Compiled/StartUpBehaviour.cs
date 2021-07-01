using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LuaFramework;

public class StartUpBehaviour : LuaBehaviour
{
    private void Awake()
    {
        RegisterCommand();

        StartUpSocket();

        StartUpEntry();
    }

    /// <summary>
    /// 注册命令
    /// </summary>
    void RegisterCommand()
    {
        //-----------------关联命令-----------------------
        AppFacade.Instance.RegisterCommand(NotiConst.DISPATCH_MESSAGE, typeof(SocketCommand));
    }

    /// <summary>
    /// 启动Socket
    /// </summary>
    void StartUpSocket()
    {
        AppConst.SocketAddress = "119.45.195.29";
        AppConst.SocketPort = 9001;
        LuaHelper.GetNetManager().SendConnect();
    }

    /// <summary>
    /// 启动游戏实例
    /// </summary>
    void StartUpEntry()
    {
        // 启动实例
        // XAsset/Runtime/Initializer 组件面板中修改
        gameObject.AddLuaComponent(AppConst.luaEntry);
    }
}
