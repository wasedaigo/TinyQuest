@module "roga.canvas", ->
  #
  # Stage
  # The root of a scene graph, which manages nodes and sprites.
  #
  class @Stage
    constructor: (game, surface) ->
      assert surface
      @_game = game
      @_surface = surface
      @_root = null

    setRoot: (node) ->
      @_root = node
    
    prioritySortFunc: (a, b) ->
      a.getAbsPriority() - b.getAbsPriority()
    
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

  #
  # Node
  # Nodes are the minimum components of scenes
  #
  class @Node
    constructor: (baseTransform) ->
      @_position = [ 0, 0 ]
      @_velocity = [ 0, 0 ]
      @_size = [ 1, 1 ]
      @_scale = [ 1, 1 ]
      @_alpha = 1.0
      @_absAlpha = 1.0
      @_depth = 0
      @_priority = 0.5
      @_absPriority = 0.5
      @_blendType = null
      @_hue = [ 0, 0, 0 ]
      @_rotation = 0
      @_parent = null
      @_children = []
      @_transform = null
      @_hasBaseTransform = false
      @_center = [ 0, 0 ]
      if baseTransform
        @_hasBaseTransform = true
        @_transform = baseTransform
    
    getAttribute: (name) ->
      @["_#{name}"]
    
    setAttribute: (name, value) ->
      @["_#{name}"] = value

    getSize: ->
      @_size
    
    setSize: (value) ->
      @_size = value
    
    getPosition: ->
      @_position
    
    setPosition: (value) ->
      @_position = value
    
    getVelocity: ->
      @_velocity
    
    setVelocity: (value) ->
      @_velocity = value
      
    getAlpha: ->
      @_alpha
    
    setAlpha: (value) ->
      @_alpha = value
    
    getAbsAlpha: ->
      @_absAlpha
    
    getBlendType: ->
      @_blendType
    
    setBlendType: (value) ->
      @_blendType = value
    
    getCenter: ->
      @_center
    
    setCenter: (value) ->
      @_center = value
    
    getDepth: ->
      @_depth
    
    setDepth: ->
      @_depth = value
    
    getPriority: ->
      @_priority
    
    setPriority: (value) ->
      @_priority = value
    
    getAbsPriority: ->
      @_absPriority
    
    getHue: ->
      @_hue
    
    setHue: (value) ->
      @_hue = value
    
    getParent: ->
      @_parent
    
    setParent: (value) ->
      @_parent = value
    
    getRotation: ->
      @_rotation
    
    setRotation: (value)->
      @_rotation = value
    
    getScale: ->
      @_scale
    
    setScale: (value) ->
      @_scale = value
    
    getChildren: ->
      @_children
    
    getOffsetByPositionAnchor: (positionAnchor) ->
      centerX = @_size[0] / 2 + @_center[0]
      centerY = @_size[1] / 2 + @_center[1]
      [ centerX + (positionAnchor[0] * (@_size[0] / 2) - centerX) * @_scale[0], centerY + (positionAnchor[1] * (@_size[1] / 2) - centerY) * @_scale[1] ]
      
    getTransform: ->
      unless @_transform
        if @_parent
          @updateTransform()
        else
          @updateTransform()
      @_transform
    
    setTransform: (transform) ->
      set: (transform) ->
        @_transform = transform
    
    addChild: (node) ->
      @_children.push node
      node.setParent this
      
    removeChild: (node) ->
      i = 0
      while i < @_children.length
        if @_children[i] is node
          node.setParent null
          @_children.splice i, 1
          break
        i++
    
    removeAllChildren: ->
      i = 0
      while i < @_children.length
        @_children[i].setParent null
        i++
      @_children.length = 0
    
    applyTransform: (context) ->
      t = @_transform
      context.setTransform t[0][0], t[0][1], t[1][0], t[1][1], t[2][0], t[2][1]
    
    updateTransform: ->
      parentTransform = null
      parentTransform = @_parent.getTransform() if @_parent
      if @_hasBaseTransform
        matrix = roga.matrix.createMatrixIdentity(3)
        matrix[0][0] = parentTransform[0][0]
        matrix[0][1] = parentTransform[0][1]
        matrix[1][0] = parentTransform[1][0]
        matrix[1][1] = parentTransform[1][1]
        velocity = roga.matrix.transformPoint(@_velocity, matrix)
        @_transform[2][0] += velocity[0]
        @_transform[2][1] += velocity[1]
      else
        transform = roga.matrix.getNodeTransformMatirx(@_position[0], @_position[1], @_rotation * Math.PI / 180, @_scale[0], @_scale[1])
        transform = roga.matrix.matrixMultiply(transform, parentTransform)  if parentTransform
        @_transform = transform
      i = 0
    
      while i < @_children.length
        @_children[i].updateTransform()
        i++
    
    updateAttributes: ->
      if @_parent
          @_depth = @_parent.getDepth() + 1
          @_absAlpha = @_alpha * @_parent.getAlpha()
          @_absPriority = 2 * @_priority * @_parent.getAbsPriority()
      else
          @_depth = 0
          @_absAlpha = @_alpha
          @_absPriority = @_priority
      i = 0
    
      while i < @_children.length
          @_children[i].updateAttributes()
          i++
    
    registerDrawCommand: (drawCommands) ->
      return  if @_absAlpha <= 0
      i = 0
      
      while i < @_children.length
        @_children[i].registerDrawCommand drawCommands
        i++

  #
  # Sprite
  # Sprites are for displaying images on a scene graph
  #
  class @Sprite extends @Node
    constructor: (srcPath, srcRect) ->
      super()
      @_srcPath = srcPath or null
      @_srcRect = srcRect or null
  
    getSrcPath: ->
      @_srcPath
    
    setSrcPath: (value) ->
      @_srcPath = value
    
    getSrcRect: ->
      @_srcRect
    
    setSrcRect: (value) ->
      @_srcRect = value
        
    updateTransform: ->
      parentTransform = null
      parentTransform = @_parent.getTransform()  if @_parent
      if @_srcPath
        transform = roga.matrix.getImageTransformMatirx(-@_position[0], -@_position[1], @_rotation * Math.PI / 180, @_scale[0], @_scale[1])
        transform = roga.matrix.matrixMultiply(transform, parentTransform)  if parentTransform
        @_transform = transform
        i = 0
      
        while i < @_children.length
          @_children[i].updateTransform()
          i++
      else
          super()
    
    registerDrawCommand: (drawCommands) ->
      return  if @getAbsAlpha() is 0
      if @_srcPath
        drawCommands.push @
      else
        super drawCommands
    
    draw: (assets, surface) ->
      return  if @getAbsAlpha() is 0
      key = "../../static/assets/images/" + @_srcPath
      src = assets[key]
      assert src isnt `undefined`, "No file found at path = " + key
      @_srcRect = [ 0, 0, src.width, src.height ]  unless @_srcRect
      @_size = [ @_srcRect[2], @_srcRect[3] ]  unless @_size
      @applyTransform surface.context
  
      surface.context.globalAlpha = @getAbsAlpha()
      operation = undefined
      if @_blendType is "add"
        operation = "lighter"
      else
        operation = "source-over"

      surface.context.globalCompositeOperation = operation  unless operation is surface.context.globalCompositeOperation
      posX = Math.floor(@_position[0] - @_center[0] - @_srcRect[2] / 2)
      posY = Math.floor(@_position[1] - @_center[1] - @_srcRect[3] / 2)
      uvCutX = 1
      uvCutY = 0
      surface.draw src, @_srcRect[0], @_srcRect[1], @_srcRect[2] - uvCutX, @_srcRect[3] - uvCutY, posX, posY, @_srcRect[2], @_srcRect[3]
      