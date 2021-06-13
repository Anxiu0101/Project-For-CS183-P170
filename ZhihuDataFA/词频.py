'''
Function: Zhihu wordclloud
'''
import matplotlib.pyplot as plt
import numpy as np
import xlrd
import jieba
import wordcloud
data = xlrd.open_workbook('data.xlsx')#打开表格 open the xlsx
table = data.sheet_by_name('Sheet 1')#获取sheet1的数据 get the data from sheet 1
ls=[]#创建列表 creat list

for i in range(51):#获取热榜数据  Get hot list data
    a=0
    for j in table.row_values(i):
       if(a>4):#定向到标签数据 Directed to tag data
           ls.append(j)
       a=a+1

txt = " ".join(ls)
w = wordcloud.WordCloud(font_path = "msyh.ttc",width = 1000, height = 700, background_color = "white")#设置词云图参数 Setting the parameters of word cloud
w.generate(txt)
w.to_file("wordcloud.png")#输出词云图 output the result

