using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 默认自动绑定规则辅助器
/// </summary>
public class DefaultAutoBindRuleHelper : IAutoBindRuleHelper
{

    /// <summary>
    /// 命名前缀与类型的映射
    /// </summary>
    private static Dictionary<string, System.Type> m_PrefixesDict = new Dictionary<string, System.Type>()
    {
        { "Tr",         typeof(Transform)               },
        { "Go",         typeof(GameObject)              },
        { "Anin",       typeof(Animation)               }, 
        { "Anir",       typeof(Animator)                },

        { "RTr",        typeof(RectTransform)           },
        { "Cvs",        typeof(Canvas)                  },
        { "CGroup",     typeof(CanvasGroup)             },
        { "VGroup",     typeof(VerticalLayoutGroup)     },
        { "HGroup",     typeof(HorizontalLayoutGroup)   },
        { "GGroup",     typeof(GridLayoutGroup)         },
        { "TGroup",     typeof(ToggleGroup)             },
        // UI
        { "Btn",        typeof(Button)                  },
        { "Img",        typeof(Image)                   },
        { "RImg",       typeof(RawImage)                },
        { "Txt",        typeof(Text)                    },
        { "Input",      typeof(InputField)              },
        { "Slider",     typeof(Slider)                  },
        { "Mask",       typeof(Mask)                    },
        { "RMask",      typeof(RectMask2D)              },
        { "Tog",        typeof(Toggle)                  },
        { "Sbar",       typeof(Scrollbar)               },
        { "SRect",      typeof(ScrollRect)              },
        { "Drop",       typeof(Dropdown)                },
    };

    public static Dictionary<string, System.Type> PrefixesDict
    {
        get 
        {
            return m_PrefixesDict;
        }
    }

    public bool IsValidBind( Transform target, List<string> fieldNames, List<string> componentTypeNames)
    {
        // Txt_Label_Show
        var name = target.name;
        var first = string.Empty;
        var field = string.Empty;
        System.Type component = null;
        var splits = new List<string>(name.Split('_'));
        if (splits.Count < 2)
        {
            return false;
        }

        // 只绑定一个多个会存在歧义
        first = splits[0];
        for (int i = 1; i < splits.Count; i++)
        {
            string pre = (i == 1) ? "" : "_";
            string n = pre + splits[i];
            field = field + n;
        }

        if (!m_PrefixesDict.TryGetValue(first, out component))
        {
            Debug.LogError($"[{name}]的命名中[{first}]不存在对应的组件类型，绑定失败");
            return false;
        }

        fieldNames.Add($"{first}_{field}");
        componentTypeNames.Add(component.Name);

        return true;
    }
}
