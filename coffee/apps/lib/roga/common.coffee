@module = (names, fn) ->
  names = names.split '.' if typeof names is 'string'
  space = @[names.shift()] ||= {}
  space.module ||= @module
  if names.length
    space.module names, fn
  else
    fn.call space

@module 'roga', ->
  class @Helper
    # We can make private variables!
    instance = null    
  
    # Static singleton retriever/loader
    @instance: ->
      if not instance?
        instance = new @
      instance

    clone: (src) ->
      newObj = (if (src instanceof Array) then [] else {})
      for i of src
        continue  if i is "clone"
        if src[i] and typeof src[i] is "object"
          newObj[i] = @clone(src[i])
        else
          newObj[i] = src[i]
      newObj

