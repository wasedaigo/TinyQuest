var MainView = {    
    loadAdventure : function()
    {
        templates = {};
        $.template( "template", templateHtml );
        templateHtml = $.tmpl( "template", templates );
        $('#mainContainer div.content').html(templateHtml);
        if (gLoginInfo.admin)
        {
            $("#Admin").show();
        }
    },

    updateProgressInfo : function(floor, step, lv, exp, nextExp)
    {
        $("#progressTable").empty();
        var data = ""
        data += "<tr class='column'><td>Floor</td><td>Step</td><td>lv</td><td>Exp</td></tr>";
        data += "<tr>";
        data += "<td>" + floor + "</td>";
        data += "<td>" + step + "</td>";
        data += "<td>" + lv + "</td>";
        data += "<td>" + exp + " / " + nextExp  + "</td>";
        data += "</tr>";
        $("#progressTable").append(data); 
    },

    updatePlayerInfo : function(name, hp, maxHP, atk, def, hit)
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
    },
    
    updateMap : function(mapdata, step) {
        $("#mapBar").empty();
        
        var symbols = ["○", "S", "G", "M", "T", "R", "H"];
        var data = "";
        for (var i = 0; i < mapdata.length; i++) {
            
            if (i == step) {
                data += "<div class='activeMarker'>";
                data += symbols[mapdata[i]];
                data += "</div>";
            } else {
                data += symbols[mapdata[i]];
            }
            data += " ";
        }
        $("#mapBar").append(data); 
    },
    
    clearEnemyInfo : function()
    {
        this.updateEnemyInfo("", "", "", "", "", "", "");
    },
    
    updateEnemyInfo : function(name, exp, hp, maxHP, atk, def, hit)
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
    },
    
    addLog : function(categoryName, message) 
    {
        var data = ""
        data += "<tr>";
        data += "<td class='column'>" + categoryName + "</td>";
        data += "<td>" + message + "</td>";
        data += "</tr>";
        $("#logTable").append(data);
        
        $("#logBox").animate({ scrollTop: $("#logTable").height() }, 0);
    },
    
    showItemDesc : function(no) {
        var itemDesc = cache.itemdata[no];
        $("#itemTable").empty();
        var data = "";
        data += "<button onclick='MainView.updateItems()'>←</button>";
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
                data += "<td class='column'>" + cache.buffdata[buff.id].name + "</td>";
                data += "<td>" + Utils.visualizeSigned(buff.value) + "</td>";
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
    },
    
    addItem : function(no, equipped, categoryName, itemName) 
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
        data += "<td><button onclick='MainView.showItemDesc(" + no + ")'>→</button></td>";
        data += "</tr>";
    
        $("#itemTable").append(data);
    },
    
    updateItems : function() 
    {
        $("#itemTable").empty();
        for (var i = 0; i < cache.itemdata.length; i++) {
            var itemDesc = cache.itemdata[i];
            this.addItem(i, itemDesc.equip, itemDesc.category, itemDesc.name);
        }
    },
    
    updateActionButtons : function(currentState) 
    {
        $("#actionButtons").empty();
        var data = "";
        switch (currentState) {
            case 0: // Nothing
                data += "<button onClick='Main.onAction(\"goBack\");'>back</button>";
                data += "<button onClick='Main.onAction(\"goForward\");'>go</button>";
            break;
            case 1: // Start
                data += "<button onClick='Main.onAction(\"goHome\");'>goHome</button>";
                data += "<button onClick='Main.onAction(\"goForward\");'>go</button>";
            break;
            case 2: // Goal
                data += "<button onClick='Main.onAction(\"goBack\");'>back</button>";
                data += "<button onClick='Main.onAction(\"stairDown\");'>go down</button>";
            break;
            case 3: // Monster
                data += "<button onClick='Main.onAction(\"monsterEscape\");'>escape</button>";
                data += "<button onClick='Main.onAction(\"monsterAttack\");'>attack</button>";
            break;
            case 4: // Treasure
                data += "<button onClick='Main.onAction(\"goBack\");'>back</button>";
                data += "<button onClick='Main.onAction(\"treasureOpen\");'>open</button>";
                data += "<button onClick='Main.onAction(\"goForward\");'>go</button>";
            break;
            case 5: // Metal
                data += "<button onClick='Main.onAction(\"goBack\");'>back</button>";
                data += "<button onClick='Main.onAction(\"metalDig\");'>dig</button>";
                data += "<button onClick='Main.onAction(\"goForward\");'>go</button>";
            break;
            case 6: // Herbs
                data += "<button onClick='Main.onAction(\"goBack\");'>back</button>";
                data += "<button onClick='Main.onAction(\"herbHarvest\");'>get</button>"
                data += "<button onClick='Main.onAction(\"goForward\");'>go</button>";
            break;
        }
        
        $("#actionButtons").append(data);
    }
}
