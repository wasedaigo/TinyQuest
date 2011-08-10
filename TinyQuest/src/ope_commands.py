#!/usr/bin/env python
# -*- coding: utf-8 -*-

import os
import sys

from google.appengine.ext import webapp
from google.appengine.ext.webapp import template
from google.appengine.api import memcache
from db_model import *

class OpeCommands:
  @classmethod
  def clean_up_entity(cls, entity):
    query = entity.all()
    results = query.fetch(200)
    while results:
      db.delete(results)
      results = query.fetch(1000)

  @classmethod
  def clean_up_data(cls):
    cls.clean_up_entity(PlayerModel)
    cls.clean_up_entity(ItemModel)
    cls.clean_up_entity(LocalizedStringModel)
    cls.clean_up_entity(ActiveSceneModel)
    cls.clean_up_entity(EnemyModel)
    memcache.flush_all()

  @classmethod
  def create_master_data(cls):
    cls.clean_up_data()

    
    #map10Name = LocalizedStringModel(ja = u'霧の谷', en = "map4", de = "Map4").put()
