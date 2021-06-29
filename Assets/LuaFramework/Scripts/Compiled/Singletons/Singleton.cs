/** 
 *Author:       guo
 *Date:         2018
 *Description:  普通的单例模式
*/ 

public abstract class Singleton<T> where T : class,new()  
{
	private static T _instance = null;

	public static T Instance
	{
		get 
		{ 
			if (_instance == null)
			{
				_instance = new T ();
			}
			return _instance;
		}
	}

}
