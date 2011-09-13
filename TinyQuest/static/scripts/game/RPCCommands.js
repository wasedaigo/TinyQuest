// Static Data
var mapdata = [0];
var exploredMapStep = 0;
var itemData = [
    {"category":"Helm","name":"Helm + 2", "desc":"Good helm", "value":100, "buffs" : [], "equip":true, "equippable":true},
    {"category":"Armor","name":"Blessed Fire ChainMail", "desc":"Mail", "value":40, "buffs" : [{"id":1001, "value":5}, {"id":1002, "value":10}], "equip":true, "equippable":true}
];
var buffData = {
    1001 : {"name":"Fire Immunity", "valueType":"value"},
    1002 : {"id":1002, "name":"HP UP", "valueType":"proportion"}
};

var itemMaster = {
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

function generatemapdata() 
{
    var size = 10 + Math.floor(Math.random() * 11);
    mapdata = new Array(size);
    mapdata[0] = 0;
    mapdata[size - 1] = 1;
    var randomSeeds = [2, 2, 2, 2, 2, 3, 4, 5, 6];
    for (var i = 1; i < size - 1; i++) {
        mapdata[i] = randomSeeds[Math.floor(Math.random() * randomSeeds.length)];
    }
}

var rpc = {
    getCurrentState : function(cb) {
        generatemapdata();
        var obj = {"floor":currentFloor, "step":currentStep, "mapdata" : [mapdata[0]]};
        cb.call(this, obj);
    },
    battleAttack : function(cb) {
        mapdata[currentStep] = 2;
        var obj = {};
        obj.enemy = {damage : 32, exp : 128, dead : true};
        obj.self = {damage : 15, dead : false};
        obj.mapType = mapdata[currentStep];
        cb.call(this, obj);
    },
    battleEscape : function(cb) {
        var obj = {"floor":currentFloor, "step":currentStep - 1}
        cb.call(this, obj);
    },
    openTreasureChest : function(cb) {
        var obj = {}
        var data = itemMaster.treasure;
        var index = Math.floor(Math.random() * data.length);
        obj.data = data[index];
        mapdata[currentStep] = 2;
        obj.mapType = mapdata[currentStep];
        cb.call(this, obj);
    },
    digMetal : function(cb) {
        var obj = {}
        var data = itemMaster.metal;
        var index = Math.floor(Math.random() * data.length);
        obj.data = data[index];
        mapdata[currentStep] = 2;
        obj.mapType = mapdata[currentStep];
        cb.call(this, obj);
    },
    harvestHerb : function(cb) {
        var obj = {}
        var data = itemMaster.herb;
        var index = Math.floor(Math.random() * data.length);
        obj.data = data[index];
        mapdata[currentStep] = 2;
        obj.mapType = mapdata[currentStep];
        cb.call(this, obj);
    },
    goForward : function(cb) {
        var obj = {"floor":currentFloor, "step":currentStep + 1};
        if (currentStep >= exploredMapStep) {
            obj.mapType = mapdata[currentStep + 1];
            exploredMapStep += 1;
        }
        
        cb.call(this, obj);
    },
    goBack : function(cb) {
        var obj = {"floor":currentFloor, "step":currentStep - 1}
        cb.call(this, obj);
    },
    goDown : function(cb) {
        var obj = {"floor":currentFloor + 1, "step":0, "mapType" : 0}
        generatemapdata();
        exploredMapStep = 0;
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
  loadJSON("api/ChargeEnergy", function(json)
  {
    if (json["success"])
    {
      loadState("adventure");
    }
  })
}

function getCurrentActiveScene(cb)
{
  loadJSON("api/GetCurrentActiveScene", cb);
}