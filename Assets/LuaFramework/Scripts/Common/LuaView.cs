using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using LuaInterface;
using LuaFramework;

namespace LuaFramework {
    /// <summary>
    /// 这个类是没有被Lua修复的
    /// 里面添加的任何东西Lua都找不到
    /// </summary>
    public class LuaView : Base, IView {

        /// <summary>
        /// 接收消息回调
        /// </summary>
        public virtual void OnMessage(IMessage message) {}
    }
}