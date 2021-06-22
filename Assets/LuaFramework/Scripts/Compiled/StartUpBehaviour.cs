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

        StartUpGame();
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
    void StartUpGame()
    {
        // 启动实例可以在这里修改
        LuaHelper.GetPanelManager().AddComponent(gameObject, "Game");
    }
}
