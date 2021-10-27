using UnityEngine;
using LuaFramework;
using SUIFW;
using UnityEngine.UI;
using CSharpGeneratorForProton.Json;
using CSharpGeneratorForProton.Protobuf;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public partial class PromptForm : BaseUIForms
{
    public override void OnInit()
    {
        //是否需要清空“反向切换”
        CurrentUIType.IsClearReverseChange = false;
        //UI窗体类型
        CurrentUIType.UIForms_Type = UIFormsType.Normal;
        //UI窗体显示类型
        CurrentUIType.UIForms_ShowMode = UIFormsShowMode.Normal;
        //UI窗体透明度类型
        CurrentUIType.UIForms_LucencyType = UIFormsLucencyType.Lucency;

        // MVC注册
        AttentionList.Add(MessageConst.PromptFormMessageTest);

        print($"{LuaClass} CurrentUIType.UIForms_Type is {CurrentUIType.UIForms_Type}");

        print($"{LuaClass} OnInit Is Call . ");
    }

    /*
    // 不能重写他
    private void Awake()
    {
        print("我是Cs被打印了 PromptForm Awake");
    }
    */

    public void Start()
    {
        print("我是Cs被打印了 PromptForm Start");

        this.LuaBehaviourTest();
        this.BaseUIFormsTest();

        this.DoTest();
        //this.ConfigTest();
        //this.ProtoTest();
        //this.ProtoPersionTest();
    }

    public void DoTest()
    {
        // 这是正确的
        var behaviour = gameObject.AddComponent<BehaviourTest>();
        behaviour.Init();

        // 这是错误的
        //gameObject.AddComponent<BehaviourTest1>();
        
        // 这是错误的
        //var test2 = new BehaviourTest2();
        //test2.SayHello();

        // 这是正确的
        //var test3 = new TestBaseBehaviourScript3();
        //test3.ClassSayHello();
        //TestBaseBehaviourScript3.SayHello();

        // 有问题...
        //var test3_ = new BehaviourTest3();
        //test3_.SayChild();

        //SingletonTest singletonTest = gameObject.AddComponent<SingletonTest>();

        //gameObject.AddComponent<BehaviourTest4>();

        SingletonNormalTest.Instance.TestPrint();
    }

    void ConfigTest()
    {
        // ConfigTest
        var config = GlobalConfig.Load();
        int NameLimit = config.NameLimit;
        var LevelRange = config.LevelRange;
        int Max = LevelRange.Max;
        print ($"Max -> {Max}");

        var heroConfigs = HeroConfig.Load();
        foreach (var heroConfig in heroConfigs)
        {
            var Bags = heroConfig.Bags;
            foreach (var bag in Bags)
            {
                print($"bag.Id -> {bag.Id}");
            }
        }
    }

    void ProtoTest()
    {
        // ProtoTest
        var config = TextProto.Load();
        var Text_1 = config.Text_1;
        var Text_2 = config.Text_2;
        var Text_3 = config.Text_3;
        print ($"config Text_1 -> {Text_1}");
        print ($"config Text_2 -> {Text_2}");
        print ($"config Text_3 -> {Text_3}");

        byte[] data = DataUtils.ObjectToBytes<TextProto>(config);
        //print (data);

        var deconfig = DataUtils.BytesToObject<TextProto>(data);
        //print (deconfig);

        Text_1 = deconfig.Text_1;
        Text_2 = deconfig.Text_2;
        Text_3 = deconfig.Text_3;
        print ($"deconfig Text_1 -> {Text_1}");
        print ($"deconfig Text_2 -> {Text_2}");
        print ($"deconfig Text_3 -> {Text_3}");
    }

    void ProtoPersionTest()
    {
        var config = PersonProto.Load();
        print ($"config.id -> {config.id}");
        print ($"config.name -> {config.name}");
        print ($"config.age -> {config.age}");
        print ($"config.email -> {config.email}");        
        foreach (var a in config.arrays)
        {
            print ($"config.a -> {a}");
        }

        config.id = 2;
        config.name = "蛤蟆怪";
        config.age = 999;
        config.email = "9999@qq.com";
        config.arrays[0] = 111;
        config.arrays[1] = 222;
        config.arrays[2] = 333;

        var arrays1 = config.arrays[0];
        print (arrays1);

        byte[] data = DataUtils.ObjectToBytes<PersonProto>(config);
        print ("-------------------------分割线----------------------------------");

        var deconfig = DataUtils.BytesToObject<PersonProto>(data);
        print ($"config.id -> {config.id}");
        print ($"config.name -> {config.name}");
        print ($"config.age -> {config.age}");
        print ($"config.email -> {config.email}");        
        List<int> list = new List<int>();
        foreach (var a in config.arrays)
        {
            print ($"config.arrays.a -> {a}");
            list.Add(a);
        } 
        foreach (var a in list)
        {
            print ($"list.a -> {a}");
        }         
    }

    public override void OnOpen()
    {
        OnCreate();

        // 向自己发送消息
        AppFacade.Instance.SendMessageCommand(MessageConst.PromptFormMessageTest, "测试消息");
    }

    public override void OnMessage(IMessage message)
    {
        Debug.Log($"PromptForm OnMessage name is {message.Name} body is {message.Body}");
    }

    public void OnCreate()
    {
        Debug.Log("Start lua--->>" + gameObject.name);

        AddClickEventListener(m_Btn_Open.gameObject, this.OnClick);
        LuaHelper.GetResManager().LoadAsset(R.GetPrefab("PromptItem"), this.InitPanel);
    }

    public void InitPanel(Object objs)
    {
        int count = 100; 
        var parent = this.m_Tr_Content;
        for (int i = 0; i < count; i++)
        {
            var go = GameObject.Instantiate(objs) as GameObject;
            go.name = "Item" + i.ToString();
            go.transform.SetParent(parent, false);
            go.transform.localScale = Vector3.one;
            go.transform.localPosition = Vector3.zero;

            PromptItem item = go.GetComponent<PromptItem>();
            item.text = i.ToString();
        }

        Util.CalcTime("执行lua完成");
    }

    public void OnClick(GameObject go)
    {
        Debug.Log("OnClick---->>>" + go.name);
    }
}