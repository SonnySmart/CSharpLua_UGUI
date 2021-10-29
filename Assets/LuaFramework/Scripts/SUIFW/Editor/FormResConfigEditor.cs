/** 
 *Author:       lang
 *Date:         2017
 *Description:  自动生成窗体资源配置json
*/

#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class FormResConfigEditor
{
    private static readonly string ASSET_RESHOTFIX_PATH = Path.Combine(Application.dataPath, "ResHotfix");
    private static readonly string ASSET_PREFABS_FORMS_PATH = Path.Combine(ASSET_RESHOTFIX_PATH, "Prefabs", "Forms");
    private static readonly string ASSET_CONFIG_INFO_JSON = Path.Combine(ASSET_RESHOTFIX_PATH, "Config", "UIFormsConfigInfo.json");

    [MenuItem("LuaFramework/Gen SUIFW 窗体配置", false, 83)]
    public static void GenUIFormConfigJson()
    {
        string resourceDir = ASSET_RESHOTFIX_PATH;
        string formDir = ASSET_PREFABS_FORMS_PATH;
        string jsonFile = ASSET_CONFIG_INFO_JSON;

        if (!Directory.Exists(resourceDir))
        {
            Directory.CreateDirectory(resourceDir);
        }
        if (!Directory.Exists(formDir))
        {
            Directory.CreateDirectory(formDir);
        }

        string[] files = Directory.GetFiles(formDir, "*Form.prefab", SearchOption.AllDirectories);

        if (files.Length == 0)
        {
            Debug.Log("没有要更新的form");
            return;
        }

        KeyValuesInfo infos = new KeyValuesInfo();
        infos.ConfigInfo = new List<KeyValuesNode>(files.Length);

        foreach (var file in files)
        {
            string formName = Path.GetFileNameWithoutExtension(file);
            string formPath = file.Substring(resourceDir.Length + 1);
            formPath = formPath.Replace(".prefab", "").Replace("\\", "/");

            infos.ConfigInfo.Add(new KeyValuesNode
            {
                Key = formName,
                Value = formPath
            });
        }
        string json = JsonUtility.ToJson(infos);
        try
        {
            File.WriteAllText(jsonFile, json);
        }
        catch (Exception e)
        {
            Debug.LogError("写入json文件失败：" + e.Message);
            return;
        }

        AssetDatabase.Refresh();

        Debug.LogFormat("已更新{0}个，路径：{1}", files.Length, jsonFile);
    }
}

[Serializable]
internal class KeyValuesInfo
{
    //配置信息
    public List<KeyValuesNode> ConfigInfo;
}

[Serializable]
internal class KeyValuesNode
{
    //键
    public string Key;

    //值
    public string Value;
}
#endif