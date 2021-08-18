--[[
    luaide  模板位置位于 Template/FunTemplate/NewFileTemplate.lua 其中 Template 为配置路径 与luaide.luaTemplatesDir
    luaide.luaTemplatesDir 配置 https://www.showdoc.cc/web/#/luaide?page_id=713062580213505
    author:{author}
    time:2021-08-17 13:39:05
]]

local Protobuf = CSharpGeneratorForProton.Protobuf

local GeneratorUtility = {}

Protobuf.GeneratorUtility = GeneratorUtility

local Module = 'Compiled.Generator.Proto'

--CSharpGeneratorForProtonProtobuf.GeneratorUtility.Load("HerosProto", "Hero", T)
function GeneratorUtility.Load(fileName, T)
    print (fileName)
    local module_name = string.format('%s.%s_pb', Module, fileName)
    local m = require (module_name)
    return m
end
