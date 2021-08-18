#encoding=utf-8

# Need to export the public configuration file (client, server needs) 需要导出的公共配置文件(客户端,服务器都需要)
EXPORT_FILES = [
]

# Additional configuration files that the client needs to export (only the client needs) 客户端额外需要导出的额外配置文件(仅客户端需要)
EXPORT_CLIENT_ONLY = [
]

# Server-side need to export the configuration file (only the server needs) 服务器端额外需要导出的配置文件(仅服务器需要)
EXPORT_SERVER_ONLY = [
]

# Protobuf导出配置文件
EXPORT_PROTO_FILES = [
]

# do not modify the following

import os
import platform
import traceback
import shutil
import sys

workpath = os.getcwd()
exportscript = './proton.py'     
exportconfig = './__export.txt'
pythonpath = 'sample\\tools\\py37\\py37.exe ' if platform.system() == 'Windows' else 'python '
csprotonpath = 'sample\\tools\\CSharpGeneratorForProton\\CSharpGeneratorForProton.exe '
xlsxfolder = 'sample/Config' # 默认xlsx配置路径
protofolder = 'sample/Proto' # 默认protobuf配置路径
protogenfolder = 'sample/Export'
assetsfolder = os.path.join(workpath, '..', '..', 'Assets')
xlsxdatafolder = os.path.join(assetsfolder, 'ResHotfix') # 默认xlsx输出路径 -> .json/.xml
xlsxcsfolder = os.path.join(assetsfolder, 'Scripts', 'Compiled') # 默认xlsx -> c#输出路径 -> .cs
luagenfolder = os.path.join(assetsfolder, 'LuaFramework', 'Lua', 'Compiled', 'Generator', 'Proto') # lua protobuf 路径

class ExportError(Exception):
  pass

def defconfig():
  strconfig = '# 默认xlsx配置路径\n'
  strconfig += (xlsxfolder + '\n')
  strconfig += '# 默认protobuf配置路径\n'
  strconfig += (protofolder + '\n')
  strconfig += '# 默认protobuf输出路径\n'
  strconfig += (protogenfolder)
  return strconfig

def readline(f):
  s = f.readline()
  if '#' in s:
    s = f.readline()
  s = s.replace('\n', '')
  return s

def readconfig():
  global xlsxfolder
  global protofolder
  global protogenfolder
  if not os.path.isfile(exportconfig):
    with open(exportconfig, 'w') as f:
      strconfig = defconfig()
      f.write(strconfig)
      pass
  with open(exportconfig, 'r') as f:
    xlsxfolder = readline(f)
    protofolder = readline(f)
    protogenfolder = readline(f)
    pass
  print ('xlsxfolder ->' , xlsxfolder)
  print ('protofolder ->' , protofolder)
  print ('protogenfolder ->' , protogenfolder)
  pass

def readxlsx(proto):
  if proto:
    _readxlsx(protofolder, EXPORT_PROTO_FILES)
  else:
    _readxlsx(xlsxfolder, EXPORT_FILES, EXPORT_SERVER_ONLY, EXPORT_CLIENT_ONLY)

def _readxlsx(folder, export_files, export_s = None, export_c = None):
  if not os.path.isdir(folder):
    raise ExportError('xlsxfolder [%s] is not exist .' % folder)
  for root, dirs, files in os.walk(folder):
    for f in files:
      if not f.endswith('.xlsx'):
        continue
      if '~$' in f:
        continue
      path = os.path.join(root, f)
      export_files.append(path)
      pass
    pass

def export(filelist, format, sign, outfolder, suffix, schema):
  outfolder = os.path.join(xlsxdatafolder, outfolder)
  cmd = r' -p "' + ','.join(filelist) + '" -f ' + outfolder + ' -e ' + format + ' -s ' + sign
  if suffix:
    cmd += ' -t ' + suffix
  if schema:
    cmd += ' -c ' + schema
  cmd = pythonpath + exportscript + cmd
  print ('export cmd -> ', cmd)
  code = os.system(cmd)
  if code != 0:
    raise ExportError('export excel fail, please see print')

def codegenerator(schema, outfolder, namespace, suffix, proto = None, outprotofolder = None):
  outfolder = os.path.join(xlsxcsfolder, outfolder.replace('/', os.path.sep))
  if proto:
    outprotofolder = os.path.join(xlsxdatafolder, outprotofolder.replace('/', os.path.sep))
  if os.path.exists(schema):
    cmd = csprotonpath + '-n ' + namespace + ' -f ' + outfolder + ' -p ' + schema
    if suffix:
      cmd += ' -t ' + suffix 
    if proto:
      cmd += ' -e -d ' + outprotofolder + ' -g ' + protogenfolder + ' -b .bytes'
    with open('__cmd.txt', 'w') as f:
      f.write(cmd)
    #print ('codegenerator cmd -> ', cmd)
    code = os.system(cmd)
    #os.remove(schema)      
    if code != 0:
      raise ExportError('codegenerator fail, please see print')
    else:
      # protobuf生成成功处理 -> 拷贝lua到项目
      if proto:
        copy_lua_generator()        
        pass

def copy_lua_generator():
  folder = os.path.join(protogenfolder, 'Gen', 'Lua')
  target = luagenfolder
  if not os.path.isdir(folder):
    return
  for root, dirs, files in os.walk(folder):
    for f in files:
      if not f.endswith('.lua'):
        continue
      source = os.path.join(root, f)
      try:
        shutil.copy(source, target)
        print ('shutil.copy [%s] -> [%s]' % (source, 'Generator/Proto'))
      except IOError as e:
        print('Unable to copy file. %s' % e)
      except:
        print('Unexpected error:', sys.exc_info())
      pass
    pass
        
def exportserver(proto):
  if proto:
    export(EXPORT_PROTO_FILES, 'json', 'server', 'Generator', 'Proto', 'schemaserver.proto.json')
    codegenerator('schemaserver.proto.json', 'Generator/Proto', 'CSharpGeneratorForProton.Protobuf', 'Proto', True, 'Generator') 
  else:
    export(EXPORT_FILES + EXPORT_SERVER_ONLY, 'json', 'server', 'Generator', 'Config', 'schemaserver.json')
    codegenerator('schemaserver.json', 'Generator/Config', 'CSharpGeneratorForProton.Json', 'Config') 
    
def exportclient():
  export(EXPORT_FILES + EXPORT_CLIENT_ONLY, 'lua', 'client', 'config_client', 'Template', None)
    
def main():
  try:
    proto = False
    args = sys.argv
    if len(args) >= 2:
      proto = True
    readconfig()
    readxlsx(proto)
    exportserver(proto)
    #exportclient()
    print("all operation finish successful")
    return 0
  except ExportError as e:
    print(e)
    print("has error, see logs, please return key to exit")
    input()
    return 1
  except Exception as e:
    traceback.print_exc()
    print("has error, see logs, please return key to exit")
    input()
    return 1
    
if __name__ == '__main__':
    sys.exit(main())
