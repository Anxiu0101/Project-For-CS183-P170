import matplotlib.pyplot as plt
import numpy as np
import csv
import sys
import os

if(len(sys.argv) != 2):
    exit(-1)

N =51
ind = np.arange(N)
width = 0.35
title=[]
hot=[]
dOneT=0
dFiveH=0
dOneH=0
dmin=0

for i in range(N):#获取排名 get rank
    if i>0:
        
        title.append(i)
n=0
with open(os.path.join(sys.argv[1], 'zhihu.csv'), 'r',encoding='utf_8') as f:
     reader = csv.reader(f)
     for i in reader:
         if(n%2==0 and n>0):
             hot.append(int(i[2][0:-3]))
             d=i[2][0:-3]
             da=int(d)
             if(da>=1000):
                 dOneT=dOneT+1
             elif (500<=da<1000):
                 dFiveH=dFiveH+1
             elif 100<=dOneH<500:
                 dOneH=dOneH+1
             else:
                 dmin=dmin+1
             
         n=n+1



plt.rcdefaults()
plt.figure(figsize=(15, 5))
plt.rcParams['font.sans-serif']=['STSong'] #用来正常显示中文 Used to display Chinese normally
plt.figure(1)
ax = plt.subplot(121)

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




plt.figure(1)

ax1 = plt.subplot(122)
data=[dOneT,dFiveH,dOneH,dmin]
ingredients = ["hot>=1000","1000>hot>=500","500>hot>=100","hot<100"]
#处理数据 Processing data
def func(pct, allvals):
    absolute = int(round(pct/100.*np.sum(allvals)))
    return "{:.1f}%({:d})".format(pct, absolute)


wedges, texts, autotexts = ax1.pie(data, autopct=lambda pct: func(pct, data),
                                  textprops=dict(color="w"))
ax1.legend(wedges, ingredients,
          title="Ingredients",
          loc="center left",
          bbox_to_anchor=(1, 0, 0.5, 1))

plt.setp(autotexts, size=8, weight="bold")

ax1.set_title("hot ratio in the Top 50")

plt.savefig(os.path.join(sys.argv[1], 'zhihu_hot.png'))

