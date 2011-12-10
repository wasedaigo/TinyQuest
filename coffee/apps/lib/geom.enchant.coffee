enchant.geom = {}
enchant.geom.Size = enchant.Class.create(initialize: (width, height) ->
  @width = width
  @height = height
)
enchant.geom.Point = enchant.Class.create(initialize: (x, y, width, height) ->
  @x = x
  @y = y
)
enchant.geom.Rectangle = enchant.Class.create(
  initialize: (x, y, width, height) ->
    @x = x
    @y = y
    @width = width
    @height = height

  right:
    get: ->
      @x + @width

  bottom:
    get: ->
      @y + @height
)