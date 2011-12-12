enchant.loader =
  setRootPath: (rootPath) ->
    @rootPath = rootPath

  setAnimation: (path, data) ->
    @set @rootPath + "/animations/" + path + ".json", data

  getAnimation: (path) ->
    @get @rootPath + "/animations/" + path + ".json"

  set: (path, data) ->
    enchant.Game.instance.assets[path] = data

  get: (path) ->
    data = enchant.Game.instance.assets[path]
    console.log "[enchant.loader] No path '" + path + "' defined in cache"  unless data
    data

  load: (assets, cb) ->
    if assets.length > 0
      loaded = 0
      i = 0
      len = assets.length

      while i < len
        enchant.Game.instance.load assets[i], ->
          loaded = ++loaded
          cb.call()  if loaded is len
        i++
    else
      cb.call()