using UnityEngine;
using LuaInterface;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine.Events;
using SUIFW;

namespace LuaFramework {
    public partial class LuaBehaviour : LuaView {
        /// <summary>
        /// Lua回调集合
        /// </summary>
        private List<LuaFunction> LuaFunctions = new List<LuaFunction>();

        #region Cs2Lua
        /// <summary>
        /// Cs2Lua Update
        /// </summary>
        private static readonly YieldInstruction[] updateYieldInstructions_ = new YieldInstruction[] { 
            null, new WaitForFixedUpdate(), new WaitForEndOfFrame() 
        };
        /// <summary>
        /// Cs2Lua Lua表
        /// </summary>
        [HideInInspector]
        public LuaTable Table { get; private set; }
        /// <summary>
        /// Cs2Lua Lua类名称
        /// </summary>
        [SerializeField]
        public string LuaClass;
        /// <summary>
        /// Cs2Lua Lua序列化数据
        /// </summary>
        [HideInInspector]
        public string SerializeData;
        /// <summary>
        /// Cs2Lua Lua序列化对象
        /// </summary>
        [HideInInspector]
        public UnityEngine.Object[] SerializeObjects;

        public void Bind(LuaTable table) {
            Table = table;
        }

        public void Bind(LuaTable table, string luaClass) {
            Table = table;
            LuaClass = luaClass;
        }

        public void Bind(string luaClass, string serializeData, UnityEngine.Object[] serializeObjects) {
            LuaClass = luaClass;
            SerializeData = serializeData;
            SerializeObjects = serializeObjects;
        }
        
        public void RegisterUpdate(int instructionIndex, LuaFunction updateFn) {
            StartCoroutine(StartUpdate(updateFn, updateYieldInstructions_[instructionIndex]));
        }

        private IEnumerator StartUpdate(LuaFunction updateFn, YieldInstruction yieldInstruction) {
            while (true) {
                yield return yieldInstruction;
                updateFn.Call(Table);
            }
        }

        /// <summary>
        /// 调用lua对象方法
        /// </summary>
        /// <param name="function"> 方法名称 </param>
        /// <param name="args"> 参数 </param>
        /// <returns> 返回值 </returns>
        public object ObjectInvoke(string function, params object[] args)
        {
            return LuaHelper.ObjectInvoke(Table, function, args);
        }

        /// <summary>
        /// 调用lua对象方法
        /// </summary>
        /// <param name="function"> 方法名称 </param>
        /// <param name="args"> 参数 </param>
        public void ObjectCall(string function, params object[] args)
        {
            LuaHelper.ObjectCall(Table, function, args);
        }
        #endregion

        #region 生命周期相关
        /// <summary>
        /// 子类Awake函数不能重写|不能重写|不能重写
        /// </summary>
        private void Awake()
        {
#if !USE_LUA
            if (!string.IsNullOrEmpty(LuaClass))
            {
                gameObject.AddLuaComponent(LuaClass);
            }
            // 进行UI绑定
            InitializeComponent();
            // view消息注册
            InitializeLuaView();
#endif

#if USE_LUA
            // 为空进行绑定 & 绑定的时候会调用Awake
            if (!string.IsNullOrEmpty(LuaClass))
            {
                if (Table == null)
                {
                    Table = LuaHelper.GetLuaManager().BindLua(this);
                }
            }
#endif
        }

        private void OnEable()
        {
            ObjectCall("OnEable");
        }

        private void Start()
        {
            ObjectCall("Start");
        }
        #endregion

        #region mvc相关
        /// <summary>
        /// 事件关心列表
        /// </summary>
        [HideInInspector]
        private List<string> _AttentionList = new List<string>();
        public List<string> AttentionList { get { return _AttentionList; }}

        /// <summary>
        /// view初始化 视图消息可在这里注册
        /// add AttentionList
        /// </summary>
        protected virtual void InitializeLuaView() {}
        #endregion

        #region UI 查找相关
        /// <summary>
        /// 获取UI控件 - 自动绑定功能
        /// </summary>
        protected virtual void InitializeComponent() {}

        /// <summary>
        /// 查找节点 - 名称
        /// </summary>
        public Transform Find(string n)
        {
            return Prefab.Find(transform, n);
        }

        /// <summary>
        /// 获取节点 - 索引
        /// </summary>
        public Transform GetChild(int index)
        {
            return Prefab.GetChild(transform, index);
        }

        /// <summary>
        /// 点击事件
        /// </summary>
        public void AddClickEventListener(string n, EventTriggerListener.VoidDelegate callback)
        {
            Prefab.AddClickEventListener(transform, n, callback);
        }

        /// <summary>
        /// 点击事件 lua function
        /// </summary>
        public void AddClickEventListener(string n, LuaFunction callback)
        {
            AddClickEventListener(n, (go) => {
                callback.Call(go);
            });
            LuaFunctions.Add(callback);
        }

        /// <summary>
        /// 点击事件
        /// </summary>
        public void AddClickEventListener(Transform transform, EventTriggerListener.VoidDelegate callback)
        {
            Prefab.AddClickEventListenerByTransform(transform, callback);
        }

        /// <summary>
        /// 点击事件 lua function
        /// </summary>
        public void AddClickEventListener(Transform transform, LuaFunction callback)
        {
            AddClickEventListener(transform, (go) => {
                callback.Call(go);
            });
            LuaFunctions.Add(callback);
        }

        /// <summary>
        /// 点击事件
        /// </summary>
        public void AddClickEventListener(GameObject gameObject, EventTriggerListener.VoidDelegate callback)
        {
            Prefab.AddClickEventListenerByGameObject(gameObject, callback);
        }

        /// <summary>
        /// 点击事件 lua function
        /// </summary>
        public void AddClickEventListener(GameObject gameObject, LuaFunction callback)
        {
            AddClickEventListener(gameObject, (go) => {
                callback.Call(go);
            });
            LuaFunctions.Add(callback);
        }

        /// <summary>
        /// 删除点击事件
        /// </summary>
        public void RemoveClickEventListener()
        {
            foreach (var fn in LuaFunctions)
            {
                fn.Dispose();
            }
            LuaFunctions.Clear();
        }
        #endregion

        //-----------------------------------------------------------------
        protected void OnDestroy()
        {
            RemoveClickEventListener();
            Debug.Log("~" + name + " was destroy!");
        }

        public void LuaBehaviourTest()
        {
            Debug.Log("LuaBehaviourTest is call .");
        }
    }
}