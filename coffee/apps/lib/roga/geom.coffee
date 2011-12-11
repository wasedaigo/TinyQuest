@module "roga.geom", ->
  class @Size
    constructor: (width, height) ->
      @width = width
      @height = height

  class @Point
    constructor: (x, y) ->
        @x = x
        @y = y
  
  class @Rectangle
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