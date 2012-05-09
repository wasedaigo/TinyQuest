from google.appengine.api import users
from google.appengine.ext import db

#----------------------------------
#    Master Data
#----------------------------------

class LocalizedStringModel(db.Model):
    ja = db.StringProperty()
    de = db.StringProperty()
    en = db.StringProperty()

class ItemAttributeModel(db.Model):
    name = db.ReferenceProperty(LocalizedStringModel, collection_name="item_attribute_name")
    description = db.ReferenceProperty(LocalizedStringModel, collection_name="item_attribute_description")
    type = db.CategoryProperty()

class ItemModel(db.Model):
    name = db.ReferenceProperty(LocalizedStringModel, collection_name="item_name")
    description = db.ReferenceProperty(LocalizedStringModel, collection_name="item_description")
    attributes = db.ListProperty(db.Key)
    rarity = db.CategoryProperty()
    type = db.CategoryProperty()

class EnemyModel(db.Model):
    name = db.ReferenceProperty(LocalizedStringModel)
    tag = db.CategoryProperty()
    attack = db.IntegerProperty()
    defense = db.IntegerProperty()
    life = db.IntegerProperty()
    max_hp = db.IntegerProperty()

#----------------------------------
#    Dynamic Data
#----------------------------------

class PlayerItemModel(db.Model):
    item = db.ReferenceProperty(ItemModel)
    count = db.IntegerProperty()

class ActiveBuffModel(db.Model):
    type = db.CategoryProperty()
    life_time = db.IntegerProperty()

class PlayerModel(db.Model):
    user_id = db.StringProperty()
    name = db.StringProperty()
    buffs = db.ListProperty(db.Key)
    items = db.ListProperty(db.Key)
    equip_helm = db.ReferenceProperty(PlayerItemModel, collection_name="player_equip_helm")
    equip_armor = db.ReferenceProperty(PlayerItemModel, collection_name="player_equip_armor")
    equip_weapon = db.ReferenceProperty(PlayerItemModel, collection_name="player_equip_weapon")
    equip_shield = db.ReferenceProperty(PlayerItemModel, collection_name="player_equip_shield")
    equip_ring1 = db.ReferenceProperty(PlayerItemModel, collection_name="player_equip_ring1")
    equip_ring2 = db.ReferenceProperty(PlayerItemModel, collection_name="player_equip_ring2")
    equip_ring3 = db.ReferenceProperty(PlayerItemModel, collection_name="player_equip_ring3")
    equip_ring4 = db.ReferenceProperty(PlayerItemModel, collection_name="player_equip_ring4")
    attack = db.IntegerProperty()
    defense = db.IntegerProperty()
    energy = db.IntegerProperty()
    max_energy = db.IntegerProperty()
    total_xp = db.IntegerProperty()
    life = db.IntegerProperty()
    max_life = db.IntegerProperty()
    exp = db.IntegerProperty()
    level = db.IntegerProperty()
    money = db.IntegerProperty()
    jewels = db.IntegerProperty()

class ActiveEnemyModel(db.Model):
    buffs = db.ListProperty(db.Key)
    attack = db.IntegerProperty()
    defense = db.IntegerProperty()
    life = db.IntegerProperty()
    exp = db.IntegerProperty()
    
class ActiveSceneModel(db.Model):
    player = db.ReferenceProperty(PlayerModel)
    scene_type = db.CategoryProperty()
    target = db.ReferenceProperty(collection_name="active_scene_target")
    floor = db.IntegerProperty()
    step = db.IntegerProperty()