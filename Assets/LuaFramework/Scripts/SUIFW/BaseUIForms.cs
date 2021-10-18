// 基础UI窗体     
// 功能：所有用户UI窗体的父类
// 1：定于四个“UI窗体”的状态
// Display:    显示状态
// Hiding:     隐藏状态(即：不能看见，不能操作)
// Redisplay:  重新显示状态
// Freeze:     冻结状态(即：在其他窗体下面，看见但不能操作)

//using DemoProject;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using LuaFramework;

namespace SUIFW
{
    public class BaseUIForms : LuaBehaviour
    {
        /// <summary>
        /// Lua包装类
        /// </summary>
        private BaseUIForms_LuaWrap LuaWrap = new BaseUIForms_LuaWrap();
        /// <summary>
        /// Lua包装类
        /// </summary>
        private AppFacade_LuaWrap AppFacadeLuaWrap = new AppFacade_LuaWrap();
        /*  字段  */
        //当前(基类)窗口的类型
        private UIType _CurrentUIType=new UIType();

        /// <summary>
        /// 事件关心列表
        /// </summary>
        [HideInInspector]
        private List<string> _AttentionList = new List<string>();

        /*  属性  */
        /// <summary>
        /// 属性_当前UI窗体类型
        /// </summary>
        public UIType CurrentUIType { get { return _CurrentUIType; } }

        public List<string> AttentionList { get { return _AttentionList; }}

        #region 窗体生命周期

        /// <summary>
        /// 初始化窗体
        /// </summary>
        public virtual void OnInit()
        {
            /*
            //是否需要清空“反向切换”
            CurrentUIType.IsClearReverseChange = false;
            //UI窗体类型
            CurrentUIType.UIForms_Type = UIFormsType.Normal;
            //UI窗体显示类型
            CurrentUIType.UIForms_ShowMode = UIFormsShowMode.Normal;
            //UI窗体透明度类型
            CurrentUIType.UIForms_LucencyType = UIFormsLucencyType.Lucency;
            */
            LuaWrap.OnInit(this);
        }

        /// <summary>
        /// 打开窗体
        /// </summary>
        public virtual void OnOpen()
        { 
            LuaWrap.OnOpen(this);
        }

        /// <summary>
        /// 重新打开窗体
        /// </summary>
        public virtual void OnReOpen()
        { 
            LuaWrap.OnReOpen(this);
        }

        /// <summary>
        /// 关闭窗体
        /// </summary>
        public virtual void OnClose()
        { 
            LuaWrap.OnClose(this);
        }

        /// <summary>
        /// 冻结窗体
        /// </summary>
        public virtual void OnFreeze()
        { 
            LuaWrap.OnFreeze(this);
        }

        //初始化
        internal void Init()
        {
            OnInit();
        }

        //页面显示
        internal void Open()
        {
            this.gameObject.SetActive(true);
            if (_CurrentUIType.UIForms_Type == UIFormsType.PopUp 
            || _CurrentUIType.UIForms_Type == UIFormsType.TopUp) 
            {
                //添加UI遮罩处理
                UIMaskMgr.GetInstance().SetMaskWindow(this.gameObject,_CurrentUIType.UIForms_LucencyType);                
            }

            // 自动注册MVC View
            if (AttentionList.Count > 0)
            {                
                AppFacadeLuaWrap.Instance.RegisterMessage(this, AttentionList);
            }
            OnOpen();
        }

        //页面隐藏(不在“栈”集合中)
        internal void Close()
        {
            this.gameObject.SetActive(false);
            if (_CurrentUIType.UIForms_Type == UIFormsType.PopUp
            || _CurrentUIType.UIForms_Type == UIFormsType.TopUp)
            {
                //添加UI遮罩处理
                UIMaskMgr.GetInstance().CancleMaskWindow();
            }

            // 自动销毁MVC View
            if (AttentionList.Count > 0)
            {
                AppFacadeLuaWrap.Instance.RemoveMessage(this, AttentionList);
            }            
            OnClose();
        }

        //页面重新显示
        internal void ReOpen()
        {
            this.gameObject.SetActive(true);
            if (_CurrentUIType.UIForms_Type == UIFormsType.PopUp
            || _CurrentUIType.UIForms_Type == UIFormsType.TopUp)
            {
                //添加UI遮罩处理
                UIMaskMgr.GetInstance().SetMaskWindow(this.gameObject, _CurrentUIType.UIForms_LucencyType);
            }

            OnReOpen();
        }

        //页面冻结(还在“栈”集合中)
        internal void Freeze()
        {
            this.gameObject.SetActive(true);
            OnFreeze();
        } 
        #endregion

        #region 给子类封装的方法
        /// <summary>
        /// 注册按钮对象事件
        /// </summary>
        /// <param name="strButtonName">(UI预设)需要注册事件的按钮名称</param>
        /// <param name="delHandle">([委托类型]按钮的注册方法)</param>
        public void RegisteButtonObjectEvent(string strButtonName, EventTriggerListener.VoidDelegate delHandle)
        {
            Transform transform = UnityHelper.FindTheChild(this.gameObject, strButtonName);
            if (transform == null)
                return;

            RegisteButtonObjectEvent(transform.gameObject, delHandle);
        }

        public void RegisteButtonObjectEvent(GameObject gameObj, EventTriggerListener.VoidDelegate delHandle)
        {
            if (gameObj == null)
                return;

            EventTriggerListener.Get(gameObj).onClick = delHandle;
        }

        /// <summary>
        /// 关闭与返回UI窗体  
        /// </summary>
        public void CloseOrReturnUIForms()
        {
            string strUIFomrsName = null;
            int intPosition = -1;

            strUIFomrsName = GetType().ToString();
            intPosition = strUIFomrsName.IndexOf('.');
            if (intPosition != -1)
            {
                strUIFomrsName = strUIFomrsName.Substring(intPosition + 1);
            }
            UIManager.GetInstance().CloseOrReturnUIForms(strUIFomrsName);
        }

        /// <summary>
        /// 显示UI窗体
        /// </summary>
        /// <param name="formType">UI窗体的类型</param>
        public void ShowUIForms(System.Type formType)
        {
            UIManager.GetInstance().ShowUIForms(formType);
        }

        /// <summary>
        /// 打开UI窗体
        /// </summary>
        /// <param name="strUIFormsName"></param>
        public void ShowUIForms(string strUIFormsName)
        {
            UIManager.GetInstance().ShowUIForms(strUIFormsName);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="strMsgType">消息大类</param>
        /// <param name="strSmallClassType">消息小类</param>
        /// <param name="strMsgContent">消息内容</param>
        protected void SendMessage(string strMsgType, string strSmallClassType, object objMsgContent)
        {
            KeyValuesUpdate kv = new KeyValuesUpdate(strSmallClassType, objMsgContent);
            MessageCenter.SendMessage(strMsgType, kv);
        }

        /// <summary>
        /// 显示语言信息
        /// </summary>
        /// <param name="info"></param>
        protected string Show(string info)
        {
            return LauguageMgr.GetInstance().ShowText(info);
        }

        #endregion

        public void BaseUIFormsTest()
        {
            Debug.Log("BaseUIFormsTest is call .");
        }

    }//Class_end
}
