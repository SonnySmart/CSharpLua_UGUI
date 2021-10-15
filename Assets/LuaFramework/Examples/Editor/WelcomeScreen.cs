#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

public class WelcomeScreen : EditorWindow
{
    private string version = "Version : 1.0.0";
    private bool flag = true;
    private Rect mContactDescriptionRect = new Rect(70f, 344f, 250f, 30f);
    private Rect mContactHeaderRect = new Rect(70f, 324f, 250f, 20f);
    private Texture mContactImage;
    private Rect mContactImageRect = new Rect(15f, 322f, 50f, 50f);
    private Rect mDocDescriptionRect = new Rect(70f, 143f, 260f, 30f);
    private Rect mDocHeaderRect = new Rect(70f, 123f, 350f, 20f);
    private Texture mDocImage;
    private Rect mDocImageRect = new Rect(15f, 124f, 53f, 50f);
    private Rect mForumDescriptionRect = new Rect(70f, 278f, 380f, 30f);
    private Rect mForumHeaderRect = new Rect(70f, 258f, 300f, 20f);
    private Texture mForumImage;
    private Rect mForumImageRect = new Rect(15f, 256f, 50f, 50f);
    private Rect mSamplesDescriptionRect = new Rect(70f, 77f, 250f, 30f);
    private Rect mSamplesHeaderRect = new Rect(70f, 57f, 250f, 20f);
    private Texture mSamplesImage;
    private Rect mSamplesImageRect = new Rect(15f, 58f, 50f, 50f);
    private Rect mToggleButtonRect = new Rect(280f, 385f, 125f, 20f);
    private Rect mVersionRect = new Rect(5f, 385f, 125f, 20f);
    private Rect mVideoDescriptionRect = new Rect(70f, 209f, 380f, 30f);
    private Rect mVideoHeaderRect = new Rect(70f, 189f, 350f, 20f);
    private Texture mVideoImage;
    private Rect mVideoImageRect = new Rect(15f, 190f, 50f, 50f);
    private Rect mWelcomeIntroRect = new Rect(10f, 12f, 400f, 40f);
    private Texture mWelcomeScreenImage;
    private Rect mWelcomeScreenImageRect = new Rect(0f, 0f, 340f, 44f);

    public void OnEnable()
    {
        //this.mWelcomeScreenImage = EditorGUIUtility.Load("WelcomeScreenHeader.png") as Texture;
            //BehaviorDesignerUtility.LoadTexture("WelcomeScreenHeader.png", false, this);
        flag = PlayerPrefs.GetInt("ShowWelcomeScreen", 1) == 1;
        this.mSamplesImage = LoadTexture("WelcomeScreenSamplesIcon.png");
        this.mDocImage = LoadTexture("WelcomeScreenDocumentationIcon.png");
        this.mVideoImage = LoadTexture("WelcomeScreenVideosIcon.png");
        this.mForumImage = LoadTexture("WelcomeScreenForumIcon.png");
        this.mContactImage = LoadTexture("WelcomeScreenContactIcon.png");
    }


    Texture LoadTexture(string name) {
        string path = "Assets/LuaFramework/Examples/Editor Default Resources/";
        return (Texture)AssetDatabase.LoadAssetAtPath(path + name, typeof(Texture));
    }

    public void OnGUI()
    {
        //GUI.DrawTexture(this.mWelcomeScreenImageRect, this.mWelcomeScreenImage);
        GUI.Label(this.mWelcomeIntroRect, "欢迎使用LuaFramework，它是个基于tolua#，\n将C#类注册进Lua，并且附带了AssetBundle管理的演示框架。入门步骤如下：");
        GUI.DrawTexture(this.mSamplesImageRect, this.mSamplesImage);
        GUI.Label(this.mSamplesHeaderRect, "新手入门 - 生成Wrap文件(必须)" );
        GUI.Label(this.mSamplesDescriptionRect, "单机菜单 - Lua/Generate All");
        GUI.DrawTexture(this.mDocImageRect, this.mDocImage);
        GUI.Label(this.mDocHeaderRect, "新手入门 - 根据不同平台生成AssetBundle资源(必须)");
        GUI.Label(this.mDocDescriptionRect, "单机菜单 - XASSET/Build/Bundles");
        GUI.DrawTexture(this.mVideoImageRect, this.mVideoImage);
        GUI.Label(this.mVideoHeaderRect, "新手入门 - 改完注册到Lua的C#类，需清除文件缓存，重新生成");
        GUI.Label(this.mVideoDescriptionRect, "单机菜单 - Lua/Clear Wrap Files");
        GUI.DrawTexture(this.mForumImageRect, this.mForumImage);
        GUI.Label(this.mForumHeaderRect, "新手入门 - 生成CSharp.lua翻译文件.cs -> .lua");
        GUI.Label(this.mForumDescriptionRect, "单机菜单 - LuaFramework/Compile C#2Lua");
        GUI.DrawTexture(this.mContactImageRect, this.mContactImage);
        GUI.Label(this.mContactHeaderRect, "新手入门 - 打包apk exe");
        GUI.Label(this.mContactDescriptionRect, "单机菜单 - XASSET/Build/Player");
        GUI.Label(this.mVersionRect, version );

        flag = GUI.Toggle(this.mToggleButtonRect, flag, "开始时候显示对话框");
        if (flag) {
            PlayerPrefs.SetInt("ShowWelcomeScreen", 1);
        } else {
            PlayerPrefs.SetInt("ShowWelcomeScreen", 0);
        }
        EditorGUIUtility.AddCursorRect(this.mSamplesImageRect, MouseCursor.Link);
        EditorGUIUtility.AddCursorRect(this.mSamplesHeaderRect, MouseCursor.Link);
        EditorGUIUtility.AddCursorRect(this.mSamplesDescriptionRect, MouseCursor.Link);
        EditorGUIUtility.AddCursorRect(this.mDocImageRect, MouseCursor.Link);
        EditorGUIUtility.AddCursorRect(this.mDocHeaderRect, MouseCursor.Link);
        EditorGUIUtility.AddCursorRect(this.mDocDescriptionRect, MouseCursor.Link);
        EditorGUIUtility.AddCursorRect(this.mVideoImageRect, MouseCursor.Link);
        EditorGUIUtility.AddCursorRect(this.mVideoHeaderRect, MouseCursor.Link);
        EditorGUIUtility.AddCursorRect(this.mVideoDescriptionRect, MouseCursor.Link);
        EditorGUIUtility.AddCursorRect(this.mForumImageRect, MouseCursor.Link);
        EditorGUIUtility.AddCursorRect(this.mForumHeaderRect, MouseCursor.Link);
        EditorGUIUtility.AddCursorRect(this.mForumDescriptionRect, MouseCursor.Link);
        EditorGUIUtility.AddCursorRect(this.mContactImageRect, MouseCursor.Link);
        EditorGUIUtility.AddCursorRect(this.mContactHeaderRect, MouseCursor.Link);
        EditorGUIUtility.AddCursorRect(this.mContactDescriptionRect, MouseCursor.Link);
        if (Event.current.type == EventType.MouseUp)
        {
            Vector2 mousePosition = Event.current.mousePosition;
            if ((this.mSamplesImageRect.Contains(mousePosition) || this.mSamplesHeaderRect.Contains(mousePosition)) || this.mSamplesDescriptionRect.Contains(mousePosition))
            {
                //LuaBinding.Binding();
            }
            else if ((this.mDocImageRect.Contains(mousePosition) || this.mDocHeaderRect.Contains(mousePosition)) || this.mDocDescriptionRect.Contains(mousePosition))
            {
                if (Application.platform == RuntimePlatform.WindowsEditor) {
                    //Packager.BuildWindowsResource();
                }
                if (Application.platform == RuntimePlatform.OSXEditor) {
                    //Packager.BuildiPhoneResource();
                }
            }
            else if ((this.mVideoImageRect.Contains(mousePosition) || this.mVideoHeaderRect.Contains(mousePosition)) || this.mVideoDescriptionRect.Contains(mousePosition))
            {
                //LuaBinding.ClearLuaBinder();
            }
            else if ((this.mForumImageRect.Contains(mousePosition) || this.mForumHeaderRect.Contains(mousePosition)) || this.mForumDescriptionRect.Contains(mousePosition))
            {
                //LuaBinding.EncodeLuaFile();
            }
            else if ((this.mContactImageRect.Contains(mousePosition) || this.mContactHeaderRect.Contains(mousePosition)) || this.mContactDescriptionRect.Contains(mousePosition))
            {
                Application.OpenURL("http://shang.qq.com/wpa/qunwpa?idkey=20a9db3bac183720c13a13420c7c805ff4a2810c532db916e6f5e08ea6bc3a8f");
            }
        }
    }

    [UnityEditor.MenuItem("LuaFramework/Welcome Screen", false, 1)]
    public static void ShowWindow()
    {
        WelcomeScreen window = EditorWindow.GetWindow<WelcomeScreen>(true, "Welcome to LuaFramework");
        window.minSize = window.maxSize = new Vector2(410f, 410f);
        UnityEngine.Object.DontDestroyOnLoad(window);
    }
}
#endif