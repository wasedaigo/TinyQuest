from google.appengine.api import memcache
from db_model import *
from serialize_commands import *
import const
import logging

def get_provider_list():
  key = "provider_list"
  data = memcache.get(key)
  if data is not None:
    return data
  else:
    data = []
    for p in const.openIdProviders:
        name = p.split('.')[0]
        url = users.create_login_url(p, p, p)
        data.append({"name":name, "url":url })

    if not memcache.add(key, data, const.cache_time):
      logging.error("[get_provider_list] Memcache failed")
    
  return data
