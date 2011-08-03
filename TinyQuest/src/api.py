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

from db_model import *
from enemy_factory import *
from cachable_commands import *
from game_commands import *
from serialize_commands import *

logging.getLogger().setLevel(logging.INFO)

class ApiServer(webapp.RequestHandler):  

  # for debug
  def check_stats(self):
    text = "Result:<br />"
    text += "Cache used"
    text += simplejson.dumps(memcache.get_stats())
    
    self.response.out.write(text)

  # for debug
  def clear_cache(self):
    memcache.flush_all()
    self.response.out.write("Cache Cleared")

  def get_login_success_info(self, player):
    result = {}

    result["success"] = True
    result["admin"] = users.IsCurrentUserAdmin()
    result["login"] = True
    result["player"] = SerializeCommands.serialize_player(player)
    result["logoutURL"] = users.CreateLogoutURL("/")
    
    self.response.out.write(simplejson.dumps(result))

  def get_login_fail_info(self):
    result = {}
    result["success"] = True
    result["login"] = False
    result["providers"] = get_provider_list()
    
    self.response.out.write(simplejson.dumps(result))

  def go(self, player):   
    
    result = {"success" : False}
    adventure_scene = GameCommands.get_adventure_scene_by_player(player)
    if adventure_scene and player.energy >= 1:
        if adventure_scene.scene_type == "enemy":
            enemy = adventure_scene.target
            player.energy -= 1
            GameCommands.proceed_combat(enemy, player)

            if enemy.hp <= 0:
                GameCommands.proceed_step(player, adventure_scene.step)
                result["enemy_life"] = 0
                result["success"] = True
            else:
                result["enemy_life"] = enemy.hp / float(enemy.max_hp)
                result["success"] = True
            enemy.put()
            player.put()

    self.response.out.write(simplejson.dumps(result))
  
  def charge_energy(self, player):
    player.energy = player.max_energy
    player.put()
    self.response.out.write(simplejson.dumps({"success" : True}))
    
  def get_current_adventure_scene(self, player):
    adventure_scene = GameCommands.get_adventure_scene_by_player(player)

    if not adventure_scene:
        enemy = EnemyFactory.build_enemy("dragon", 13, 120, 234, 134)
        adventure_scene = GameCommands.update_adventure_scene_with_player(player, "enemy", enemy, 1, 1)

    result = {}
    result["hp"] = player.hp
    result["energy"] = player.energy
    result["floor"] = adventure_scene.floor
    result["step"] = adventure_scene.step
    result["scene_type"] = adventure_scene.scene_type
    if adventure_scene.scene_type == "enemy":
        result["target"] = SerializeCommands.serialize_enemy(adventure_scene.target)
    result["success"] = True;

    self.response.out.write(simplejson.dumps(result))

  def get(self):
      user = users.get_current_user()

      if user:
        if self.request.path.startswith('/api/check_stats'):
          self.check_stats()
        elif self.request.path.startswith('/api/clear_cache'):
          self.clear_cache()
        else:
          player = GameCommands.get_player(user)
          if not player:
            player = GameCommands.setup_new_account(user)

          if player:
            if self.request.path.startswith('/api/GetLoginInfo'):
              self.get_login_success_info(player)
            elif self.request.path.startswith('/api/GetCurrentAdventureScene'):
              self.get_current_adventure_scene(player)
            elif self.request.path.startswith('/api/Go'):
              self.go(player)
            elif self.request.path.startswith('/api/ChargeEnergy'):
              self.charge_energy(player)
      else:
        self.get_login_fail_info()