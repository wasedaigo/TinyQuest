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
            }
        })
    }
    else if (state_id == "profile")
    {
        loadProfile();
    }
}

function loadAdventure(obj)
{
    templates = {
        message: "Dragon has dealt you 133 damages!!",
        hp: obj.hp,
        attack: obj.target.attack,
        defense: obj.target.defense,
        step: obj.step,
        floor: obj.floor,
        energy: obj.energy,
        enemy_hp: obj.target.hp
    };
    $.template( "template", templateHtml );
    templateHtml = $.tmpl( "template", templates );
    $('#mainContainer div.content').html(templateHtml);
    drawBG($("#adventureScreen .mainPanel"));
    $("#profileButton").attr("disabled","");
    $("#adventureButton").attr("disabled","disabled");
    $("#arenaButton").attr("disabled","");
    $("#friendsButton").attr("disabled","");
    if (gLoginInfo.admin)
    {
        $("#Admin").show();
    }
}

function loadProfile(obj)
{
    $.template( "template", templateHtml );
    templateHtml = $.tmpl( "template", templates );
    $('#mainContainer div.content').html(templateHtml);
    drawAvatar($("#homeScreen .info .avatar"));
    $("#profileButton").attr("disabled","disabled");
    $("#adventureButton").attr("disabled","");
    $("#arenaButton").attr("disabled","");
    $("#friendsButton").attr("disabled","");
    if (gLoginInfo.admin)
    {
        $("#Admin").show();
    }
}

function drawAvatar(canvas) {
    canvas.drawImage({
      source: "/static/images/avatar/chara.png",
      x: 50, y: 0,
      width: 256,
      height: 256,
      fromCenter: false
    });
}

function drawBG(canvas) {
    canvas.drawImage({
      source: "/static/images/bg/bg0001.png",
      x: 0, y: 0,
      width: 640,
      height: 364,
      fromCenter: false
    }).drawImage({
      source: "/static/images/avatar/chara.png",
      x: 50, y: 200,
      width: 128,
      height: 128,
      fromCenter: false
    }).drawImage({
      source: "/static/images/monster/enemy0001.png",
      x: 256, y: 0,
      width: 384,
      height: 384,
      fromCenter: false
    });
}

setLoadFlag("main");