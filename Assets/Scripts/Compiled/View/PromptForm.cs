using UnityEngine;
using LuaFramework;
using SUIFW;
using UnityEngine.UI;
using CSharpGeneratorForProton.Json;

public class PromptForm : BaseUIForms
{
    public GameObject btnOpen;
    public Transform gridParent;

    public override void OnInit()
    {
        //是否需要清空“反向切换”
        CurrentUIType.IsClearReverseChange = false;
        //UI窗体类型
        CurrentUIType.UIForms_Type = UIFormsType.PopUp;
        //UI窗体显示类型
        CurrentUIType.UIForms_ShowMode = UIFormsShowMode.Normal;
        //UI窗体透明度类型
        CurrentUIType.UIForms_LucencyType = UIFormsLucencyType.Lucency;

        // MVC注册
        AttentionList.Add(MessageConst.PromptFormMessageTest);

        print($"{LuaClass} CurrentUIType.UIForms_Type is {CurrentUIType.UIForms_Type}");

        print($"{LuaClass} OnInit Is Call . ");
    }

    public void Awake()
    {
        print("我是Cs被打印了 PromptForm Awake");

        this.InitPanelTest();

        this.LuaBehaviourTest();
        this.BaseUIFormsTest();

        this.DoTest();
    }

    public void Start()
    {
        print("我是Cs被打印了 PromptForm Start");
    }

    public void InitPanelTest()
    {
        var tr = this.transform;

        this.btnOpen = tr.Find("Open").gameObject;
	    this.gridParent = tr.Find("ScrollView/Grid");
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

        //SingletonTest.Instance.TestPrint();

        SingletonNormalTest.Instance.TestPrint();

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

        AddClick("Open", this.OnClick);
        LuaHelper.GetResManager().LoadAsset(R.GetPrefab("PromptItem"), this.InitPanel);
    }

    public void InitPanel(Object objs)
    {
        int count = 100; 
        var parent = this.gridParent;
        for (int i = 0; i < count; i++)
        {
            var go = GameObject.Instantiate(objs) as GameObject;
            go.name = "Item" + i.ToString();
            go.transform.SetParent(parent, false);
            go.transform.localScale = Vector3.one;
            go.transform.localPosition = Vector3.zero;
            AddClick(go, this.OnItemClick);

            var label = go.transform.Find("Text");
            label.GetComponent<Text>().text = i.ToString();
        }
    }

    public void OnClick(GameObject go)
    {
        Debug.Log("OnClick---->>>" + go.name);
    }
    
    public void OnItemClick(GameObject go)
    {
        Debug.Log("OnItemClick---->>>" + go.name);
    }
}