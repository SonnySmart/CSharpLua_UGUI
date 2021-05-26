--[[
    luaide  模板位置位于 Template/FunTemplate/NewFileTemplate.lua 其中 Template 为配置路径 与luaide.luaTemplatesDir
    luaide.luaTemplatesDir 配置 https://www.showdoc.cc/web/#/luaide?page_id=713062580213505
    author:{author}
    time:2021-04-01 10:47:02
]]

return function(dir)
    dir = (dir and #dir > 0) and (dir .. ".") or "."
    local require = require
    local load = function(module) return require(dir .. module) end
      
    load("functions")
end