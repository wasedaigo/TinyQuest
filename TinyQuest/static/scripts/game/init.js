var POINTER_MOUSE_CURSOR = (navigator.userAgent.search(/KHTML|Opera/i) >= 0 ? 'pointer' : (document.attachEvent ? 'url(/static/images/world/grab.cur)' : '-moz-grab'));
var GRAB_MOUSE_CURSOR = (navigator.userAgent.search(/KHTML|Opera/i) >= 0 ? 'pointer' : (document.attachEvent ? 'url(/static/images/world/grab.cur)' : '-moz-grab'));
var GRABBING_MOUSE_CURSOR = (navigator.userAgent.search(/KHTML|Opera/i) >= 0 ? 'move' : (document.attachEvent ? 'url(/static/images/world/grabbing.cur)' : '-moz-grabbing'));

var gNullEvent = function(e){ e.preventDefault();};
var gNullOnEvent = function(e){return false;}; 
var gState;
var gTemplates = null;
var gLocalizedTexts = null;
var gReady = false;
var gResourceLoaded = false;
var gMainLoaded = false;
var gLoginInfo = null;
var sLoadFlags = {"dom":false, "resource":false, "main":false, "login":false, "template":false}

function setLoadFlag(flag)
{
	sLoadFlags[flag] = true;
	checkAllLoaded();
}

function checkAllLoaded()
{
	for(var key in sLoadFlags)
	{
		if (!sLoadFlags[key])
		{
			return;
		}
	}

	if (gLoginInfo.login)
	{
		// Already logged in
		$('#mainContainer').html(gTemplates["afterLogin"]);
		Main.loadState("adventure");
	}
	else
	{
		// Not logged in yet, it needs to login the game!
		$('#mainContainer').html(gTemplates["beforeLogin"]);

		var link = $("<a></a>").attr("href", gLoginInfo.loginURL).text("Login");
		$('#loginURL').append(link);
	}
}

// Load template XML
function loadTemplate()
{
  $.ajax({
      url: 'static/templates.xml',
      type: 'GET',
      dataType: 'xml',
      cache: true,  
      timeout: 5000, 
      error: function(){
        alert('Error: load template');
      },
      success: function(xml)
      {
        gTemplates = {}
        $(xml).find("template").each(function()
        {
          key = $(this).attr("key");
          value = $(this).text();
          gTemplates[key] = value;
        });
        
        setLoadFlag("template");
      }
  });
}

// Load resources
function loadResources()
{
  setLoadFlag("resource");
}

// Acquire login information
function loadLoginInfo()
{
  Utils.loadJSON("api/GetLoginInfo", function(json)
  {
    gLoginInfo = json;
    setLoadFlag("login");
  });
}

// Check everything is loaded after DOM got loaded
$(function(){
  setLoadFlag("dom");
  $("#Admin").hide();
})

function initialize()
{
  loadTemplate();
  loadResources();
  loadLoginInfo();
}
initialize();