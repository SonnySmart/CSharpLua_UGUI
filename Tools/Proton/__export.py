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

# do not modify the following

import os
import platform
import traceback
import shutil
import sys

exportscript = './proton.py'     
exportconfig = './__export.txt'
pythonpath = 'sample\\tools\\py37\\py37.exe ' if platform.system() == 'Windows' else 'python '
csprotonpath = 'sample\\tools\\CSharpGeneratorForProton\\CSharpGeneratorForProton.exe '
xlsxfolder = './sample' # 默认xlsx路径
xlsxdatafolder = '../../Assets/ResHotfix/' # 默认xlsx输出路径
xlsxcsfolder = '../../Assets/Scripts/Compiled/' # 默认xlsx->c#输出路径

class ExportError(Exception):
  pass

def readconfig():
  global xlsxfolder
  if not os.path.isfile(exportconfig):
    with open(exportconfig, 'w') as f:
      f.write(xlsxfolder)
      pass
  with open(exportconfig, 'r') as f:
    xlsxfolder = f.readline()
    pass
  print ('xlsxfolder ->' , xlsxfolder)
  pass

def readxlsx():
  for root, dirs, files in os.walk(xlsxfolder):
    for f in files:
      if not f.endswith('.xlsx'):
        continue
      if '~$' in f:
        continue
      path = os.path.join(root, f)
      EXPORT_FILES.append(path)
      pass
    pass

def export(filelist, format, sign, outfolder, suffix, schema):
  outfolder = xlsxdatafolder + outfolder
  cmd = r' -p "' + ','.join(filelist) + '" -f ' + outfolder + ' -e ' + format + ' -s ' + sign
  if suffix:
    cmd += ' -t ' + suffix
  if schema:
    cmd += ' -c ' + schema
  cmd = pythonpath + exportscript + cmd
  code = os.system(cmd)
  if code != 0:
    raise ExportError('export excel fail, please see print')

def codegenerator(schema, outfolder, namespace, suffix):
  outfolder = xlsxcsfolder + outfolder
  if os.path.exists(schema):
    cmd = csprotonpath + '-n ' + namespace + ' -f ' + outfolder + ' -p ' + schema
    if suffix:
      cmd += ' -t ' + suffix 
    code = os.system(cmd)
    os.remove(schema)      
    if code != 0:
      raise ExportError('codegenerator fail, please see print')
        
def exportserver():
  export(EXPORT_FILES + EXPORT_SERVER_ONLY, 'json', 'server', 'ConfigGenerator', 'Config', 'schemaserver.json')
  codegenerator('schemaserver.json', 'ConfigGenerator', 'CSharpGeneratorForProton.Json', 'Config') 
    
def exportclient():
  export(EXPORT_FILES + EXPORT_CLIENT_ONLY, 'lua', 'client', 'config_client', 'Template', None)
    
def main():
  try:
    readconfig()
    readxlsx()
    exportserver()
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
