@module = (names, fn) ->
  names = names.split '.' if typeof names is 'string'
  space = @[names.shift()] ||= {}
  space.module ||= @module
  if names.length
    space.module names, fn
  else
    fn.call space

window.Utils =
  pendingRequest: {}
  clone: (src) ->
    newObj = (if (src instanceof Array) then [] else {})
    for i of src
      continue  if i is "clone"
      if src[i] and typeof src[i] is "object"
        newObj[i] = @clone(src[i])
      else
        newObj[i] = src[i]
    newObj

  loadJSON: (name, callback) ->
    @loadJSONWithData name, "", callback

  loadJSONWithData: (name, data, callback) ->
    unless @pendingRequest[name]
      @pendingRequest[name] = true
      self = this
      $.ajax
        url: name
        type: "GET"
        data: data
        dataType: "json"
        timeout: 5000
        error: (data) ->
          alert "Server Error: " + name
          self.pendingRequest[name] = false

        success: (json) ->
          callback json
          self.pendingRequest[name] = false

  visualizeSigned: (value) ->
    value = "+" + value  if value >= 0
    value