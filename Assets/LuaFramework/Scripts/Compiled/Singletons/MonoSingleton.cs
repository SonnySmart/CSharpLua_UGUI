
/** 
 *Author:       guo
 *Date:         2017
 *Description:  基于MonoBehaviour的单例模板
*/

using System;
using UnityEngine;

[Obsolete("Lua暂时不支持")]
public abstract class MonoSingleton<T> : MonoBehaviour
	where T : MonoBehaviour
{
	private static T m_instance = null;

	public static T Instance
	{
		get { return m_instance; }
	}

	protected virtual void Awake()
	{
		m_instance = this as T;
	}
}
