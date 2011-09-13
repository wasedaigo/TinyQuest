var cache = null;

function addCurrentStepLog() {
    switch (cache.mapdata[cache.step]) {
        case 0: // Nothing
            addLog("Nothing", "");
        break;
        case 1: // Start
            addLog("Start", "The start point");
        break;
        case 2: // Goal
            addLog("Stair", "Found a stair to go down");
        break;
        case 3: // Monster
            addLog("Monster", "Encountered a monster");
        break;
        case 4: // Treasure
            addLog("Treasure", "Found a treasure chest");
        break;
        case 5: // Metal
            addLog("Metal", "Found Metal");
        break;
        case 6: // Herbs
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
    updateEnemyInfo("Green Dragon", 53, 129, 150, 223, 123, 78);
    addCurrentStepLog();
    updateItems(cache.itemdata);
}

function goForwardHandler(obj) {
    cache.floor = obj.floor;
    cache.step = obj.step;
    if (obj.mapType !== undefined) {
        cache.mapdata.push(obj.mapType);
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
    addLog("Monster", obj.self.damage + " damage to you!");
    addLog("Monster", obj.enemy.damage + " damage to the monster!");
    if (obj.enemy.dead) {
        addLog("Monster", "Killed the monster");
        addLog("Monster", "Gained " + obj.enemy.exp + " EXP");
        cache.mapdata[cache.step] = obj.mapType;
    }
}

function battleEscapeHandler(obj) {
    addLog("Monster", "Escaped from the monster");
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
            rpc.goBack(goBackHandler);
        break;
        case "goForward":
            rpc.goForward(goForwardHandler);
        break;
        case "stairDown":
            addLog("Stair", "Going down");
            rpc.goDown(goDownHandler);
        break;
        case "monsterAttack":
            rpc.battleAttack(battleAttackHandler);
        break;
        case "monsterEscape":
            rpc.battleEscape(battleEscapeHandler);
        break;
        case "treasureOpen":
            rpc.openTreasureChest(treasureGetHandler);
        break;
        case "metalDig":
            rpc.digMetal(metalGetHandler);
        break;
        case "herbHarvest":
            rpc.harvestHerb(herbGetHandler);
        break;
    }
    
    updateProgressInfo(cache.floor, cache.step, cache.playerInfo.lv, cache.playerInfo.exp, cache.playerInfo.nextExp);
    updateActionButtons(getCurrentMapType());
    updateMap(cache.mapdata, cache.step);
}
setLoadFlag("main");