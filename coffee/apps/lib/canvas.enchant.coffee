enchant.canvas = 
    SceneGraph : class
      constructor: (game, surface) ->
        assert surface
        @_game = game
        @_surface = surface
        @_root = null
    
      setRoot: (node) ->
        @_root = node
    
      prioritySortFunc: (a, b) ->
        a.absPriority - b.absPriority
    
      update: ->
        assert @_root
        @_root.updateTransform()
        @_root.updateAttributes()
        drawCommmands = []
        @_root.registerDrawCommand drawCommmands
        drawCommmands.sort @prioritySortFunc
        i = 0
    
        while i < drawCommmands.length
          drawCommmands[i].draw @_game.assets, @_surface
          i++

enchant.canvas.Node = enchant.Class.create(
  initialize: (baseTransform) ->
    @position = [ 0, 0 ]
    @velocity = [ 0, 0 ]
    @size = [ 1, 1 ]
    @scale = [ 1, 1 ]
    @alpha = 1.0
    @absAlpha = 1.0
    @depth = 0
    @priority = 0.5
    @absPriority = 0.5
    @blendType = null
    @hue = [ 0, 0, 0 ]
    @rotation = 0
    @parent = null
    @_children = []
    @_transform = null
    @_hasBaseTransform = false
    @center = [ 0, 0 ]
    if baseTransform
      @_hasBaseTransform = true
      @_transform = baseTransform

  children:
    get: ->
      @_children

    set: (children) ->
      console.log "Sprite : children cannot be directly set!"

  getOffsetByPositionAnchor: (positionAnchor) ->
    centerX = @size[0] / 2 + @center[0]
    centerY = @size[1] / 2 + @center[1]
    [ centerX + (positionAnchor[0] * (@size[0] / 2) - centerX) * @scale[0], centerY + (positionAnchor[1] * (@size[1] / 2) - centerY) * @scale[1] ]

  transform:
    get: ->
      unless @_transform
        if @parent
          @updateTransform()
        else
          @updateTransform()
      @_transform

    set: (transform) ->
      @_transform = transform

  addChild: (node) ->
    @_children.push node
    node.parent = this

  removeChild: (node) ->
    i = 0
    while i < @_children.length
      if @_children[i] is node
        node.parent = null
        @_children.splice i, 1
        break
      i++

  removeAllChildren: ->
    i = 0
    while i < @_children.length
      @_children[i].parent = null
      i++
    @_children.length = 0

  applyTransform: (context) ->
    t = @_transform
    context.setTransform t[0][0], t[0][1], t[1][0], t[1][1], t[2][0], t[2][1]

  updateTransform: ->
    parentTransform = null
    parentTransform = @parent.transform  if @parent
    if @_hasBaseTransform
      matrix = enchant.matrix.createMatrixIdentity(3)
      matrix[0][0] = parentTransform[0][0]
      matrix[0][1] = parentTransform[0][1]
      matrix[1][0] = parentTransform[1][0]
      matrix[1][1] = parentTransform[1][1]
      velocity = enchant.matrix.transformPoint(@velocity, matrix)
      @_transform[2][0] += velocity[0]
      @_transform[2][1] += velocity[1]
    else
      transform = enchant.matrix.getNodeTransformMatirx(@position[0], @position[1], @rotation * Math.PI / 180, @scale[0], @scale[1])
      transform = enchant.matrix.matrixMultiply(transform, parentTransform)  if parentTransform
      @_transform = transform
    i = 0

    while i < @_children.length
      @_children[i].updateTransform()
      i++

  updateAttributes: ->
    if @parent
      @depth = @parent.depth + 1
      @absAlpha = @alpha * @parent.alpha
      @absPriority = 2 * @priority * @parent.absPriority
    else
      @depth = 0
      @absAlpha = @alpha
      @absPriority = @priority
    i = 0

    while i < @_children.length
      @_children[i].updateAttributes()
      i++

  registerDrawCommand: (drawCommands) ->
    return  if @absAlpha <= 0
    i = 0

    while i < @_children.length
      @_children[i].registerDrawCommand drawCommands
      i++
)
enchant.canvas.Sprite = enchant.Class.create(
  initialize: (srcPath, srcRect) ->
    @node = new enchant.canvas.Node()
    @srcPath = (if srcPath then srcPath else null)
    @srcRect = (if srcRect then srcRect else null)

  addChild: (node) ->
    @node.addChild node

  removeChild: (node) ->
    @node.removeChild node

  removeAllChildren: ->
    @node.removeAllChildren()

  getOffsetByPositionAnchor: (positionAnchor) ->
    @node.getOffsetByPositionAnchor positionAnchor

  size:
    get: ->
      @node.size

    set: (value) ->
      @node.size = value

  position:
    get: ->
      @node.position

    set: (value) ->
      @node.position = value

  alpha:
    get: ->
      @node.alpha

    set: (value) ->
      @node.alpha = value

  absAlpha:
    get: ->
      @node.absAlpha

  blendType:
    get: ->
      @node.blendType

    set: (value) ->
      @node.blendType = value

  center:
    get: ->
      @node.center

    set: (value) ->
      @node.center = value

  depth:
    get: ->
      @node.depth

    set: (value) ->
      @node.depth = value

  priority:
    get: ->
      @node.priority

    set: (value) ->
      @node.priority = value

  absPriority:
    get: ->
      @node.absPriority

  hue:
    get: ->
      @node.hue

    set: (value) ->
      @node.hue = value

  parent:
    get: ->
      @node.parent

    set: (value) ->
      @node.parent = value

  rotation:
    get: ->
      @node.rotation

    set: (value) ->
      @node.rotation = value

  scale:
    get: ->
      @node.scale

    set: (value) ->
      @node.scale = value

  children:
    get: ->
      @node.children

    set: (children) ->
      @node.children = children

  transform:
    get: ->
      @node.transform

    set: (transform) ->
      @node.transform = transform

  applyTransform: (context) ->
    @node.applyTransform context

  updateTransform: ->
    parentTransform = null
    parentTransform = @parent.transform  if @parent
    if @srcPath
      transform = enchant.matrix.getImageTransformMatirx(-@position[0], -@position[1], @rotation * Math.PI / 180, @scale[0], @scale[1])
      transform = enchant.matrix.matrixMultiply(transform, parentTransform)  if parentTransform
      @transform = transform
      i = 0

      while i < @children.length
        @children[i].updateTransform()
        i++
    else
      @node.updateTransform()

  updateAttributes: ->
    @node.updateAttributes()

  registerDrawCommand: (drawCommands) ->
    return  if @absAlpha <= 0
    if @srcPath
      drawCommands.push this
    else
      @node.registerDrawCommand drawCommands

  draw: (assets, surface) ->
    return  if @absAlpha is 0
    key = "../../static/assets/images/" + @srcPath
    src = assets[key]
    assert src isnt `undefined`, "No file found at path = " + key
    @srcRect = [ 0, 0, src.width, src.height ]  unless @srcRect
    @size = [ @srcRect[2], @srcRect[3] ]  unless @size
    @applyTransform surface.context
    surface.context.globalAlpha = @absAlpha
    operation = undefined
    if @blendType is "add"
      operation = "lighter"
    else
      operation = "source-over"
    surface.context.globalCompositeOperation = operation  unless operation is surface.context.globalCompositeOperation
    assert typeof (@srcRect[0]) is "number", "1"
    assert typeof (@srcRect[1]) is "number", "2"
    assert typeof (@srcRect[2]) is "number", "3"
    assert typeof (@srcRect[3]) is "number", "4"
    assert typeof (@position[0]) is "number", "5"
    assert typeof (@position[1]) is "number", "6"
    posX = Math.floor(@position[0] - @center[0] - @srcRect[2] / 2)
    posY = Math.floor(@position[1] - @center[1] - @srcRect[3] / 2)
    uvCutX = 1
    uvCutY = 0
    surface.draw src, @srcRect[0], @srcRect[1], @srcRect[2] - uvCutX, @srcRect[3] - uvCutY, posX, posY, @srcRect[2], @srcRect[3]
)