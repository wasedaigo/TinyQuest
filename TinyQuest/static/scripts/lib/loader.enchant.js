enchant.loader = {
  setRootPath: function(rootPath) {
    return this.rootPath = rootPath;
  },
  cache: {},
  pendingRequest: {},
  loadJSON: function(name, callback) {
    return this.loadJSONWithData(name, "", callback);
  },
  loadJSONWithData: function(name, data, callback) {
    if (!this.pendingRequest[name]) {
      this.pendingRequest[name] = true;
      return $.ajax({
        url: name,
        type: "GET",
        data: data,
        dataType: "json",
        timeout: 5000,
        error: function(data) {
          alert("Server Error: " + name);
          return this.pendingRequest[name] = false;
        },
        success: function(json) {
          callback(json);
          return this.pendingRequest[name] = false;
        }
      });
    }
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
    return data;
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
