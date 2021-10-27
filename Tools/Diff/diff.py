#!/usr/bin/env python
#coding:utf8

#比较文件夹的不同,输出不一样的文件

import os
import sys
import fileutils
import gen_md5

JSON_SUFFIX = 'md5.json'

# md5对比
def compare(a, b):
    return a == b

# 检查图片后缀
def check_suffix(k):
    return k.endswith('.jpg') or k.endswith('.jpeg') or k.endswith('.png')

def check_lua_suffix(k):
    return k.endswith('.lua')

# 比较文件不同
# src 原路径
# dst 目标路径
# out 输出路径
# rmdst 删除目标目录不存在的源文件达到同步效果
# mode 0 所有文件 1 图片资源 2 除了图片资源
# crc 是否生成文件CRC路径
def diff(src, dst, out, rmdst=False, mode=0, crc=False):
    is_diff = False
    src_files = {}
    dst_files = {}
    json_name = JSON_SUFFIX
    src_json = os.path.join(src, json_name)
    dst_json = os.path.join(dst, json_name)

    if os.path.isfile(src_json):
        fileutils.rmfile(src_json)
    if os.path.isfile(dst_json):
        fileutils.rmfile(dst_json)
    
    if not gen_md5.make(src, src):
        print ('gen_md5 [%s] error .' % src)
        return is_diff
    if not gen_md5.make(dst, dst):
        print ('gen_md5 [%s] error .' % dst)
        return is_diff

    src_files = gen_md5.readmd5(src)
    dst_files = gen_md5.readmd5(dst)

    # 遍历源文件
    op_count = 0
    op_len   = len(src_files)
    for k, v in src_files.items():
        name = k
        file_src = src + name
        a = v
        b = ''

        # 源路径是否被新路径包含 => 可以处理新增文件但是 处理不了删除文件
        if k in dst_files:
            b = dst_files[k]
            # 剔除包含文件剩下的就是删除文件
            if rmdst:
                dst_files.pop(k)

        # 图片文件后缀检查
        if mode == 1:
            if not check_suffix(k):
                continue
        elif mode == 2:
            if check_suffix(k):
                continue
            pass
        # 检查是否是lua
        if not check_lua_suffix(k):
            continue

        # 比较md5是否相等 => 判断新增和修改 处理不了删除
        is_comp = compare(a, b)
        if not is_comp:
            sub_name = name
            file = out + sub_name + '.bytes'
            print ('cp file[%s] => [%s].' % (name, file))
            fileutils.copyfile(file_src, file)
            is_diff = True

        op_count = op_count + 1
        if (op_count % 1000) == 0:
            print ('md5 compare ==> %s/%s' % (op_count, op_len))
    
    # 遍历目标所剩文件 => 还有剩余则进行删除
    if rmdst:
        for k, v in dst_files.items():
            sub_name = k
            file = dst + sub_name
            print ('rm file[%s] .' % file)
            fileutils.rmfile(file)

    if is_diff:
        fileutils.copyfile(src_json, dst_json)
    
    return is_diff

def useage():
    print ('python diff.py [src] [dst]')
    pass

if __name__ == '__main__':
    #src = 'E:\\git\\sszg\\client\\src'
    #dst = 'E:\\sszg_tmp\\backup20200901\\client\\src'
    #out = 'E:\\tmp\\diff'
    args = sys.argv
    if len(args) <= 2:
        useage()
        pass
    else:
        src = args[1]
        dst = args[2]
        out = dst
        if len(args) > 3:
            out = args[3]
        diff(src, dst, out)
    pass