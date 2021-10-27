using System.Collections;
using System.Collections.Generic;
using SUIFW;
using UnityEngine;
using LuaFramework;

public partial class SpineForm : BaseUIForms
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

        AddClickEventListener(m_Btn_Button.gameObject, OnClickClose);
    }

    public override void OnOpen()
    {
        var manager = LuaHelper.GetResManager();
        manager.LoadAsset(R.GetPrefab("Prefabs/Spines/dragon"), OnLoadSpine);
    }

    public override void OnMessage(IMessage message)
    {
        
    }

    void OnClickClose(GameObject go)
    {
        CloseUIForms("SpineForm");
    } 

    void OnLoadSpine(Object obj)
    {
        var go = GameObject.Instantiate(obj) as GameObject;
        go.transform.SetParent(transform, false);
        go.transform.localScale = Vector3.one;
        go.transform.localPosition = Vector3.zero;
    }
}
