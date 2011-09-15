var cache = null;
var Main = {
    loadState : function(state_id)
    {
        templateHtml = gTemplates[state_id];
        if(state_id == "adventure")
        {
          MainView.loadAdventure();
          Rpc.getCurrentState(Main.setupMockScreen);
        }
    },
    
    addCurrentStepLog : function() {
        switch (cache.mapdata[cache.step]) {
            case Const.MapType.Empty: // Nothing
                MainView.addLog("Nothing", "");
            break;
            case Const.MapType.Start: // Start
                MainView.addLog("Start", "The start point");
            break;
            case Const.MapType.Goal: // Goal
                MainView.addLog("Stair", "Found a stair to go down");
            break;
            case Const.MapType.Enemy: // Enemy
                MainView.addLog("Enemy", "Encountered " + cache.enemy.name);
            break;
            case Const.MapType.Treasure: // Treasure
                MainView.addLog("Treasure", "Found a treasure chest");
            break;
            case Const.MapType.Metal: // Metal
                MainView.addLog("Metal", "Found Metal");
            break;
            case Const.MapType.Herb: // Herbs
                MainView.addLog("Herbs", "Found herbs");
            break;
        }
    },
    
    equip : function(no, equip) {
        cache.itemdata[no].equip = equip;
        MainView.updateItems();
    },

    getCurrentMapType : function() {
        return cache.mapdata[cache.step];
    },
    
    updateEnemyState : function() {
        MainView.updateEnemyInfo(cache.enemy.name, cache.enemy.exp, cache.enemy.hp, cache.enemy.max_hp, cache.enemy.attack, cache.enemy.defense, cache.enemy.hit);
    },
    
    updatePlayerState : function() {
        MainView.updatePlayerInfo(cache.player.name, cache.player.hp, cache.player.max_hp, cache.player.attack, cache.player.defense, cache.player.hit);
    },
    
    setupMockScreen : function(obj) 
    {
        cache = obj;
        MainView.updateActionButtons(Main.getCurrentMapType());
        MainView.updateMap(cache.mapdata, cache.step);
        MainView.updateProgressInfo(cache.floor, cache.step, cache.player.lv, cache.player.exp, cache.player.next_exp);
        Main.updatePlayerState();
        MainView.clearEnemyInfo();
        Main.addCurrentStepLog();
        MainView.updateItems(cache.itemdata);
    },
    
    goForwardHandler : function(obj) {
        cache.floor = obj.floor;
        cache.step = obj.step;
        if (obj.mapType !== undefined) {
            cache.mapdata.push(obj.mapType);
        }
        if (obj.enemy !== undefined) {
            cache.enemy = obj.enemy;
            Main.updateEnemyState();
        }
        Main.addCurrentStepLog();
    },
    
    goBackHandler : function(obj) {
        cache.floor = obj.floor;
        cache.step = obj.step;
        Main.addCurrentStepLog();
    },
    
    goDownHandler : function(obj) {
        cache.floor = obj.floor;
        cache.step = obj.step;
        cache.mapdata = [obj.mapType];
        
        Main.addCurrentStepLog();
    },
    
    battleAttackHandler : function(obj) {
        MainView.addLog("Battle", obj.player.damage + " damage to you!");
        MainView.addLog("Battle", obj.enemy.damage + " damage to the enemy!");
    
        cache.enemy.hp = obj.enemy.hp;
        cache.player.hp = obj.player.hp;
        cache.player.exp = obj.player.exp ? obj.player.exp : cache.player.exp;
        cache.player.next_exp = obj.player.next_exp ? obj.player.next_exp : cache.player.next_exp;
        cache.player.lv = obj.player.lv ? obj.player.lv : cache.player.lv;
        Main.updateEnemyState();
        Main.updatePlayerState();
    
        if (obj.player.hp <= 0) {
            MainView.addLog("System", "GameOver");
        } else {
            if (obj.enemy.hp <= 0) {
                MainView.addLog("Battle", "Killed the enemy");
                MainView.addLog("Battle", "Gained " + obj.enemy.exp + " EXP");
                if (obj.player.lv) {
                    MainView.addLog("System", "Level up to " + obj.player.lv + " !!");
                }
                cache.mapdata[cache.step] = obj.mapType;
                MainView.clearEnemyInfo();
            }
        }
    },
    
    battleEscapeHandler : function(obj) {
        MainView.addLog("Enemy", "Escaped from the enemy");
        cache.enemy = undefined;
        MainView.clearEnemyInfo();
        cache.floor = obj.floor;
        cache.step = obj.step;
    },
    
    treasureGetHandler : function(obj) {
        cache.mapdata[cache.step] = obj.mapType;
        cache.itemdata.push(obj.data);
        MainView.addItem(cache.itemdata.length - 1, false, obj.data.category, obj.data.name);
        MainView.addLog("Treasure", "Got [" + obj.data.name + "]");
    },
    
    metalGetHandler : function(obj) {
        cache.mapdata[cache.step] = obj.mapType;
        cache.itemdata.push(obj.data);
        MainView.addItem(cache.itemdata.length - 1, false, obj.data.category, obj.data.name);
        MainView.addLog("Metal", "Got [" + obj.data.name + "]");
    },
    
    herbGetHandler : function(obj) {
        cache.mapdata[cache.step] = obj.mapType;
        cache.itemdata.push(obj.data);
        MainView.addItem(cache.itemdata.length - 1, false, obj.data.category, obj.data.name);
        MainView.addLog("Herbs", "Got [" + obj.data.name + "]");
    },
    
    onAction : function(type) {
        switch (type) {
            case "goHome":
                alert("not implemented");
            break;
            case "goBack":
                Rpc.goBack(Main.goBackHandler);
            break;
            case "goForward":
                Rpc.goForward(Main.goForwardHandler);
            break;
            case "stairDown":
                MainView.addLog("Stair", "Going down");
                Rpc.goDown(Main.goDownHandler);
            break;
            case "monsterAttack":
                Rpc.battleAttack(Main.battleAttackHandler);
            break;
            case "monsterEscape":
                Rpc.battleEscape(Main.battleEscapeHandler);
            break;
            case "treasureOpen":
                Rpc.openTreasureChest(Main.treasureGetHandler);
            break;
            case "metalDig":
                Rpc.digMetal(Main.metalGetHandler);
            break;
            case "herbHarvest":
                Rpc.harvestHerb(Main.herbGetHandler);
            break;
        }
        
        MainView.updateProgressInfo(cache.floor, cache.step, cache.player.lv, cache.player.exp, cache.player.next_exp);
        MainView.updateActionButtons(Main.getCurrentMapType());
        MainView.updateMap(cache.mapdata, cache.step);
    }
}
setLoadFlag("main");