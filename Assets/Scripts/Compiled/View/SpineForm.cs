using System.Collections;
using System.Collections.Generic;
using SUIFW;
using UnityEngine;
using LuaFramework;

public partial class SpineForm : BaseUIForms
{
    private GameObject m_GoSpine;

    protected override void InitializeLuaView()
    {
        AttentionList.Add(MessageConst.SpineFormMessageTest);
    }

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
        //var manager = LuaHelper.GetResManager();
        var asset = ResManager.LoadAsset(R.GetPrefab("Prefabs/Spines/Goblins"));
        OnLoadSpine(asset);

        AppFacade.Instance.SendMessageCommand(MessageConst.SpineFormMessageTest, "SpineForm已经被打开");
    }

    public override void OnClose()
    {
        if (m_GoSpine)
        {
            GameObject.Destroy(m_GoSpine);
            m_GoSpine = null;
        }
    }

    public override void OnMessage(IMessage message)
    {
        MessageBox.Show(message.Name, message.Body as string, null);
    }

    void OnClickClose(GameObject go)
    {
        CloseUIForms("SpineForm");
    } 

    void OnLoadSpine(Object obj)
    {
        m_GoSpine = GameObject.Instantiate(obj) as GameObject;
        m_GoSpine.transform.SetParent(transform, false);
        m_GoSpine.transform.localScale = Vector3.one;
        m_GoSpine.transform.localPosition = Vector3.zero;
    }
}
