# -*- coding: utf-8 -*-
import requests

response=requests.get("https://weibo.com")

print(response.status_code)
print(response.cookies)
print(response.text)
