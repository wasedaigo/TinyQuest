var pendingRequest = {};


function loadJSON(name, callback)
{
  loadJSONWithData(name, "", callback);
}

function loadJSONWithData(name, data, callback)
{
  // Avoid multiple request at once
  if (!pendingRequest[name])
  {
    pendingRequest[name] = true;
    $.ajax({
      url: name,
      type: 'GET',
      data: data,
      dataType: 'json',
      timeout: 5000,
      error: function(data)
      {
        alert('Server Error: ' + name);
        pendingRequest[name] = false;
      },
      success: function(json)
      {
        callback(json);
        pendingRequest[name] = false;
      }
    });
  }
}



function showModalDialog(x, y, body)
{
  var dialog = $("#dialog");
  var mask = $("#mask");

  mask.fadeTo("slow",0.3);	
  mask.click(closeDialog);

  updateDialog(body);
  
  dialog.css('left',  x - 50);
  dialog.css('top', y - 50);
  dialog.fadeIn(300); 
}

function updateDialog(body)
{
  var dialog = $("#dialog");

  dialog.empty();
  dialog.append(body);
  body.hide();
  body.fadeIn(300);
}

function closeDialog()
{
  var dialog = $("#dialog");
  var mask = $("#mask");
  
  mask.fadeOut(300);
  dialog.fadeOut(300);
}

// Helper methods
function getMouseLocalPosition(e) 
{
  // - this.viewer.position().top
  return {
    'x' : (e.pageX || (e.clientX + (document.documentElement.scrollLeft || document.body.scrollLeft))),
    'y' : (e.pageY || (e.clientY + (document.documentElement.scrollTop || document.body.scrollTop)))
  }
};

function getContainerSize()
{
  return {
    "width":$("#scrollMapViewer").width(), 
    "height":$("#scrollMapViewer").height()
  };
}