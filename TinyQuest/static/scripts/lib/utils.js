var Utils = {
    pendingRequest : {},
    clone : function(src) {
      var newObj = (src instanceof Array) ? [] : {};
      for (i in src) {
        if (i == 'clone') continue;
        if (src[i] && typeof src[i] == "object") {
          newObj[i] = this.clone(src[i]);
        } else newObj[i] = src[i]
      } return newObj;
    },

    loadJSON : function (name, callback)
    {
      this.loadJSONWithData(name, "", callback);
    },
    
    loadJSONWithData : function (name, data, callback)
    {
      // Avoid multiple request at once
      if (!this.pendingRequest[name])
      {
        this.pendingRequest[name] = true;
        var self = this;
        $.ajax({
          url: name,
          type: 'GET',
          data: data,
          dataType: 'json',
          timeout: 5000,
          error: function(data)
          {
            alert('Server Error: ' + name);
            self.pendingRequest[name] = false;
          },
          success: function(json)
          {
            callback(json);
            self.pendingRequest[name] = false;
          }
        });
      }
    },
    
    visualizeSigned : function(value) 
    {
        if (value >= 0) {
            value = "+" + value;
        }
        
        return value;
    }
}


