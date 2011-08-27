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

// Helper methods
function getMouseLocalPosition(e) 
{
  // - this.viewer.position().top
  return {
    'x' : (e.pageX || (e.clientX + (document.documentElement.scrollLeft || document.body.scrollLeft))),
    'y' : (e.pageY || (e.clientY + (document.documentElement.scrollTop || document.body.scrollTop)))
  }
};