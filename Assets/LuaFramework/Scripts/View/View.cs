using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using LuaInterface;
using LuaFramework;

public class View : Base, IView {

    /// <summary>
    /// 接收消息回调
    /// </summary>
    public virtual void OnMessage(IMessage message) {
    }
}
