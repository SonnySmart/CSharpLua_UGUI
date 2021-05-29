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
        /*  字段  */
        //当前(基类)窗口的类型
        private UIType _CurrentUIType=new UIType();

        /*  属性  */
        /// <summary>
        /// 属性_当前UI窗体类型
        /// </summary>
        public UIType CurrentUIType
        {
            set
            {
                _CurrentUIType = value;
            }

            get
            {
                return _CurrentUIType;
            }
        }

        #region 窗体生命周期

        /// <summary>
        /// 初始化窗体
        /// </summary>
        public virtual void OnInit()
        {
            //是否需要清空“反向切换”
            CurrentUIType.IsClearReverseChange = false;
            //UI窗体类型
            CurrentUIType.UIForms_Type = UIFormsType.Normal;
            //UI窗体显示类型
            CurrentUIType.UIForms_ShowMode = UIFormsShowMode.Normal;
            //UI窗体透明度类型
            CurrentUIType.UIForms_LucencyType = UIFormsLucencyType.Lucency;
        }

        /// <summary>
        /// 打开窗体
        /// </summary>
        public virtual void OnOpen() {}

        /// <summary>
        /// 重新打开窗体
        /// </summary>
        public virtual void OnReOpen() {}

        /// <summary>
        /// 关闭窗体
        /// </summary>
        public virtual void OnClose() {}

        /// <summary>
        /// 冻结窗体
        /// </summary>
        public virtual void OnFreeze() {}

        /// <summary>
        /// 调用lua方法
        /// </summary>
        /// <param name="function"> 方法名称 </param>
        protected void CallLuaFunction(string function)
        {
#if USE_LUA
            if (Table == null)
                return;

            using (var fn = Table.GetLuaFunction(function))
            {
                if (fn != null)
                    fn.Call(Table);
            }
#endif
        }

        //初始化
        internal void Init()
        {
            CallLuaFunction("OnInit");
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

            CallLuaFunction("OnOpen");
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

            CallLuaFunction("OnClose");
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

            CallLuaFunction("OnReOpen");
            OnReOpen();
        }

        //页面冻结(还在“栈”集合中)
        internal void Freeze()
        {
            this.gameObject.SetActive(true);

            CallLuaFunction("OnFreeze");
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
        protected void CloseOrReturnUIForms()
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
        /// 打开UI窗体
        /// </summary>
        /// <param name="strUIFormsName"></param>
        protected void ShowUIForms(string strUIFormsName)
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
