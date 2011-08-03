from google.appengine.api import users
from google.appengine.ext import db

class LocalizedStringModel(db.Model):
    ja = db.StringProperty()
    de = db.StringProperty()
    en = db.StringProperty()

class ItemModel(db.Model):
    name = db.ReferenceProperty(LocalizedStringModel)
    type = db.CategoryProperty()

class EnemyModel(db.Model):
    tag = db.CategoryProperty()
    lv = db.IntegerProperty()
    hp = db.IntegerProperty()
    max_hp = db.IntegerProperty()
    attack = db.IntegerProperty()
    defense = db.IntegerProperty()

class PlayerModel(db.Model):
    user_id = db.StringProperty()
    name = db.StringProperty()
    energy = db.IntegerProperty()
    max_energy = db.IntegerProperty()
    money = db.IntegerProperty()
    level = db.IntegerProperty()
    total_xp = db.IntegerProperty()
    hp = db.IntegerProperty()
    max_hp = db.IntegerProperty()
    attack = db.IntegerProperty()
    defense = db.IntegerProperty()

class AdventureSceneModel(db.Model):
    player = db.ReferenceProperty(PlayerModel)
    scene_type = db.CategoryProperty()
    target = db.ReferenceProperty(collection_name="adventure_scene_target_set")
    floor = db.IntegerProperty()
    step = db.IntegerProperty()