using System.Collections;
using System.Collections.Generic;
using SUIFW;
using UnityEngine;

public class SpineForm : BaseUIForms
{
    public override void OnInit()
    {
        // //是否需要清空“反向切换”
        // CurrentUIType.IsClearReverseChange = false;
        // //UI窗体类型
        // CurrentUIType.UIForms_Type = UIFormsType.Normal;
        // //UI窗体显示类型
        // CurrentUIType.UIForms_ShowMode = UIFormsShowMode.Normal;
        // //UI窗体透明度类型
        // CurrentUIType.UIForms_LucencyType = UIFormsLucencyType.Lucency;
    }

    public override void OnOpen()
    {
        base.OnOpen();
    }

    public override void OnClose()
    {
        base.OnClose();
    }

    public override void OnMessage(IMessage message)
    {
        base.OnMessage(message);
    }
}
