using System.Collections.Generic;
using LuaFramework;

namespace SUIFW
{
    partial class BaseUIForms
    {
        private void OnInit_Wraper()
        {
            ObjectCall("OnInit");
        }

        /// <summary>
        /// 打开窗体
        /// </summary>
        private void OnOpen_Wraper()
        {
            ObjectCall("OnOpen");
        }

        /// <summary>
        /// 重新打开窗体
        /// </summary>
        private void OnReOpen_Wraper()
        {
            ObjectCall("OnReOpen");
        }

        /// <summary>
        /// 关闭窗体
        /// </summary>
        private void OnClose_Wraper() 
        {
            ObjectCall("OnClose");
        }

        /// <summary>
        /// 冻结窗体
        /// </summary>
        private void OnFreeze_Wraper() 
        {
            ObjectCall("OnFreeze");
        }
    }
}