class SerializeCommands:
    @classmethod
    def serialize_character(cls, character):
      return {
        "id":character.key().id(), 
        "name":character.name,
        "energy":character.energy
      }
      
    @classmethod
    def serialize_characters(cls, characters):
      arr = []
      for character in characters:
        arr.append(serialize_character(character))
      return arr
    
    @classmethod
    def serialize_player(cls, player):
      return {
        "money":player.money
      }
    
    @classmethod
    def serialize_enemy(cls, enemy):
      return {
        "tag":enemy.tag,
        "life":enemy.life,
        "attack":enemy.attack,
        "defense":enemy.defense
      }
