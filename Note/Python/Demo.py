# -*- coding:utf-8 -*-
import requests

url_baidu='https://www.baidu.com/'
url_baidu_s='https://www.baidu.com/s?'
url_weibo='https://weibo.com/'
wd={'wd':'python'}
resp1=requests.get(url_baidu)
resp2=requests.get(url_baidu_s,params=wd)
#response=requests.get("https://weibo.com")


print(resp1.status_code)
print(resp1.cookies)
print(resp1.text)
print(resp2.text)