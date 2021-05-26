--[[
    luaide  模板位置位于 Template/FunTemplate/NewFileTemplate.lua 其中 Template 为配置路径 与luaide.luaTemplatesDir
    luaide.luaTemplatesDir 配置 https://www.showdoc.cc/web/#/luaide?page_id=713062580213505
    author:{author}
    time:2021-04-01 10:29:21
]]

--[[ 报错堆栈说这个找不到构造函数需要处理下
    LuaException: Compiled/Scripts/UI/UGuiFormBase:15: field or property __ctor__ does not exist
stack traceback:
	[C]: in function '__index'
	Compiled/Scripts/UI/UGuiFormBase:15: in function '__ctor__'
	Compiled/Scripts/UI/UGuiForm:24: in function '__ctor__'
	UnityAdapter:178: in function <UnityAdapter:176>
LuaInterface.LuaState:PCall(Int32, Int32) (at Assets/Scripts/Core/ToLua/ToLua/Core/LuaState.cs:768)
LuaInterface.LuaFunction:PCall() (at Assets/Scripts/Core/ToLua/ToLua/Core/LuaFunction.cs:96)
LuaInterface.LuaFunction:Invoke(BridgeMonoBehaviour, String, String, Object[]) (at Assets/Scripts/Core/ToLua/ToLua/Core/LuaFunction.cs:281)
CSharpLua.CSharpLuaClient:BindLua(BridgeMonoBehaviour) (at Assets/Scripts/Core/CSharpLua/CSharpLuaClient.cs:78)
CSharpLua.BridgeMonoBehaviour:Awake() (at Assets/Scripts/Core/CSharpLua/BridgeMonoBehaviour.cs:46)
UnityEngine.Object:Instantiate(Object)
UnityGameFramework.Runtime.DefaultUIFormHelper:InstantiateUIForm(Object) (at Assets/Scripts/Core/GameFramework/Scripts/Runtime/UI/DefaultUIFormHelper.cs:27)
GameFramework.UI.UIManager:LoadAssetSuccessCallback(String, Object, Single, Object)
UnityGameFramework.Runtime.EditorResourceComponent:Update() (at Assets/Scripts/Core/GameFramework/Scripts/Runtime/Resource/EditorResourceComponent.cs:587)
]]

--[[ test
local UnityEngineMonoBehaviour = UnityEngine.MonoBehaviour
UnityEngine.MonoBehaviour = nil

local MonoBehaviour = System.define("UnityEngine.MonoBehaviour", {
  __ctor__ = emptyFn,
  class = "C",
  GetType = System.Object.GetType,
  print = UnityEngineMonoBehaviour.print,
  Awake = emptyFn,
  Start = emptyFn,
  Update = emptyFn,
  FixedUpdate = emptyFn,
  LateUpdate = emptyFn,
  ToString = function (this)
    return sformat("%s (%s)", this.ref.gameObject.name, this.__name__)
  end,
})
SystemTypeof(MonoBehaviour)[1] = UnityEngineMonoBehaviour

local function getMonoBehaviourFunction(metatableOfMonoBehaviour, name)
  return metatableOfMonoBehaviour[name]   
end

local metatableOfMonoBehaviour = getmetatable(BridgeMonoBehaviour) 
setmetatable(MonoBehaviour, {
  __index = function (t, k)
    if type(k) == "string" then
      local c = sbyte(k, 1)
      if c ~= 95 and c ~= 46 then -- not '.' or '_'
        local ok, f = pcall(getMonoBehaviourFunction, metatableOfMonoBehaviour, k)
        if ok then
          local v = function (this, ...)
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
]]

local emptyFn = System.emptyFn
local SystemTypeof = System.typeof
local BridgeMonoBehaviour = LuaFramework.LuaBehaviour
LuaFramework.LuaBehaviour = nil

local assert = assert
local setmetatable = setmetatable
local getmetatable = getmetatable
local rawget = rawget
local rawset = rawset
local type = type
local tinsert = table.insert
local sformat = string.format
local sbyte = string.byte
local loadstring = loadstring
local ipairs = ipairs
local pairs = pairs
local typeof = typeof
local pcall = pcall

local LuaBehaviour = System.define("LuaFramework.LuaBehaviour", {
    --默认构造函数,找不到会报错所以这里适配
    __ctor__ = emptyFn,
})
SystemTypeof(LuaBehaviour)[1] = BridgeMonoBehaviour

local function getMonoBehaviourFunction(metatableOfMonoBehaviour, name)
  return metatableOfMonoBehaviour[name]   
end

local metatableOfMonoBehaviour = getmetatable(BridgeMonoBehaviour) 
setmetatable(LuaBehaviour, {
  __index = function (t, k)
    if type(k) == "string" then
      local c = sbyte(k, 1)
      if c ~= 95 and c ~= 46 then -- not '.' or '_'
        local ok, f = pcall(getMonoBehaviourFunction, metatableOfMonoBehaviour, k)
        if ok then
          local v = function (this, ...)
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