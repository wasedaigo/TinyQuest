
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

  this.module('roga', function() {
    return this.Helper = (function() {
      var instance;

      function Helper() {}

      instance = null;

      Helper.instance = function() {
        if (!(instance != null)) instance = new this;
        return instance;
      };

      Helper.prototype.clone = function(src) {
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
      };

      return Helper;

    })();
  });
