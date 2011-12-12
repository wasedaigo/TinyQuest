
  enchant.loader = {
    setRootPath: function(rootPath) {
      return this.rootPath = rootPath;
    },
    setAnimation: function(path, data) {
      return this.set(this.rootPath + "/animations/" + path + ".json", data);
    },
    getAnimation: function(path) {
      return this.get(this.rootPath + "/animations/" + path + ".json");
    },
    set: function(path, data) {
      return enchant.Game.instance.assets[path] = data;
    },
    get: function(path) {
      var data;
      data = enchant.Game.instance.assets[path];
      if (!data) {
        console.log("[enchant.loader] No path '" + path + "' defined in cache");
      }
      return $.parseJSON(data);
    },
    load: function(assets, cb) {
      var i, len, loaded, _results;
      if (assets.length > 0) {
        loaded = 0;
        i = 0;
        len = assets.length;
        _results = [];
        while (i < len) {
          enchant.Game.instance.load(assets[i], function() {
            loaded = ++loaded;
            if (loaded === len) return cb.call();
          });
          _results.push(i++);
        }
        return _results;
      } else {
        return cb.call();
      }
    }
  };
