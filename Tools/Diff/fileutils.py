#!/usr/bin/python
# -*- coding: UTF-8 -*-

import os
import sys
import shutil

# 分隔符 win32 '\\' mac & linux '/'
def separator(dir):
    if sys.platform == 'win32':
        dir = dir.replace('/', '\\')
    else:
        dir = dir.replace('\\', '/')
    return dir

# 遍历文件
# dir 文件夹名称
# callback 遍历回调 def callback(file):
# suffixs 忽略后缀 suffixs = ['.png']
def walkfile(dir, callback = None, suffixs = None):
    dir = separator(dir)
    if not os.path.isdir(dir):
        print ("dir not exist[%s]" % dir)
        return

    for root, dirs, files in os.walk(dir):

        # root 表示当前正在访问的文件夹路径
        # dirs 表示该文件夹下的子目录名list
        # files 表示该文件夹下的文件list

        # 遍历文件
        for f in files:
            #print(os.path.join(root, f))
            has = False
            if suffixs != None and len(suffixs) > 0:
                for s in suffixs:
                    if f.endswith(s):
                        has = True
                        break
            else:
                has = True

            if callback != None and has:
                callback(os.path.join(root, f))

    print ("walk[%s] finish ." % dir)

# 遍历文件夹
# dir 文件夹名称
# callback 遍历回调 def callback(file):
def walkdir(dir, callback = None):
    dir = separator(dir)
    if not os.path.isdir(dir):
        print ("dir not exist[%s]" % dir)
        return

    for root, dirs, files in os.walk(dir):

        # root 表示当前正在访问的文件夹路径
        # dirs 表示该文件夹下的子目录名list
        # files 表示该文件夹下的文件list
        # 遍历所有的文件夹
        for d in dirs:
            #print(os.path.join(root, d))
            if callback != None:
                callback(os.path.join(root, d))

    print ("walk[%s] finish ." % dir)

# 删除目录
def rmtree(dir):
    ret = False
    if os.path.isdir(dir):
        for root, dirs, files in os.walk(dir, topdown=False):
            for name in files:
                filepath = os.path.join(root, name)
                os.remove(filepath)
            for name in dirs:
                os.rmdir(os.path.join(root, name))
        ret = True
    return ret

# 清空目录
def cleartree(dir):
    ret = False
    if rmtree(dir):
        if not os.path.isdir(dir):
            os.makedirs(dir)
        ret = True
    return ret

# 拷贝目录
def copytree(src, dst):
    ret = False
    if os.path.isfile(src):
        ret = copyfile(src, dst)
    if os.path.isdir(src):
        dirs = os.listdir(src)
        for d in dirs:
            _src = os.path.join(src, d)
            _dst = os.path.join(dst, d)
            ret = copytree(_src, _dst)
    return ret

# 拷贝文件
def copyfile(src, dst):
    try:
        ret = False
        if os.path.isfile(src):
            dir = os.path.dirname(dst)
            if not os.path.isdir(dir):
                os.makedirs(dir)
            shutil.copyfile(src, dst)
            ret = True
        return ret
    except IOError:
        print ('copyfile [%s] => [%s] is error .' % (src, dst))
        return False
        pass

# 删除文件
def rmfile(file):
    ret = False
    if os.path.isfile(file):
        os.remove(file)
        ret = True
    return ret

################################################

if __name__ == '__main__':
    copyfile("E:\\tmp\\test\\config_demo.lua", "E:\\tmp\\test2\\config_demo.lua")