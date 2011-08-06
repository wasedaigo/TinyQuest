import unittest
import logging
from google.appengine.ext import db
import db_model
from game_commands import *
from ope_commands import *
from user_mock import UserMock

class TestModel(db.Model):
    number = db.IntegerProperty()

class GameCommandTest(unittest.TestCase):

    def setUp(self):
        # Populate test entities.
        self.user = UserMock("user_id_test", "Daigo")
        self.test_model1 = TestModel(number = 128).put()
        self.test_model2 = TestModel(number = 134).put()
        
    def tearDown(self):
        # There is no need to delete test entities.
        pass

    def test_get_player(self):
        player0 = GameCommands.get_player(self.user)
        self.assertEquals(player0, None)
        
        player1 = GameCommands.setup_new_account(self.user)
        player2 = GameCommands.get_player(self.user)
        
        self.assertEquals(player1.user_id, player2.user_id)
    
    def test_update_adventure_scene_with_player(self):
        player = GameCommands.setup_new_account(self.user)
        
        adventure_scene1 = GameCommands.update_adventure_scene_with_player(player, "tresure", self.test_model2, 10, 2)
        adventure_scene2 = GameCommands.get_adventure_scene_by_player(player)
        self.assertEquals(adventure_scene1.key(), adventure_scene2.key())
        
    def test_get_adventure_scene_by_player(self):
        player = GameCommands.setup_new_account(self.user)

        # Initially there is no record for adventure_scene
        adventure_scene = GameCommands.get_adventure_scene_by_player(player)
        self.assertEquals(adventure_scene, None);

        # Test putting a record for first time
        adventure_scene = GameCommands.update_adventure_scene_with_player(player, "enemy", self.test_model1, 1, 5)

        self.assertNotEquals(adventure_scene, None)
        self.assertEquals(adventure_scene.player.key(), player.key())
        self.assertEquals(adventure_scene.scene_type, "enemy")
        self.assertEquals(adventure_scene.target.key(), self.test_model1)
        self.assertEquals(adventure_scene.floor, 1)
        self.assertEquals(adventure_scene.step, 5)

        # Test update the record
        adventure_scene = GameCommands.update_adventure_scene_with_player(player, "tresure", self.test_model2, 10, 2)

        self.assertNotEquals(adventure_scene, None)
        self.assertEquals(adventure_scene.player.key(), player.key())
        self.assertEquals(adventure_scene.scene_type, "tresure")
        self.assertEquals(adventure_scene.target.key(), self.test_model2)
        self.assertEquals(adventure_scene.floor, 10)
        self.assertEquals(adventure_scene.step, 2)

    def test_setup_new_account(self):
        player = GameCommands.setup_new_account(self.user)
        
        # Check whether all parameters are expected
        self.assertEquals(player.user_id, self.user.user_id())
        self.assertEquals(player.name, self.user.nickname())
        self.assertEquals(player.energy, const.INITIAL_PLAYER_PARAMS["energy"])
        self.assertEquals(player.money, const.INITIAL_PLAYER_PARAMS["money"])
        self.assertEquals(player.level, const.INITIAL_PLAYER_PARAMS["level"])
        self.assertEquals(player.total_xp, const.INITIAL_PLAYER_PARAMS["total_xp"])
        self.assertEquals(player.hp, const.INITIAL_PLAYER_PARAMS["hp"])
        self.assertEquals(player.max_hp, const.INITIAL_PLAYER_PARAMS["max_hp"])
        self.assertEquals(player.attack, const.INITIAL_PLAYER_PARAMS["attack"])
        self.assertEquals(player.defense, const.INITIAL_PLAYER_PARAMS["defense"])
        
        # Try again
        player = GameCommands.setup_new_account(self.user)
        self.assertEquals(player.user_id, self.user.user_id())

    def test_charge_energy(self):
        player = GameCommands.setup_new_account(self.user)
        
        player.energy = -1;
        GameCommands.charge_energy(player);
        self.assertEquals( player.energy,  player.max_energy)

    def test_get_enemy(self):
        self.fail()

    def test_proceed_combat(self):
        self.fail()
