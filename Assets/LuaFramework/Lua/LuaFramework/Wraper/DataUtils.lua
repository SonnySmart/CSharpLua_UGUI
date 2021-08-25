--[[
    luaide  模板位置位于 Template/FunTemplate/NewFileTemplate.lua 其中 Template 为配置路径 与luaide.luaTemplatesDir
    luaide.luaTemplatesDir 配置 https://www.showdoc.cc/web/#/luaide?page_id=713062580213505
    author:{author}
    time:2021-08-25 16:32:40
]]

local Protobuf = CSharpGeneratorForProton.Protobuf

local GeneratorUtility = Protobuf.GeneratorUtility

local DataUtils = {}

Protobuf.DataUtils = DataUtils

function DataUtils.ObjectToBytes(instance, T)
    return instance:SerializeToString()
end

function DataUtils.BytesToObject(bytes, T)
    return GeneratorUtility.BytesToObject(bytes, T)
end