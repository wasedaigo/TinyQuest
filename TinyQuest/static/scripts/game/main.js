
var mapdataCache = [];
var currentFloor = 1;
var currentStep = 0;
var playerInfo = {
    "lv" : 12,
    "exp" : 125,
    "nextExp" : 200
}

function loadState(state_id)
{
    templateHtml = gTemplates[state_id];
    if(state_id == "adventure")
    {
        getCurrentActiveScene(function(obj)
        {
            if (obj.success)
            {
              loadAdventure(obj);
              rpc.getCurrentState(setupMockScreen);
            }
        })
    }
}

function loadAdventure(obj)
{
    templates = {};
    $.template( "template", templateHtml );
    templateHtml = $.tmpl( "template", templates );
    $('#mainContainer div.content').html(templateHtml);
    if (gLoginInfo.admin)
    {
        $("#Admin").show();
    }
}

function updateProgressInfo()
{
    $("#progressTable").empty();
    var data = ""
    data += "<tr class='column'><td>Floor</td><td>Step</td><td>lv</td><td>Exp</td></tr>";
    data += "<tr>";
    data += "<td>" + currentFloor + "</td>";
    data += "<td>" + currentStep + "</td>";
    data += "<td>" + playerInfo.lv + "</td>";
    data += "<td>" + playerInfo.exp + " / " + playerInfo.nextExp  + "</td>";
    data += "</tr>";
    $("#progressTable").append(data); 
}

function updatePlayerInfo(name, hp, maxHP, atk, def, hit)
{
    $("#playerTable").empty();
    var data = ""
    data += "<tr class='column'><td>name</td><td>HP</td><td>ATK</td><td>DEF</td><td>HIT</td></tr>";
    data += "<tr>";
    data += "<td>" + name + "</td>";
    data += "<td>" + hp + " / " + maxHP + "</td>";
    data += "<td>" + atk + "</td>";
    data += "<td>" + def + "</td>";
    data += "<td>" + hit + "%</td>";
    data += "</tr>";
    $("#playerTable").append(data);    
}

function updateMap() {
    $("#mapBar").empty();
    
    var symbols = ["S", "G", "○", "M", "T", "R", "H"];
    var data = "";
    for (var i = 0; i < mapdataCache.length; i++) {
        
        if (i == currentStep) {
            data += "<div class='activeMarker'>";
            data += symbols[mapdataCache[i]];
            data += "</div>";
        } else {
            data += symbols[mapdataCache[i]];
        }
        data += " ";
    }
    $("#mapBar").append(data); 
}

function clearEnemyInfo()
{
    $("#enemyTable").empty();
}

function updateEnemyInfo(name, exp, hp, maxHP, atk, def, hit)
{
    $("#enemyTable").empty();
    var data = ""
    data += "<tr class='column'><td>name</td><td>HP</td><td>ATK</td><td>DEF</td><td>HIT</td><td>EXP</td></tr>";
    data += "<tr>";
    data += "<td>" + name + "</td>";
    data += "<td>" + hp + " / " + maxHP + "</td>";
    data += "<td>" + atk + "</td>";
    data += "<td>" + def + "</td>";
    data += "<td>" + hit + "%</td>";
    data += "<td>" + exp +  "</td>";
    data += "</tr>";
    $("#enemyTable").append(data);    
}

function addLog(categoryName, message) 
{
    var data = ""
    data += "<tr>";
    data += "<td class='column'>" + categoryName + "</td>";
    data += "<td>" + message + "</td>";
    data += "</tr>";
    $("#logTable").append(data);
    
    $("#logBox").animate({ scrollTop: $("#logTable").height() }, 0);
}

function addCurrentStepLog() {
    switch (mapdataCache[currentStep]) {
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
    itemData[no].equip = equip;
    updateItems();
}

function showItemDesc(no) {
    var itemDesc = itemData[no];
    $("#itemTable").empty();
    var data = "";
    data += "<button onclick='updateItems()'>←</button>";
    
    data += "<table>";
    data += "<tr>";
    data += "<td class='column'>Name</td>";
    data += "<td>" + itemDesc.name + "</td>";
    data += "</tr>";
    data += "<tr>";
    data += "<td class='column'>Desc</td>";
    data += "<td>" + itemDesc.desc + "</td>";
    data += "</tr>";
    data += "<tr>";
    data += "<td class='column'>Value</td>";
    data += "<td>" + itemDesc.value + "</td>";
    data += "</tr>";
    
    if (itemDesc.buffs) {
        for (var i = 0; i < itemDesc.buffs.length; i++) {
            var buff = itemDesc.buffs[i];
        
            data += "<tr>";
            data += "<td class='column'>" + buffData[buff.id].name + "</td>";
            data += "<td>" + visualizeSigned(buff.value) + "</td>";
            data += "</tr>";
        }
    }

    data += "</table>";

    if (itemDesc.equip) {
        data += "<button onclick='equip(" + no + ", false)'>unequip</button>";
    } else {
        data += "<button onclick='equip(" + no + ", true)'>equip</button>";
    }
    
    $("#itemTable").append(data);
}

function addItem(no, equipped, categoryName, itemName) 
{
    var data = ""
    data += "<tr>";
    if (equipped) {
        data += "<td>" + "☆" + "</td>";
    } else {
        data += "<td>" + "" + "</td>";
    }
    data += "<td class='column'>" + categoryName + "</td>";
    data += "<td class='title'>" + itemName + "</td>";
    data += "<td><button onclick='showItemDesc(" + no + ")'>→</button></td>";
    data += "</tr>";

    $("#itemTable").append(data);
}

function updateItems() 
{
    $("#itemTable").empty();
    for (var i = 0; i < itemData.length; i++) {
        var itemDesc = itemData[i];
        addItem(i, itemDesc.equip, itemDesc.category, itemDesc.name);
    }
}

function updateActionButtons() 
{
    $("#actionButtons").empty();
    
    var data = "";
    switch (mapdataCache[currentStep]) {
        case 0: // Nothing
            data += "<button onClick='onAction(\"goBack\");'>back</button>";
            data += "<button onClick='onAction(\"goForward\");'>go</button>";
        break;
        case 1: // Start
            data += "<button onClick='onAction(\"goHome\");'>goHome</button>";
            data += "<button onClick='onAction(\"goForward\");'>go</button>";
        break;
        case 2: // Goal
            data += "<button onClick='onAction(\"goBack\");'>back</button>";
            data += "<button onClick='onAction(\"stairDown\");'>go down</button>";
        break;
        case 3: // Monster
            data += "<button onClick='onAction(\"monsterEscape\");'>escape</button>";
            data += "<button onClick='onAction(\"monsterAttack\");'>attack</button>";
        break;
        case 4: // Treasure
            data += "<button onClick='onAction(\"goBack\");'>back</button>";
            data += "<button onClick='onAction(\"treasureOpen\");'>open</button>";
            data += "<button onClick='onAction(\"goForward\");'>go</button>";
        break;
        case 5: // Metal
            data += "<button onClick='onAction(\"goBack\");'>back</button>";
            data += "<button onClick='onAction(\"metalDig\");'>dig</button>";
            data += "<button onClick='onAction(\"goForward\");'>go</button>";
        break;
        case 6: // Herbs
            data += "<button onClick='onAction(\"goBack\");'>back</button>";
            data += "<button onClick='onAction(\"herbHarvest\");'>get</button>"
            data += "<button onClick='onAction(\"goForward\");'>go</button>";
        break;
    }
    
    $("#actionButtons").append(data);
}

function setupMockScreen(obj) 
{
    currentFloor = obj.floor;
    currentStep = obj.step;
    mapdataCache = obj.mapdata;
    updateActionButtons();
    updateMap();
    updateProgressInfo();
    updatePlayerInfo("Nakamura", 129, 200, 223, 123, 78);
    updateEnemyInfo("Green Dragon", 53, 129, 150, 223, 123, 78);
    addCurrentStepLog();
    updateItems(itemData);
}

function goForwardHandler(obj) {
    currentFloor = obj.floor;
    currentStep = obj.step;
    if (obj.mapType) {
        mapdataCache.push(obj.mapType);
    }
    addCurrentStepLog();
}

function goBackHandler(obj) {
    currentFloor = obj.floor;
    currentStep = obj.step;
    
    addCurrentStepLog();
}

function goDownHandler(obj) {
    currentFloor = obj.floor;
    currentStep = obj.step;
    mapdataCache = [obj.mapType];
    
    addCurrentStepLog();
}

function battleAttackHandler(obj) {
    addLog("Monster", obj.self.damage + " damage to you!");
    addLog("Monster", obj.enemy.damage + " damage to the monster!");
    if (obj.enemy.dead) {
        addLog("Monster", "Killed the monster");
        addLog("Monster", "Gained " + obj.enemy.exp + " EXP");
        mapdataCache[currentStep] = obj.mapType;
    }
}

function battleEscapeHandler(obj) {
    addLog("Monster", "Escaped from the monster");
    currentFloor = obj.floor;
    currentStep = obj.step;
}

function treasureGetHandler(obj) {
    mapdataCache[currentStep] = obj.mapType;
    itemData.push(obj.data);
    addItem(itemData.length - 1, false, obj.data.category, obj.data.name);
    addLog("Treasure", "Got [" + obj.data.name + "]");
}

function metalGetHandler(obj) {
    mapdataCache[currentStep] = obj.mapType;
    itemData.push(obj.data);
    addItem(itemData.length - 1, false, obj.data.category, obj.data.name);
    addLog("Metal", "Got [" + obj.data.name + "]");
}

function herbGetHandler(obj) {
    mapdataCache[currentStep] = obj.mapType;
    itemData.push(obj.data);
    addItem(itemData.length - 1, false, obj.data.category, obj.data.name);
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
    
    updateProgressInfo();
    updateActionButtons();
    updateMap();
}
setLoadFlag("main");