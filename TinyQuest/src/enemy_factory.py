"""Factory class for enemies"""
import sys
sys.path.append('./models')
from db_model import *

class EnemyFactory:

    @classmethod
    def build_enemy(cls, tag, lv, hp, attack, defense):
        """Build an enemy and return it"""
        
        enemy = EnemyModel(
            tag = tag,
            lv = lv,
            hp = hp,
            max_hp = hp,
            attack = attack,
            defense = defense
        )
        enemy.put()
    
        return enemy