using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using LuaInterface;
using LuaFramework;

namespace LuaFramework {
    public class LuaView : Base, IView, IController {

        /// <summary>
        /// 接收消息回调
        /// </summary>
        public virtual void OnMessage(IMessage message) {}

        /// <summary>
        /// 注册命令
        /// </summary>
        public void RegisterCommand(string messageName, Type commandType)
        {
            Controller.Instance.RegisterCommand(messageName, commandType);
        }

        /// <summary>
        /// 注册view命令
        /// </summary>
        public void RegisterViewCommand(IView view, string[] commandNames)
        {
            Controller.Instance.RegisterViewCommand(view, commandNames);
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        public void ExecuteCommand(IMessage message)
        {
            Controller.Instance.ExecuteCommand(message);
        }

        /// <summary>
        /// 移除命令
        /// </summary>
        public void RemoveCommand(string messageName)
        {
            Controller.Instance.RemoveCommand(messageName);
        }

        /// <summary>
        /// 移除view命令
        /// </summary>
        public void RemoveViewCommand(IView view, string[] commandNames)
        {
            Controller.Instance.RegisterViewCommand(view, commandNames);
        }

        /// <summary>
        /// 是否存在命令
        /// </summary>
        public bool HasCommand(string messageName)
        {
            return Controller.Instance.HasCommand(messageName);
        }
    }
}