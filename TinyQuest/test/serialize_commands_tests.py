import unittest
import logging
from google.appengine.ext import db

from enemy_factory import *
from serialize_commands import *

class SerializeCommandTest(unittest.TestCase):

    def setUp(self):
        pass

    def tearDown(self):
        pass

    def test_seliarize_enemy(self):
        enemy = EnemyFactory.build_enemy("dragon", 1025, 234, 134)
        hash = SerializeCommands.serialize_enemy(enemy)
        self.assertEquals(hash["tag"], "dragon")
        self.assertEquals(hash["life"], 1025)
        self.assertEquals(hash["attack"], 234)
        self.assertEquals(hash["defense"], 134)
