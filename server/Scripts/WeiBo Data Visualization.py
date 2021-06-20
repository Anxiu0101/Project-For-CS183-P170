import matplotlib.pyplot as plt
import matplotlib.gridspec as gridspec
import pandas as pd
import numpy as np
import sys
import os

if(len(sys.argv) != 2):
    exit(-1)

# Read the csv file about the information of WeiBo hotspot
UnprocessedWeiBo = pd.read_csv(os.path.join(sys.argv[1], 'weibo.csv'), header=1,
                    names=["rank", "topic", "hotValue"])
print(UnprocessedWeiBo)
print(UnprocessedWeiBo.info())

# Solve the error data
WeiBo = UnprocessedWeiBo.dropna(axis=0, how='any')
print(WeiBo)

# Some initialization setting
plt.rcParams["font.family"] = "STSong"
plt.style.use('bmh')
plt.figure(figsize=(15, 5))
plt.title("WeiBo hotspot today", fontsize=12)
plt.ylabel("Hotspot Value", fontsize=10, fontstyle='italic')

# Set the pyplot area
gs = gridspec.GridSpec(1, 2)

# bar picture
ax1 = plt.subplot(gs[0, 0])
for i in range(50):
    x = WeiBo.iloc[i, 0]
    y = WeiBo.iloc[i, 2]
    ax1.axhline()
    ax1.bar(x, y, ls='-', width=0.75, align='center', linewidth='100')
ax1.axis([0, 51, 10000, 5000000])

# spot picture
ax2 = ax1.twinx()
n = np.arange(50)
for i in n:
    ax2.plot(WeiBo.iloc[n, 0], WeiBo.iloc[n, 2], '.c-')

# pie picture
ax3 = plt.subplot(gs[0, 1])
n = np.arange(10)
for i in n:
    ax3.pie(WeiBo.iloc[n, 2], labels=WeiBo.iloc[n, 1],
            explode=[0.12, 0.08, 0.04, 0, 0, 0, 0, 0, 0, 0],
            colors=['#DAE2F8', '#12c2e9', '#757F9A', '#6DD5FA', '#24C6DC',
                    '#4286f4', '#93EDC7', '#86A8E7', '#91EAE4', '#348AC7'])

# store the picture and show
plt.axis('equal')
plt.tight_layout()
plt.savefig(os.path.join(sys.argv[1], './WeiBoResult.jpg'))
# plt.show()
# plt.close()
