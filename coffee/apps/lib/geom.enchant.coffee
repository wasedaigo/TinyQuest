enchant.geom = {}
enchant.geom.Size = class
    constructor: (width, height) ->
      @width = width
      @height = height

enchant.geom.Point = class
    constructor: (x, y) ->
        @x = x
        @y = y

enchant.geom.Rectangle = class
    constructor: (x, y, width, height) ->
        @x = x
        @y = y
        @width = width
        @height = height

    getLeft: ->
        @x
        
    getTop: ->
        @y
        
    getRight: ->
        @x + @width
    
    getBottom: ->
        @y + @height