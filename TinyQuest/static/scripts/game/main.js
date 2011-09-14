var cache = null;

function addCurrentStepLog() {
    switch (cache.mapdata[cache.step]) {
        case Const.MapType.Empty: // Nothing
            addLog("Nothing", "");
        break;
        case Const.MapType.Start: // Start
            addLog("Start", "The start point");
        break;
        case Const.MapType.Goal: // Goal
            addLog("Stair", "Found a stair to go down");
        break;
        case Const.MapType.Enemy: // Enemy
            addLog("Enemy", "Encountered a enemy");
        break;
        case Const.MapType.Treasure: // Treasure
            addLog("Treasure", "Found a treasure chest");
        break;
        case Const.MapType.Metal: // Metal
            addLog("Metal", "Found Metal");
        break;
        case Const.MapType.Herb: // Herbs
            addLog("Herbs", "Found herbs");
        break;
    }
}

function visualizeSigned(value) 
{
    if (value >= 0) {
        value = "+" + value;
    }
    
    return value;
}

function equip(no, equip) {
    cache.itemdata[no].equip = equip;
    updateItems();
}


function getCurrentMapType() {
    return cache.mapdata[cache.step];
}

function setupMockScreen(obj) 
{
    cache = obj;
    updateActionButtons(getCurrentMapType());
    updateMap(cache.mapdata, cache.step);
    updateProgressInfo(cache.floor, cache.step, cache.playerInfo.lv, cache.playerInfo.exp, cache.playerInfo.nextExp);
    updatePlayerInfo("Nakamura", 129, 200, 223, 123, 78);
    clearEnemyInfo();
    addCurrentStepLog();
    updateItems(cache.itemdata);
}

function goForwardHandler(obj) {
    cache.floor = obj.floor;
    cache.step = obj.step;
    if (obj.mapType !== undefined) {
        cache.mapdata.push(obj.mapType);
    }
    if (obj.enemy !== undefined) {
        updateEnemyInfo(obj.enemy.name, obj.enemy.exp, obj.enemy.hp, obj.enemy.max_hp, obj.enemy.attack, obj.enemy.defense, obj.enemy.hit);
    }
    addCurrentStepLog();
}

function goBackHandler(obj) {
    cache.floor = obj.floor;
    cache.step = obj.step;
    
    addCurrentStepLog();
}

function goDownHandler(obj) {
    cache.floor = obj.floor;
    cache.step = obj.step;
    cache.mapdata = [obj.mapType];
    
    addCurrentStepLog();
}

function battleAttackHandler(obj) {
    addLog("Enemy", obj.self.damage + " damage to you!");
    addLog("Enemy", obj.enemy.damage + " damage to the enemy!");
    if (obj.enemy.dead) {
        addLog("Enemy", "Killed the enemy");
        addLog("Enemy", "Gained " + obj.enemy.exp + " EXP");
        cache.mapdata[cache.step] = obj.mapType;
        clearEnemyInfo();
    }
}

function battleEscapeHandler(obj) {
    addLog("Enemy", "Escaped from the enemy");
    clearEnemyInfo();
    cache.floor = obj.floor;
    cache.step = obj.step;
}

function treasureGetHandler(obj) {
    cache.mapdata[cache.step] = obj.mapType;
    cache.itemdata.push(obj.data);
    addItem(cache.itemdata.length - 1, false, obj.data.category, obj.data.name);
    addLog("Treasure", "Got [" + obj.data.name + "]");
}

function metalGetHandler(obj) {
    cache.mapdata[cache.step] = obj.mapType;
    cache.itemdata.push(obj.data);
    addItem(cache.itemdata.length - 1, false, obj.data.category, obj.data.name);
    addLog("Metal", "Got [" + obj.data.name + "]");
}

function herbGetHandler(obj) {
    cache.mapdata[cache.step] = obj.mapType;
    cache.itemdata.push(obj.data);
    addItem(cache.itemdata.length - 1, false, obj.data.category, obj.data.name);
    addLog("Herbs", "Got [" + obj.data.name + "]");
}

function onAction(type) {
    switch (type) {
        case "goHome":
            alert("not implemented");
        break;
        case "goBack":
            Rpc.goBack(goBackHandler);
        break;
        case "goForward":
            Rpc.goForward(goForwardHandler);
        break;
        case "stairDown":
            addLog("Stair", "Going down");
            Rpc.goDown(goDownHandler);
        break;
        case "monsterAttack":
            Rpc.battleAttack(battleAttackHandler);
        break;
        case "monsterEscape":
            Rpc.battleEscape(battleEscapeHandler);
        break;
        case "treasureOpen":
            Rpc.openTreasureChest(treasureGetHandler);
        break;
        case "metalDig":
            Rpc.digMetal(metalGetHandler);
        break;
        case "herbHarvest":
            Rpc.harvestHerb(herbGetHandler);
        break;
    }
    
    updateProgressInfo(cache.floor, cache.step, cache.playerInfo.lv, cache.playerInfo.exp, cache.playerInfo.nextExp);
    updateActionButtons(getCurrentMapType());
    updateMap(cache.mapdata, cache.step);
}
setLoadFlag("main");