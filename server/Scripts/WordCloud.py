'''
Function: Zhihu wordclloud
Author: Qihong.Yang
'''
import matplotlib.pyplot as plt
import numpy as np
import csv
import jieba
import wordcloud
import os
import sys

if(len(sys.argv) != 2):
    exit(-1)

ls=[]#创建列表 creat list
cntls = []
n=0
with open(os.path.join(sys.argv[1], 'zhihu.csv'), 'r',encoding='utf_8') as f:#获取热榜数据  Get hot list data
    reader = csv.reader(f)
    for i in reader:
        if(len(i) != 0):
            ls=ls+i[5:-1]
            cntls+=[i[4]]
        n=n+1

txt = " ".join(ls)
cnts = jieba.lcut(",".join(cntls))
stwds = ['的', '呢', '啊', '呀', '吧', '之', '正在', '中', '有',
         '是', '做', '着', '正', '在', '呵', '了', '被', '把',
         '其', '这', '这个', '那', '那个', '为', '为了', '由于', '也',
         '通过', '经由', '和', '与', '作为', '像', '如同', '我', '不'] # structure words, not need in key words
for wd in stwds:
    for cnt in cnts:
        if(wd == cnt):
            cnts.remove(cnt)
txt.replace('热点话题', '') # since most topic is hot topic, this label is a polution
w = wordcloud.WordCloud(font_path = "msyh.ttc",width = 1000, height = 700, background_color = "white", max_words=1000)#设置词云图参数 Setting the parameters of word cloud
w.generate(txt)
w.to_file(os.path.join(sys.argv[1], "zhihu_wordcloud.png"))#输出词云图 output the result
w.generate(" ".join(cnts))
w.to_file(os.path.join(sys.argv[1], "zhihu_wordcloud2.png"))#Zhihu question description word cloud
