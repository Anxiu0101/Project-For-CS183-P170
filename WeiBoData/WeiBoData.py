# -*- ecoding: utf-8 -*-
# @ModuleName: WeiBoData
# @Function: 
# @Author: Anxiu
# @Time: 2021/6/13 22:40

from pyquery import PyQuery as pq
from wordcloud import WordCloud
import csv
import jieba
import os
import sys

# import time
'''
Author: Xueyi.Chen
Date: 2021-6-6
'''
# 这里需要用到两个python库
# PyQuery用来爬取微博热搜版的界面以及解析下载下来的html文件
# csv用于数据导入到csv表格方便数据的存储
if __name__ == '__main__':
    if(len(sys.argv) != 2): # ensure only ONE path be given via console 
        print("Argument Error")
        exit(-1)

    # localtime = time.strftime('%Y%m%d_%H%M%S', time.localtime(time.time()))
    # path = os.path.join(sys.argv[1], localtime)
    path = sys.argv[1]
    # os.makedirs(path)
    html = pq("https://s.weibo.com/top/summary/")
    # 讲微博热搜的html源码下载解析
    headers = ['排名', '热搜', '热度']
    # 设置表头
    if os.path.exists(os.path.join(path, 'weibo.csv')):
        os.remove('weibo.csv')
    with open(os.path.join(path, 'weibo.csv'), "a", encoding="utf-8", newline='') as f:
        # 打开/新建一个csv文件并规定以'utf-8'编码
        csv.writer(f).writerow(headers)
        # 将表头作为该表格的首行写入

        for item in html("#pl_top_realtimehot > table > tbody > tr").items():
            # 遍历html源码文件中的热搜项目，按照所需数据在html中的位置逐一输出
            if item('td.td-01.ranktop').text() != '':
                row = [item('td.td-01.ranktop').text(), item('td.td-02 > a').text(), item('td.td-02 > span').text()]
                # 将50个热搜逐一写入csv文档中
                csv.writer(f).writerow(row)
            # 特殊置顶话题特殊处理
            else:
                row = ['特别报道', item('td.td-02 > a').text(), '置顶话题']
                csv.writer(f).writerow(row)

    # 打开热搜文件
    with open(os.path.join(path, 'weibo.csv'), 'r', encoding='utf-8') as f:
        reader = csv.DictReader(f)
        column = [row['热搜'] for row in reader]
        # 将热搜内容提取出来
        text = ",".join(column)
        # 第一张的词由内容交给jieba库自动分解成
        words = jieba.cut(text)
        # 第二张的词由热搜本身组成
        word1 = " ".join(words)
        delete = ['的', '了', '啊', '在', '是', '被', '吧', '把']
        for i in delete:
            word1 = word1.replace(i, '')
        word2 = " ".join(column)
        wc = WordCloud(
            font_path="msyh.ttc",
            background_color='white',
            width=1000,
            height=800,
            max_words=1000,
        )
        # 设置云图的格式
        wc.generate(word1) # modified so that the path can be given external
        wc.to_file(os.path.join(path, 'Weibo1.png'))
        wc.generate(word2)
        wc.to_file(os.path.join(path, 'Weibo2.png'))
        # 分别生成并保存图片
        f.close()