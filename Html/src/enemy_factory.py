"""Factory class for enemies"""
import sys
sys.path.append('./models')
from db_model import *

class EnemyFactory:

    @classmethod
    def build_enemy(cls, tag, life, attack, defense):
        """Build an enemy and return it"""
        
        enemy = EnemyModel(
            tag = tag,
            life = life,
            max_life = life,
            attack = attack,
            defense = defense
        )
        enemy.put()
    
        return enemy