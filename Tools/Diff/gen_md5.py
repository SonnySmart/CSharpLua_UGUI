#!/usr/bin/env python
#coding:utf8
# -*- coding: utf-8 -*-  

import os
import json

EXEC = 'gen_md5.exe'
MD5_FILE = 'md5.json'

def make(src, dst, name = MD5_FILE):
    dir = os.path.dirname(os.path.abspath(__file__))
    exec_path = os.path.join(dir, EXEC)
    jsonfile = os.path.join(dst, name)
    ret = False
    cmd = '%s %s %s %s' % (exec_path, src, dst, name)
    try:
        if os.path.isfile(jsonfile):
            ret = True
            return ret
            
        if os.system(cmd) == 0:
            ret = True
        pass
    except Exception as e:
        print (e)
        pass
    return ret

def readmd5(dir):
    file = os.path.join(dir, MD5_FILE)
    md5s = {}
    if os.path.isfile(file):
        with open(file, 'rb') as f:
            jsonstr = f.read()
            md5s = json.loads(jsonstr)
        pass
    return md5s