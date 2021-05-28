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

Lua挂载脚本实现原理:
	LuaFramework.LuaBehaviour继承至MonoBehaviour
	MonoBehaviour不能直接AddComponent会报错
	例子:
	public LuaChildClass : LuaFramework.LuaBehaviour {
		...
	}
	
	FixBehaviour.lua:
	-- 修复节点对象
	local function fixbehaviour(cls)
		local define = cls[".name"]
		local BridgeMonoBehaviour = cls
		-- 这里定义是把原来的LuaFramework.LuaBehaviour替换成了一个table
		local MonoBehaviour = System.define(define, {
			...
		})
		-- 这里保存了原先的组件
		SystemTypeof(MonoBehaviour)[1] = BridgeMonoBehaviour
		-- 获取父table
		local metatableOfMonoBehaviour = getmetatable(BridgeMonoBehaviour) 
		-- 设置父table
		setmetatable(MonoBehaviour, {
			-- 设置索引
			__index = function (t, k)
				if type(k) == "string" then
				local c = sbyte(k, 1)
				if c ~= 95 and c ~= 46 then -- not '.' or '_'
					local ok, f = pcall(getMonoBehaviourFunction, metatableOfMonoBehaviour, k)
					if ok then
					local v = function (this, ...)
						-- 利用this.ref来修复绑定对象,table是不具备组件对象的
						return f(this.ref, ...)
					end
					t[k] = v
					return v
					end
				end
				end
				return nil
			end
		})
	end
	
	UnityAdapter.lua:
	local function addBridgeMonoBehaviour(gameObject, T)
		-- 这里获取父table也就是TableLuaChildClass
		local metatableOfSuper = getmetatable(T)
		-- 挂载的时候是挂载的父节点组件而不是table这里被替换掉了,否则不能挂载成功
		local MonoBehaviour = SystemTypeof(metatableOfSuper)[1]
		assert(MonoBehaviour, "addBridgeMonoBehaviour MonoBehaviour is nil .")
		local typeofMonoBehaviour = typeof(MonoBehaviour)
		-- 挂载父节点组件
		local monoBehaviour = sourceAddComponent(gameObject, typeofMonoBehaviour)
		return newMonoBehaviour(T, monoBehaviour)
	end
	
Cs2Lua使用注意事项:
	1.所有Compiled下的文件继承基类,基类必须生成Lua Wrap绑定文件,否则Lua会找不到基类
	2.避免Interface为基类Lua貌似不能生成Interface的Wrap
	3.函数具有回调的参数如:
		public void LoadPrefab(string abName, string assetName, Action<UObject[]> action)
		Action<UObject[]> action这样的回调必须在定义一个Lua用于调取的函数
		
		public void LoadPrefab(string abName, string assetName, LuaFunction func)
		Action<UObject[]> 替换为了 LuaFunction 这样Lua才能调用回调
	4.使用模板存在一些问题需要注意
	5.Compiled继承需要注意的地方:
		需要继承新的类,要在Lua/LuaFramework/FixBehaviour.lua中修复基类函数
		注意事项:
			不能直接继承MonoBehaviour,详细 => Lua挂载脚本实现原理
		支持的组件:
			LuaFramework.LuaBehaviour
			SUIFW.BaseUIForms
		
		
		
		