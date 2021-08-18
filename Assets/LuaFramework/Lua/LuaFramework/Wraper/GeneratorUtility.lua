--[[
    luaide  模板位置位于 Template/FunTemplate/NewFileTemplate.lua 其中 Template 为配置路径 与luaide.luaTemplatesDir
    luaide.luaTemplatesDir 配置 https://www.showdoc.cc/web/#/luaide?page_id=713062580213505
    author:{author}
    time:2021-08-17 13:39:05
]]

local global = _G
local sgsub = string.gsub
local sformat = string.format

local Protobuf = CSharpGeneratorForProton.Protobuf

local ResourceManager = LuaFramework.LuaHelper.GetResManager()

local NameSpace = 'CSharpGeneratorForProton.Protobuf.'

local GeneratorUtility = {}

Protobuf.GeneratorUtility = GeneratorUtility

local Module = 'Compiled.Generator.Proto'

--[[
    protobuf lua模块加载适配
    @test CSharpGeneratorForProtonProtobuf.GeneratorUtility.Load("HerosProto", "Hero", T)
]]
function GeneratorUtility.Load(arg1, arg2, arg3)
    if arg3 == nil then
        return GeneratorUtility.Load3(arg1, nil, arg2)
    else
        return GeneratorUtility.Load3(arg1, arg2, arg3)
    end
end

function GeneratorUtility.Load3(fileName, itemName, T)
    print (fileName)
    local class_name = sgsub((T.__name__ or ''), NameSpace, '')
    local module_name = sformat('%s_pb', fileName)
    local module_full_name = sformat('%s.%s', Module, module_name)
    -- 加载模块
    require (module_full_name)
    -- 实例化
    local message = global[module_name][class_name]()
    local path = 'Generator/' .. fileName .. '.bytes'
    local bytes = ResourceManager:LoadAsset(path)
    message:ParseFromString(bytes)
    return message
end