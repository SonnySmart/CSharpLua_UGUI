#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public static class ComponentAutoBindToolMenuItems
{
    private static bool Invalid()
    {
        return (Selection.gameObjects == null || Selection.gameObjects.Length == 0);
    }

    private static void AddComponentPrefix(System.Type type)
    {
        if (Invalid())
            return;

        var first = string.Empty;
        var prefixesDict = DefaultAutoBindRuleHelper.PrefixesDict;
        foreach (var kv in prefixesDict)
        {
            var key = kv.Key;
            var value = kv.Value;
            if (value == type)
            {
                first = key;
                break;
            }
        }

        if (string.IsNullOrEmpty(first))
        {
            Debug.LogError($"没有找到类型 [{type.Name}] 检查DefaultAutoBindRuleHelper.PrefixesDict的配置");
            return;
        }

        foreach(GameObject obj in Selection.gameObjects)
        {
            AddComponentPrefixDo(obj, first);
        }
    }

    private static void AddComponentPrefixDo(GameObject obj, string first)
    {
        var prefix = string.Empty;
        var field = obj.name;
        var new_name = string.Empty;
        var splits = field.Split('_');
        var prefixesDict = DefaultAutoBindRuleHelper.PrefixesDict;
        // 已经包含前缀取消前缀
        if (field.Contains(first))
        {
            var pre = first + "_";
            new_name = field.Replace(pre, "");
        }
        else
        {
            // 获得前缀
            prefix = splits[0];
            if (prefixesDict.TryGetValue(prefix, out _))
            {
                // 替换前缀
                new_name = field.Replace(prefix, first);
            }
            else
            {
                // 添加前缀
                new_name = $"{first}_{field}";
            }
        }

        // 设置新名字
        obj.name = new_name;
        EditorUtility.SetDirty(obj);
    }

    [MenuItem("GameObject/ComponentAutoBindTool/Transform")]
    private static void Transform()
    {
        AddComponentPrefix(typeof(Transform));
    }

    [MenuItem("GameObject/ComponentAutoBindTool/GameObject")]
    private static void GameObject()
    {
        AddComponentPrefix(typeof(GameObject));
    }

    [MenuItem("GameObject/ComponentAutoBindTool/Animation")]
    private static void Animation()
    {
        AddComponentPrefix(typeof(Animation));
    }

    [MenuItem("GameObject/ComponentAutoBindTool/Animator")]
    private static void Animator()
    {
        AddComponentPrefix(typeof(Animator));
    }

    [MenuItem("GameObject/ComponentAutoBindTool/RectTransform")]
    private static void RectTransform()
    {
        AddComponentPrefix(typeof(RectTransform));
    }

    [MenuItem("GameObject/ComponentAutoBindTool/Canvas")]
    private static void Canvas()
    {
        AddComponentPrefix(typeof(Canvas));
    }

    [MenuItem("GameObject/ComponentAutoBindTool/CanvasGroup")]
    private static void CanvasGroup()
    {
        AddComponentPrefix(typeof(CanvasGroup));
    }

    [MenuItem("GameObject/ComponentAutoBindTool/VerticalLayoutGroup")]
    private static void VerticalLayoutGroup()
    {
        AddComponentPrefix(typeof(VerticalLayoutGroup));
    }

    [MenuItem("GameObject/ComponentAutoBindTool/HorizontalLayoutGroup")]
    private static void HorizontalLayoutGroup()
    {
        AddComponentPrefix(typeof(HorizontalLayoutGroup));
    }

    [MenuItem("GameObject/ComponentAutoBindTool/GridLayoutGroup")]
    private static void GridLayoutGroup()
    {
        AddComponentPrefix(typeof(GridLayoutGroup));
    }

    [MenuItem("GameObject/ComponentAutoBindTool/ToggleGroup")]
    private static void ToggleGroup()
    {
        AddComponentPrefix(typeof(ToggleGroup));
    }

    [MenuItem("GameObject/ComponentAutoBindTool/Button")]
    private static void Button()
    {
        AddComponentPrefix(typeof(Button));
    }

    [MenuItem("GameObject/ComponentAutoBindTool/Image")]
    private static void Image()
    {
        AddComponentPrefix(typeof(Image));
    }

    [MenuItem("GameObject/ComponentAutoBindTool/RawImage")]
    private static void RawImage()
    {
        AddComponentPrefix(typeof(RawImage));
    }

    [MenuItem("GameObject/ComponentAutoBindTool/Text")]
    private static void Text()
    {
        AddComponentPrefix(typeof(Text));
    }

    [MenuItem("GameObject/ComponentAutoBindTool/InputField")]
    private static void InputField()
    {
        AddComponentPrefix(typeof(InputField));
    }

    [MenuItem("GameObject/ComponentAutoBindTool/Slider")]
    private static void Slider()
    {
        AddComponentPrefix(typeof(Slider));
    }

    [MenuItem("GameObject/ComponentAutoBindTool/Mask")]
    private static void Mask()
    {
        AddComponentPrefix(typeof(Mask));
    }

    [MenuItem("GameObject/ComponentAutoBindTool/RectMask2D")]
    private static void RectMask2D()
    {
        AddComponentPrefix(typeof(RectMask2D));
    }

    [MenuItem("GameObject/ComponentAutoBindTool/Toggle")]
    private static void Toggle()
    {
        AddComponentPrefix(typeof(Toggle));
    }

    [MenuItem("GameObject/ComponentAutoBindTool/Scrollbar")]
    private static void Scrollbar()
    {
        AddComponentPrefix(typeof(Scrollbar));
    }

    [MenuItem("GameObject/ComponentAutoBindTool/ScrollRect")]
    private static void ScrollRect()
    {
        AddComponentPrefix(typeof(ScrollRect));
    }

    [MenuItem("GameObject/ComponentAutoBindTool/Dropdown")]
    private static void Dropdown()
    {
        AddComponentPrefix(typeof(Dropdown));
    }
}
#endif