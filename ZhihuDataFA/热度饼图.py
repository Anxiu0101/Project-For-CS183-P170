'''
Function: Zhihu hotspot data  visualization
'''
import matplotlib.pyplot as plt
import numpy as np
import xlrd
#打开表格并准备获取数据 Open the xlsx and prepare to get the data
data = xlrd.open_workbook('data.xlsx')
table = data.sheet_by_name('Sheet 1')

N =table.nrows
ind = np.arange(N)
width = 0.35
dOneT=0
dFiveH=0
dOneH=0
dmin=0

#统计数据 statistical data
for i in range(N):
    if i>0:
        d=table.cell(i,2).value
        d=d[0:-3]
        da=int(d)
        if(da>=1000):
            dOneT=dOneT+1
        elif (500<=da<1000):
            dFiveH=dFiveH+1
        elif 100<=dOneH<500:
            dOneH=dOneH+1
        else:
            dmin=dmin+1

fig, ax = plt.subplots(figsize=(6, 3), subplot_kw=dict(aspect="equal"))
data=[dOneT,dFiveH,dOneH,dmin]
ingredients = ["hot>=1000","1000>hot>=500","500>hot>=100","hot<100"]
#处理数据 Processing data
def func(pct, allvals):
    absolute = int(round(pct/100.*np.sum(allvals)))
    return "{:.1f}%({:d})".format(pct, absolute)


wedges, texts, autotexts = ax.pie(data, autopct=lambda pct: func(pct, data),
                                  textprops=dict(color="w"))
ax.legend(wedges, ingredients,
          title="Ingredients",
          loc="center left",
          bbox_to_anchor=(1, 0, 0.5, 1))

plt.setp(autotexts, size=8, weight="bold")

ax.set_title("hot ratio in the Top 50")

plt.show()
            
        
