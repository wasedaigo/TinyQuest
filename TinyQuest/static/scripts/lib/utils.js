
  this.module = function(names, fn) {
    var space, _name;
    if (typeof names === 'string') names = names.split('.');
    space = this[_name = names.shift()] || (this[_name] = {});
    space.module || (space.module = this.module);
    if (names.length) {
      return space.module(names, fn);
    } else {
      return fn.call(space);
    }
  };

  window.Utils = {
    pendingRequest: {},
    clone: function(src) {
      var i, newObj;
      newObj = (src instanceof Array ? [] : {});
      for (i in src) {
        if (i === "clone") continue;
        if (src[i] && typeof src[i] === "object") {
          newObj[i] = this.clone(src[i]);
        } else {
          newObj[i] = src[i];
        }
      }
      return newObj;
    },
    loadJSON: function(name, callback) {
      return this.loadJSONWithData(name, "", callback);
    },
    loadJSONWithData: function(name, data, callback) {
      var self;
      if (!this.pendingRequest[name]) {
        this.pendingRequest[name] = true;
        self = this;
        return $.ajax({
          url: name,
          type: "GET",
          data: data,
          dataType: "json",
          timeout: 5000,
          error: function(data) {
            alert("Server Error: " + name);
            return self.pendingRequest[name] = false;
          },
          success: function(json) {
            callback(json);
            return self.pendingRequest[name] = false;
          }
        });
      }
    },
    visualizeSigned: function(value) {
      if (value >= 0) value = "+" + value;
      return value;
    }
  };
