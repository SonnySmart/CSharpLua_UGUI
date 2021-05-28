local emptyFn = System.emptyFn
local SystemTypeof = System.typeof
local setmetatable = setmetatable
local getmetatable = getmetatable
local rawget = rawget
local rawset = rawset
local type = type
local sbyte = string.byte
local typeof = typeof
local pcall = pcall
local sformat = string.format
local UnityEngineMonoBehaviour = UnityEngine.MonoBehaviour

local function getMonoBehaviourFunction(metatableOfMonoBehaviour, name)
  return metatableOfMonoBehaviour[name]   
end

local function fixbehaviour(cls)
    local define = cls[".name"]
    local BridgeMonoBehaviour = cls
    local MonoBehaviour = System.define(define, {
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
    SystemTypeof(MonoBehaviour)[1] = BridgeMonoBehaviour

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
end

local cls = LuaFramework.LuaBehaviour
LuaFramework.LuaBehaviour = nil
fixbehaviour(cls)

local cls = SUIFW.BaseUIForms
SUIFW.BaseUIForms = nil
fixbehaviour(cls)