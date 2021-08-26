--[[
    luaide  模板位置位于 Template/FunTemplate/NewFileTemplate.lua 其中 Template 为配置路径 与luaide.luaTemplatesDir
    luaide.luaTemplatesDir 配置 https://www.showdoc.cc/web/#/luaide?page_id=713062580213505
    author:{author}
    time:2021-08-17 13:39:05
]]

local global = _G
local sgsub = string.gsub
local sformat = string.format
local rawget = rawget
local class = class

local Protobuf = CSharpGeneratorForProton.Protobuf

local ResourceManager = LuaFramework.LuaHelper.GetResManager()

local NameSpace = 'CSharpGeneratorForProton.Protobuf.'

-- 这里需要赋值Wraper生成的GeneratorUtility进行函数扩充
local GeneratorUtility = class('GeneratorUtility', Protobuf.GeneratorUtility) or {}

Protobuf.GeneratorUtility = GeneratorUtility

local Module = 'Compiled.Generator.Proto'

-- protobuf lua模块加载适配
function GeneratorUtility.Load(arg1, arg2, arg3)
    if arg3 == nil then
        return GeneratorUtility.Load3(arg1, nil, arg2)
    else
        return GeneratorUtility.Load3(arg1, arg2, arg3)
    end
end

-- 加载pb.lua bytes进行序列化
function GeneratorUtility.Load3(fileName, itemName, T)
    return GeneratorUtility.Serialize(nil, T)
end

-- 加载bytes进行序列化
function GeneratorUtility.BytesToObject(bytes, T)
    return GeneratorUtility.Serialize(bytes, T)
end

-- 序列化对象
function GeneratorUtility.Serialize(bytes, T)
    -- 类名
    local cls_name = sgsub((T.__name__ or ''), NameSpace, '')
    -- 模块名
    local m_name = sformat('%s_pb', cls_name)
    -- 模块路径
    local m_path = sformat('%s.%s', Module, m_name)
    local module = rawget(global, m_name)
    if not module then
        -- 加载模块
        require (m_path)
        module = rawget(global, m_name)
    end
    -- 构造class
    local message = module[cls_name]()
    if not bytes then
        -- 获取bytes
        bytes = GeneratorUtility.GetContentBytes(cls_name)
    end
    -- 从字符串序列化
    message:ParseFromString(bytes)
    return message
end