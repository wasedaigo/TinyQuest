#!/usr/bin/env python
# -*- coding: utf-8 -*-

import os

from google.appengine.api import users
from google.appengine.ext import webapp

from django.utils import simplejson
from django.core import serializers

import logging

# App Engine imports
from google.appengine.ext import webapp
from google.appengine.api import memcache

logging.getLogger().setLevel(logging.INFO)

class ApiServer(webapp.RequestHandler):  
  def start_battle(self):
    game_no = "1"
    
    assignedGroupNo = 0
    if memcache.get(game_no):
      memcache.delete(game_no)
      assignedGroupNo = 1
    else:
      memcache.add(game_no, True, 60)

    result = '{"assignedGroupNo":'+str(assignedGroupNo)+', "gameNo":1, "combatUnitGroups":[{"combatUnits":[{"buffs":[],"groupType":0,"index":0,"hp":990,"userUnit":{"id":1,"unit":1,"exp":1,"skillExp":1}},{"buffs":[],"groupType":0,"index":1,"hp":100,"userUnit":{"id":2,"unit":1,"exp":1,"skillExp":1}},{"buffs":[],"groupType":0,"index":2,"hp":60,"userUnit":{"id":2,"unit":3,"exp":1,"skillExp":1}},{"buffs":[],"groupType":0,"index":3,"hp":100,"userUnit":{"id":3,"unit":6,"exp":1,"skillExp":1}},{"buffs":[],"groupType":0,"index":4,"hp":150,"userUnit":{"id":4,"unit":6,"exp":1,"skillExp":1}},{"buffs":[],"groupType":0,"index":5,"hp":200,"userUnit":{"id":5,"unit":1,"exp":1,"skillExp":1}}],"activeIndex":0},{"combatUnits":[{"buffs":[],"groupType":1,"index":0,"hp":120,"userUnit":{"id":7,"unit":2,"exp":1,"skillExp":1}},{"buffs":[],"groupType":1,"index":1,"hp":100,"userUnit":{"id":8,"unit":1,"exp":1,"skillExp":1}},{"buffs":[],"groupType":1,"index":2,"hp":60,"userUnit":{"id":9,"unit":1,"exp":1,"skillExp":1}},{"buffs":[],"groupType":1,"index":3,"hp":100,"userUnit":{"id":10,"unit":3,"exp":1,"skillExp":1}},{"buffs":[],"groupType":1,"index":4,"hp":150,"userUnit":{"id":11,"unit":4,"exp":1,"skillExp":1}},{"buffs":[],"groupType":1,"index":5,"hp":200,"userUnit":{"id":12,"unit":5,"exp":1,"skillExp":1}}],"activeIndex":0}]}'
    self.response.out.write(result)#simplejson.dumps(result))

  def progress_turn(self):
    playerGroupType = (int)(self.request.get('playerGroupType'))
    playerIndex = self.request.get('playerIndex')
    turn = self.request.get('turn')
    game_no = "1"

    keys = [game_no + "_0_" + turn, game_no + "_1_" + turn]
    
    player_indexes = [None, None]
    player_indexes[0] = memcache.get(keys[0])
    player_indexes[1] = memcache.get(keys[1])

    if not player_indexes[playerGroupType]:
      if (memcache.add(keys[playerGroupType], playerIndex, 60)):
        player_indexes[playerGroupType] = playerIndex

    result = {}
    if player_indexes[0] and player_indexes[1]:
      opponentGroupType = 1 - playerGroupType
      result["opponentIndex"] = player_indexes[opponentGroupType]
      result["valid"] = True
    else:
      result["valid"] = False

    self.response.out.write(simplejson.dumps(result))

  def get(self):
    if self.request.path.startswith('/api/start_battle'):
      self.start_battle();
    elif self.request.path.startswith('/api/progress_turn'):
      self.progress_turn()
   
  def post(self):
    if self.request.path.startswith('/api/progress_turn'):
      self.progress_turn()   
