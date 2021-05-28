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