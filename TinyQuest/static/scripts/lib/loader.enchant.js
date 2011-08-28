// Interval helper modules
enchant.loader  = 
{
    setRootPath : function(rootPath) {
        this.rootPath = rootPath;
    },
    cache: {},
    pendingRequest: {},
    loadJSON: function(name, callback) {
        this.loadJSONWithData(name, "", callback);
    },
    loadJSONWithData: function(name, data, callback) {
        // Avoid multiple request at once
        if (!this.pendingRequest[name])
        {
        this.pendingRequest[name] = true;
        $.ajax({
          url: name,
          type: 'GET',
          data: data,
          dataType: 'json',
          timeout: 5000,
          error: function(data)
          {
            alert('Server Error: ' + name);
            this.pendingRequest[name] = false;
          },
          success: function(json)
          {
            callback(json);
            this.pendingRequest[name] = false;
          }
        });
        }
    },
    setAnimation: function(path, data) {
        this.set(this.rootPath + "/animations/" + path + ".json", data);
    },
    getAnimation: function(path) {
        return this.get(this.rootPath + "/animations/" + path + ".json");
    },
    set: function(path, data) {
        enchant.Game.instance.assets[path] = data;
    },
    get: function(path) {
        var data = enchant.Game.instance.assets[path];
        if (!data) {
            console.log("[enchant.loader] No path '" + path + "' defined in cache");
        }
        return data;
    },
    load: function(assets, cb) {
        if (assets.length > 0) {
            var loaded = 0;
            for (var i = 0, len = assets.length; i < len; i++) {
                enchant.Game.instance.load(assets[i], function() {
                    loaded = ++loaded;
                    if (loaded == len) {
                        cb.call();
                    }
                });
            }
        } else {
            cb.call();
        }
    }     
}