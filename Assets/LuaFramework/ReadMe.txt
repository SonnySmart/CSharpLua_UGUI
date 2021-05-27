#####################################
Cs2Lua ReadMe.txt
虽然tolua使用已经足够方便,但是lua开发效率慢这是没办法的

这个项目是基于tolua的但是也缺乏了很多基础性的东西
https://github.com/yanghuan/CSharpLuaForUnity.git

所以尝试将这两个项目融合一下(俗称接缝侠)
项目优点:
	*具有tolua的便捷以及热更新功能
	*具有C#高效开发和强大的语法检查
	*测试用C#进行开发功能,发布的时候翻译成Lua发布,业务模块几乎都能更新到

#####################################

Cs2Lua使用:
	*跟LuaFramework_UGUI_V2的使用方式一致并无不同

Cs2Lua目录结构:
	*此目录为热更新目录下面的所有C#代码将会编译为lua
	Assets/Scripts/Compiled
	
	*项目用到的非热更新类可放到这里,记得生成Wrap
	Assets/Scripts/XXXX
	
	*此目录为Lua文件项目所有的Lua文件均在此处
	Assets/LuaFramework/Lua
	
	*此目录为C#翻译之后的文件,不需要去手动编辑他
	Assets/LuaFramework/Lua/Compiled
	
Cs2Lua使用注意事项:
	1.所有Compiled下的文件继承基类,基类必须生成Lua Wrap绑定文件,否则Lua会找不到基类
	2.避免Interface为基类Lua貌似不能生成Interface的Wrap
	3.函数具有回调的参数如:
		public void LoadPrefab(string abName, string assetName, Action<UObject[]> action)
		Action<UObject[]> action这样的回调必须在定义一个Lua用于调取的函数
		
		public void LoadPrefab(string abName, string assetName, LuaFunction func)
		Action<UObject[]> 替换为了 LuaFunction 这样Lua才能调用回调
	4....