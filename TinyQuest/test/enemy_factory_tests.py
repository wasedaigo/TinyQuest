import unittest
import logging
from google.appengine.ext import db
import db_model
from enemy_factory import *

class EnemyFactoryTest(unittest.TestCase):

    def setUp(self):
        pass
        
    def tearDown(self):
        pass

    def test_build_enemy(self):
        enemy = EnemyFactory.build_enemy("dragon", 13, 1025, 234, 134)
        self.assertEquals(enemy.tag, "dragon")
        self.assertEquals(enemy.lv, 13)
        self.assertEquals(enemy.hp, 1025)
        self.assertEquals(enemy.attack, 234)
        self.assertEquals(enemy.defense, 134)