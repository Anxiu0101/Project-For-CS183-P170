'''
Function: Zhihu hotspot data  visualization
'''
import matplotlib.pyplot as plt
import numpy as np
import xlrd
data = xlrd.open_workbook('data.xlsx')#打开表格
table = data.sheet_by_name('Sheet 1')#获取sheet1的数据 get the data from sheet 1
N =table.nrows#获取有多少行 Get how many rows there are
ind = np.arange(N)
width = 0.35
title=[]
hot=[]

for i in range(N):#获取排名 get rank
    if i>0:
        title.append(i)
for i in range(N):#获取热度数据 get data about hot
    if i>0:
        da=table.cell(i,2).value
        da=da[0:-3]
        hot.append(int(da))


plt.rcdefaults()

plt.rcParams['font.sans-serif']=['STSong'] #用来正常显示中文 Used to display Chinese normally
fig, ax = plt.subplots()

# 设置参数 Set parameters
yTitle = title
y_pos = np.arange(len(hot))

error = np.random.rand(len(yTitle))

ax.barh(y_pos, hot, xerr=error, align='center')
ax.set_yticks(y_pos)
ax.set_yticklabels(yTitle)
ax.invert_yaxis()  # labels read top-to-bottom
ax.set_xlabel('热度hot(万ten thousand)')
ax.set_ylabel('排行rank')
ax.set_title('知乎zhihu')
for x,y in zip(title,hot):
    ax.text(y+100,x-0.5,y,fontsize=8,horizontalalignment='center')

plt.show()
