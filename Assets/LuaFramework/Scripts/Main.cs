using UnityEngine;
using System.Collections;

namespace LuaFramework {

    /// <summary>
    /// 我是启动组件
    /// </summary>
    public class Main : MonoBehaviour {

        void Start() {
            AppFacade.Instance.StartUp();   //启动游戏
        }
    }
}