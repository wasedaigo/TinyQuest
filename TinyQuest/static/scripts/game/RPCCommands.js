// Static Data
var gCurrentFloor = 0;
var gCurrentStep = 0;
var gMapdata = [0];
var gExploredMapStep = 0;
var gItemData = [
    {"category":"Helm","name":"Helm + 2", "desc":"Good helm", "value":100, "buffs" : [], "equip":true, "equippable":true},
    {"category":"Armor","name":"Blessed Fire ChainMail", "desc":"Mail", "value":40, "buffs" : [{"id":1001, "value":5}, {"id":1002, "value":10}], "equip":true, "equippable":true}
];
var gBuffData = {
    1001 : {"name":"Fire Immunity", "valueType":"value"},
    1002 : {"id":1002, "name":"HP UP", "valueType":"proportion"}
};

var gItemMaster = {
    "herb":[
            {"category":"Herb","name":"Green Herb", "desc":"Standard herb, green tea smells good", "value":100},
            {"category":"Herb","name":"Blue Herb", "desc":"Blue herb, which has antipoison effect", "value":0},
            {"category":"Herb","name":"Red Herb", "desc":"Rare herb, smells like roses", "value":0}
     ],
    "metal":[
            {"category":"Metal","name":"Steel", "desc":"Good for making short sword", "value":100},
            {"category":"Metal","name":"Copper", "desc":"Copper is not that expensive", "value":0},
            {"category":"Metal","name":"Gold", "desc":"Gold, you must be happy to get this", "value":0}
     ],
    "treasure":[
            {"category":"Helm","name":"Iron Helm", "desc":"not that cool", "value":10},
            {"category":"Helm","name":"Metal Helm", "desc":"soso good", "value":30},
            {"category":"Helm","name":"Great Helm", "desc":"Very cool!", "value":60},
            {"category":"Sword","name":"Short Sword", "desc":"A sword which is short", "value":10},
            {"category":"Sword","name":"Long Sword", "desc":"A sword which is long", "value":30},
            {"category":"Sword","name":"Great Sowrd", "desc":"A sword which is great", "value":60}
     ]
};

var gEnemyMaster = [
    {"name":"Green Dragon", "exp":500, "hp":1000, "max_hp":1000, "attack":120, "defense":100, "hit":68},
    {"name":"Goblin", "exp":80, "hp":200, "max_hp":200, "attack":30, "defense":100, "hit":68},
    {"name":"Naga", "exp":120, "hp":700, "max_hp":700, "attack":90, "defense":100, "hit":68},
    {"name":"Cyclops", "exp":60, "hp":800, "max_hp":800, "attack":80, "defense":100, "hit":68}
];

var gEnemies = {};

var gPlayer = {
    "lv" : 1,
    "exp" : 0,
    "next_exp" : 200,
    "name" : "Nakamura", 
    "hp" : 2000, 
    "max_hp" : 2000, 
    "attack" : 223, 
    "defense" : 123, 
    "hit" : 78
}

function generatemapdata() 
{
    var size = 10 + Math.floor(Math.random() * 11);
    gMapdata = new Array(size);
    gMapdata[0] = 1;
    gMapdata[size - 1] = 2;
    var randomSeeds = [0, 0, 0, 0, 0, 3, 4, 5, 6];
    for (var i = 1; i < size - 1; i++) {
        gMapdata[i] = randomSeeds[Math.floor(Math.random() * randomSeeds.length)];
    }
}

var Rpc = {
    getCurrentState : function(cb) {
        generatemapdata();
        var obj = {};
        obj.floor = gCurrentFloor;
        obj.step = gCurrentStep;
        obj.mapdata = [gMapdata[0]];
        obj.player = gPlayer;
        obj.itemdata = gItemData;
        obj.buffdata = gBuffData;
        cb.call(this, obj);
    },
    battleAttack : function(cb) {
        var enemy = gEnemies[gCurrentStep];
        var obj = {};
        obj.success = false;
        if (enemy) {
            obj.success = true;
            obj.mapType = gMapdata[gCurrentStep];
            
            enemy.hp -= gPlayer.attack;
            gPlayer.hp -= enemy.attack;
       
            obj.enemy = {damage : gPlayer.attack, hp : enemy.hp};
            obj.player = {damage : enemy.attack, hp : gPlayer.hp};
    
            if (gPlayer.hp <= 0) {
                // Initialize all params
                // gCurrentFloor = 0;
                // gCurrentStep = 0;
                // gExploredMapStep = 0;
                // generatemapdata();
                // gEnemies = {};
            } else {
                if (enemy.hp <= 0) {
                    gEnemies[gCurrentStep] = undefined;
                    gMapdata[gCurrentStep] = 0;
                    obj.mapType = 0;
                    gPlayer.exp += enemy.exp;
                    
                    // Level UP!
                    while (gPlayer.exp >= gPlayer.next_exp) {
                        gPlayer.lv += 1;
                        gPlayer.exp -= gPlayer.next_exp;
                        obj.player.lv = gPlayer.lv;
                        // TODO add levelup event to the queue
                    }

                    obj.player.exp = gPlayer.exp;
                    obj.enemy.exp = enemy.exp;
                }
            }
        }
        cb.call(this, obj);
    },
    battleEscape : function(cb) {
        gCurrentStep -= 1;
        var obj = {"floor":gCurrentFloor, "step":gCurrentStep}
        cb.call(this, obj);
    },
    openTreasureChest : function(cb) {
        var obj = {}
        var data = gItemMaster.treasure;
        var index = Math.floor(Math.random() * data.length);
        obj.data = data[index];
        gMapdata[gCurrentStep] = 0;
        obj.mapType = gMapdata[gCurrentStep];
        cb.call(this, obj);
    },
    digMetal : function(cb) {
        var obj = {}
        var data = gItemMaster.metal;
        var index = Math.floor(Math.random() * data.length);
        obj.data = data[index];
        gMapdata[gCurrentStep] = 0;
        obj.mapType = gMapdata[gCurrentStep];
        cb.call(this, obj);
    },
    harvestHerb : function(cb) {
        var obj = {}
        var data = gItemMaster.herb;
        var index = Math.floor(Math.random() * data.length);
        obj.data = data[index];
        gMapdata[gCurrentStep] = 0;
        obj.mapType = gMapdata[gCurrentStep];
        cb.call(this, obj);
    },
    goForward : function(cb) {
        gCurrentStep += 1;
        var obj = {"floor":gCurrentFloor, "step":gCurrentStep};
        if (gCurrentStep > gExploredMapStep) {
            obj.mapType = gMapdata[gCurrentStep];
            gExploredMapStep += 1;
        }
        
        if (gMapdata[gCurrentStep] == Const.MapType.Enemy) {
            if (gEnemies[gCurrentStep] === undefined) {
                var index = Math.floor(Math.random() * gEnemyMaster.length);
                gEnemies[gCurrentStep] = Utils.clone(gEnemyMaster[index]);
            }
            obj.enemy = gEnemies[gCurrentStep];
        }
        
        cb.call(this, obj);
    },
    goBack : function(cb) {
        gCurrentStep -= 1;
        var obj = {"floor":gCurrentFloor, "step":gCurrentStep}
        cb.call(this, obj);
    },
    goDown : function(cb) {
        gCurrentFloor += 1;
        gCurrentStep = 0;
        gExploredMapStep = 0;
        generatemapdata();
        gEnemies = {};
        var obj = {"floor":gCurrentFloor, "step":gCurrentStep, "mapType" : 1};
        cb.call(this, obj);
    }
};

function go()
{
  loadJSON("api/Go", function(json)
  {
    if (json["success"])
    {
      loadState("adventure");
    }
  })
}

function charge_energy()
{
  logEnemiesadJSON("api/ChargeEnergy", function(json)
  {
    if (json["success"])
    {
      loadState("adventure");
    }
  })
}

function getCurrentActiveScene(cb)
{
  Utils.loadJSON("api/GetCurrentActiveScene", cb);
}