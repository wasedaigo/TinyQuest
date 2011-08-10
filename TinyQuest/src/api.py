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
    result["logoutURL"] = users.create_logout_url("/")
    
    self.response.out.write(simplejson.dumps(result))

  def get_login_fail_info(self):
    result = {}
    result["success"] = True
    result["login"] = False
    result["loginURL"] = users.create_login_url("/")
    
    self.response.out.write(simplejson.dumps(result))

  def go(self, player):   
    
    result = {"success" : False}
    active_scene = GameCommands.get_active_scene_by_player(player)
    if active_scene and player.energy >= 1:
        if active_scene.scene_type == "enemy":
            enemy = active_scene.target
            player.energy -= 1
            result["combat_result"] = GameCommands.proceed_combat(enemy, player)

            if enemy.life <= 0:
                GameCommands.proceed_step(player, active_scene.step)
            #else if player.life <= 0:
                # Game Over
                #GameCommands.game_over(player)

            result["enemy_hp"] = enemy.life
            result["success"] = True
            enemy.put()
            player.put()

    self.response.out.write(simplejson.dumps(result))
  
  def charge_energy(self, player):
    GameCommands.charge_energy(player)
    player.put()
    self.response.out.write(simplejson.dumps({"success" : True}))
    
  def get_current_active_scene(self, player):
    active_scene = GameCommands.get_active_scene_by_player(player)

    if not active_scene:
        enemy = EnemyFactory.build_enemy("dragon", 120, 234, 134)
        active_scene = GameCommands.update_active_scene_with_player(player, "enemy", enemy, 1, 1)

    result = {}
    
    result = {
        "life": player.life,
        "energy": player.energy,
        "floor": active_scene.floor,
        "step": active_scene.step,
        "scene_type": active_scene.scene_type,
        "success": True,
    
    }
    
    if active_scene.scene_type == "enemy":
        result["target"] = SerializeCommands.serialize_enemy(active_scene.target)

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
            elif self.request.path.startswith('/api/GetCurrentActiveScene'):
              self.get_current_active_scene(player)
            elif self.request.path.startswith('/api/Go'):
              self.go(player)
            elif self.request.path.startswith('/api/ChargeEnergy'):
              self.charge_energy(player)
      else:
        self.get_login_fail_info()