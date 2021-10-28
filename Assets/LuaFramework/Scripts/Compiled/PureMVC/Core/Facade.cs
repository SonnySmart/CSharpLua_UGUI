/* 
    LuaFramework Code By Jarjin lee
*/

using System;
using System.Collections.Generic;
using UnityEngine;

public class Facade {
    protected IController m_controller;
    protected IModel m_model;
    static GameObject m_GameManager;
    static Dictionary<string, object> m_Managers = new Dictionary<string, object>();

    GameObject AppGameManager {
        get {
            if (m_GameManager == null) {
                m_GameManager = GameObject.Find("GameManager");
            }
            return m_GameManager;
        }
    }

    protected Facade() {
        InitFramework();
    }
    protected virtual void InitFramework() {
        m_controller = Controller.Instance;
        m_model = Model.Instance;
    }

    #region Command
    public virtual void RegisterCommand(string commandName, Type commandType) {
        m_controller.RegisterCommand(commandName, commandType);
    }

    public virtual void RemoveCommand(string commandName) {
        m_controller.RemoveCommand(commandName);
    }

    public virtual bool HasCommand(string commandName) {
        return m_controller.HasCommand(commandName);
    }

    public void RegisterMultiCommand(Type commandType, params string[] commandNames) {
        int count = commandNames.Length;
        for (int i = 0; i < count; i++) {
            RegisterCommand(commandNames[i], commandType);
        }
    }

    public void RemoveMultiCommand(params string[] commandName) {
        int count = commandName.Length;
        for (int i = 0; i < count; i++) {
            RemoveCommand(commandName[i]);
        }
    }

    public void SendMessageCommand(string message, object body = null) {
        m_controller.ExecuteCommand(new Message(message, body));
    }
    #endregion

    #region Proxy
    /// <summary>
    /// 注册代理
    /// </summary>
    public virtual void RegisterProxy(IProxy proxy)
    {
        this.m_model.RegisterProxy(proxy);
    }

    /// <summary>
    /// 删除代理
    /// </summary>
    public virtual IProxy RemoveProxy(string proxyName)
    {
        return this.m_model.RemoveProxy(proxyName);
    }

    /// <summary>
    /// 获取代理
    /// </summary>
    public IProxy RetrieveProxy(string proxyName)
    {
        return this.m_model.RetrieveProxy(proxyName);
    }

    public virtual bool HasProxy(string proxyName)
    {
        return this.m_model.HasProxy(proxyName);
    }
    #endregion

    #region View
    /// <summary>
    /// 注册消息
    /// </summary>
    public void RegisterMessage(IView view, List<string> messages) {
        if (messages == null || messages.Count == 0) return;
        m_controller.RegisterViewCommand(view, messages.ToArray());
    }

    /// <summary>
    /// 移除消息
    /// </summary>
    public void RemoveMessage(IView view, List<string> messages) {
        if (messages == null || messages.Count == 0) return;
        m_controller.RemoveViewCommand(view, messages.ToArray());
    }
    #endregion

    /// <summary>
    /// 添加管理器
    /// </summary>
    public void AddManager(string typeName, object obj) {
        if (!m_Managers.ContainsKey(typeName)) {
            m_Managers.Add(typeName, obj);
        }
    }

    /// <summary>
    /// 添加Unity对象
    /// </summary>
    public T AddManager<T>(string typeName) where T : Component {
        object result = null;
        m_Managers.TryGetValue(typeName, out result);
        if (result != null) {
            return (T)result;
        }
        Component c = AppGameManager.AddComponent<T>();
        m_Managers.Add(typeName, c);
        return default(T);
    }

    /// <summary>
    /// 获取系统管理器
    /// </summary>
    public T GetManager<T>(string typeName) where T : class {
        if (!m_Managers.ContainsKey(typeName)) {
            return default(T);
        }
        object manager = null;
        m_Managers.TryGetValue(typeName, out manager);
        return (T)manager;
    }

    /// <summary>
    /// 删除管理器
    /// </summary>
    public void RemoveManager(string typeName) {
        if (!m_Managers.ContainsKey(typeName)) {
            return;
        }
        object manager = null;
        m_Managers.TryGetValue(typeName, out manager);
        Type type = manager.GetType();
        if (type.IsSubclassOf(typeof(MonoBehaviour))) {
            GameObject.Destroy((Component)manager);
        }
        m_Managers.Remove(typeName);
    }
}
