"""Set of game commands"""
import sys
sys.path.append('./models')
from db_model import *
from google.appengine.api import memcache
import const
from enemy_factory import *

class GameCommands:

    @classmethod
    def get_adventure_scene_by_player(cls, player):
        """Return adventure scene where player is currently on"""
        adventure_scenes = AdventureSceneModel.all().filter('player = ', player).fetch(1)
        if adventure_scenes:
            return adventure_scenes[0]
        else:
            return None

    @classmethod
    def update_adventure_scene_with_player(cls, player, scene_type, target, floor, step):
        """update current player scene"""
        adventure_scene = cls.get_adventure_scene_by_player(player)
        if adventure_scene:
            adventure_scene.scene_type = scene_type;
            adventure_scene.target = target;
            adventure_scene.floor = floor;
            adventure_scene.step = step;
            adventure_scene.put()
        else:
            adventure_scene = AdventureSceneModel(
                player = player,
                scene_type = scene_type,
                target = target,
                floor = floor,
                step = step
            )
            adventure_scene.put()
            
        return adventure_scene

    @classmethod
    def get_player(cls, user):
        """Return player currently connecting"""
        players = PlayerModel.all().filter('user_id = ', user.user_id()).fetch(1)
        
        if players:
            return players[0]
        else:
            return None

    @classmethod
    def setup_new_account(cls, user):
        """Set up a new account, if specified user does not exist"""
        player = cls.get_player(user)
        
        if player == None:
            # Create a new player
            player = PlayerModel(
                user_id = user.user_id(), 
                name = user.nickname(),
                energy = const.INITIAL_PLAYER_PARAMS["energy"], 
                max_energy = const.INITIAL_PLAYER_PARAMS["energy"], 
                money = const.INITIAL_PLAYER_PARAMS["money"], 
                level = const.INITIAL_PLAYER_PARAMS["level"],
                total_xp = const.INITIAL_PLAYER_PARAMS["total_xp"],
                hp = const.INITIAL_PLAYER_PARAMS["hp"],
                max_hp = const.INITIAL_PLAYER_PARAMS["max_hp"],
                attack = const.INITIAL_PLAYER_PARAMS["attack"],
                defense = const.INITIAL_PLAYER_PARAMS["defense"]
            )
            
            player.put()
        
        return player

    @classmethod
    def set_enemy(cls, player):
        """Set an for the player to encounter"""
        pass

    @classmethod
    def get_enemy(cls, player):
        """Get the enemy the player is fighting right now."""
        pass

    @classmethod
    def start_adventure(cls):
        """Start the whole adventure"""
        player = get_player()
        
        return result

    @classmethod
    def proceed_step(cls, player, step):
        """Proceed the player to next scene"""
        enemy = EnemyFactory.build_enemy("dragon", 13, 120, 234, 134)
        cls.update_adventure_scene_with_player(player, "enemy", enemy, 1, step + 1)

    @classmethod
    def proceed_combat(cls, enemy, player):
        """Proceed current combat"""
        enemy.hp -= player.attack

    @classmethod
    def simulate_combat(cls, player, enemy):
        """Simulate combat and return result"""
        damage = player.attack
        enemy.hp -= player.attack
        result = {
        "player_hp":player.hp,
        "enemy_hp":enemy.hp,
        "damage":damage
        }
        
        return result
