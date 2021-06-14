# -*- coding: utf-8 -*-  
import mechanize
import csv
import json
import sys
from bs4 import BeautifulSoup

if len(sys.argv) != 2: #comfire console arguments is correct
    print(sys.argv)
    print('Argument Error')
    exit(-1)

agent = mechanize.Browser() # create an mechanize to get data through http
agent.addheaders = [('User-agent', 'Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.9.1.11) Gecko/20100701 Firefox/3.5.11')] # by-pass the UA filter of zhihu
agent.set_handle_robots(False)# by-pass the robots.txt of zhihu

stream = agent.open('https://www.zhihu.com/api/v3/feed/topstory/hot-lists/total?limit=50&desktop=true') #get data via api
doc = stream.read()

data = json.loads(doc) #parse json

# wb = xlsxwriter.Workbook(sys.argv[1]) #create the target file by given filename
# ws = wb.add_worksheet('Sheet 1')
f = open(sys.argv[1], 'w', encoding='utf_8')
ws = csv.writer(f)
# i = 0 #the row needs to write
ws.writerow(['id', 'answer_count', 'hot_score', 'title', 'desc']) #table header
# i = i + 1 #next row

for entry in data['data']: # here, the asyncio may get better performance
    target = entry['target']
    detail = agent.open('https://www.zhihu.com/question/' + str(target['id'])) #since need to fetch lable data and no api to do so, get the whole webpage to parse
    soup = BeautifulSoup(detail.read(), 'html5lib') #to parse html
    tags = []
    #print(target['id']) #only for debug
    for e in soup.find('div', {'class': 'QuestionHeader-topics'}).findAll('div', {'class': 'Popover'}): #parse html webpage, fetch data we actually need
        tags.append(e.contents[0].get_text()) #todo: more data can be collect here, extend if needed
    ws.writerow([target['id'], target['answer_count'], entry['detail_text'], target['title'], target['excerpt']] + tags) #write data
    # i = i + 1 #next row

f.close()
