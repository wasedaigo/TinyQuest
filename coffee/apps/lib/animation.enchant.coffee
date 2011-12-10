enchant.utils = clone: (src) ->
  newObj = (if (src instanceof Array) then [] else {})
  for i of src
    continue  if i is "clone"
    if src[i] and typeof src[i] is "object"
      newObj[i] = enchant.utils.clone(src[i])
    else
      newObj[i] = src[i]
  newObj

enchant.animation = {}
enchant.animation.animationManager =
  initialize: (root) ->
    @root = root
    @instances = []

  start: (animation) ->
    @instances.push animation
    @root.addChild animation.node
    animation.interval.start()

  update: ->
    i = @instances.length - 1

    while i >= 0
      instance = @instances[i]
      instance.interval.update()
      if instance.interval.isDone()
        @root.removeChild instance.node
        @instances.splice i, 1
      i--

  CreateAnimation: (data, isSubAnimation, baseTransform, baseAlpha, basePriority, target) ->
    timelines = data["timelines"]
    parallels = []
    node = new enchant.canvas.Node(baseTransform)
    node.alpha = baseAlpha
    node.priority = basePriority
    for timelineNo of timelines
      timeline = timelines[timelineNo]
      sprite = new enchant.canvas.Sprite()
      sequences = []
      attributes = [ "alpha", "scale", "position", "rotation" ]
      for i of attributes
        attribute = attributes[i]
        sequence = enchant.animation.animationManager.CreateAttributeTween(sprite, attribute, timeline[attribute], target)
        sequences.push sequence  if sequence
      sourceInterval = new enchant.animation.interval.SourceInterval(sprite, timeline["source"], target)
      sequences.push sourceInterval
      parallels.push new enchant.animation.interval.Parallel(sequences)
      node.addChild sprite
    parallelInterval = new enchant.animation.interval.Parallel(parallels)
    interval = null
    if isSubAnimation
      interval = new enchant.animation.interval.Loop(parallelInterval, 0)
    else
      interval = parallelInterval
    interval: interval
    node: node

  CreateAttributeTween: (node, attribute, keyframes, target) ->
    if keyframes.length is 0
      null
    else
      intervals = []
      i = 0

      while i < keyframes.length
        frame = keyframes[i]
        tween = frame.tween
        interval = null
        if frame.wait
          duration = frame.duration
          interval = new enchant.animation.interval.Wait(duration)
        else
          startValue = frame.startValue
          endValue = frame.endValue
          duration = frame.duration
          options = {}
          if attribute is "position"
            options.startPositionAnchor = frame.startPositionAnchor
            options.endPositionAnchor = frame.endPositionAnchor
            options.startPositionType = frame.startPositionType
            options.endPositionType = frame.endPositionType
            options.target = target
          else if attribute is "rotation"
            options.facingOption = frame.facingOption
            options.target = target
          interval = new enchant.animation.interval.AttributeInterval(node, attribute, startValue, endValue, duration, tween, options)
        intervals.push interval
        i++
      new enchant.animation.interval.Sequence(intervals)

enchant.animation.interval =
  Completement: (startValue, endValue, proportion) ->
    result = null
    if typeof (startValue) is "number"
      result = startValue + (endValue - startValue) * proportion
    else if typeof (startValue) is "object"
      result = []
      i = 0

      while i < startValue.length
        value = startValue[i] + (endValue[i] - startValue[i]) * proportion
        result.push value
        i++
    result

  _GetRelativePosition: (node, target, positionType, positionAnchor, offset) ->
    result = offset
    if positionType is "relativeToTarget"
      anchorOffset = target.node.getOffsetByPositionAnchor(positionAnchor)
      tempPos = [ target.node.position[0], target.node.position[1] ]
      tempPos[0] = tempPos[0] + offset[0] + anchorOffset[0]
      tempPos[1] = tempPos[1] + offset[1] + anchorOffset[1]
      targetPosition = enchant.matrix.transformPoint(tempPos, target.node.parent.transform)
      invertMatrix = enchant.matrix.createInverseMatrix(node.parent.transform, 3)
      result = enchant.matrix.transformPoint(targetPosition, invertMatrix)
    else if positionType is "relativeToTargetOrigin"
      anchorOffset = target.node.getOffsetByPositionAnchor(positionAnchor)
      tempPos = [ target.origin[0], target.origin[1] ]
      tempPos[0] = tempPos[0] + offset[0] + anchorOffset[0]
      tempPos[1] = tempPos[1] + offset[1] + anchorOffset[1]
      targetPosition = enchant.matrix.transformPoint(tempPos, target.node.parent.transform)
      invertMatrix = enchant.matrix.createInverseMatrix(node.parent.transform, 3)
      result = enchant.matrix.transformPoint(targetPosition, invertMatrix)
    result

  CalculateRelativePosition: (startValue, endValue, node, startPositionType, endPositionType, startPositionAnchor, endPositionAnchor, target) ->
    resultStartValue = startValue
    resultEndValue = endValue
    if target and node.parent
      resultStartValue = _GetRelativePosition(node, target, startPositionType, startPositionAnchor, startValue)
      resultEndValue = _GetRelativePosition(node, target, endPositionType, endPositionAnchor, endValue)
    [ resultStartValue, resultEndValue ]

  CalculateDynamicRotation: (startValue, endValue, node, facingOption, target, dataStore) ->
    if target
      invertMatrix = null
      if facingOption is "faceToDir"
        absStartPosition = undefined
        absTargetPosition = undefined
        if node.parent
          absStartPosition = enchant.matrix.transformPoint(node.position, node.parent.transform)
          absTargetPosition = enchant.matrix.transformPoint(target.position, node.parent.transform)
        else
          absStartPosition = node.position
          absTargetPosition = target.position
        dx = absTargetPosition[0] - absStartPosition.position[0]
        dy = absTargetPosition[1] - absStartPosition.position[1]
        startValue = (Math.atan2(dy, dx) / Math.PI) * 180
        endValue = startValue
      else if facingOption is "faceToMov"
        absStartPosition = (if dataStore.lastAbsPosition then dataStore.lastAbsPosition else [ 0, 0 ])
        absTargetPosition = node.position
        absTargetPosition = enchant.matrix.transformPoint(node.position, node.parent.transform)  if node.parent
        dx = absTargetPosition[0] - absStartPosition[0]
        dy = absTargetPosition[1] - absStartPosition[1]
        startValue += (Math.atan2(dy, dx) / Math.PI) * 180
        endValue = startValue
        if dataStore.ignore
          node.parent.alpha = dataStore.lastAlpha
        else
          dataStore.ignore = true
          dataStore.lastAlpha = node.parent.alpha
          node.parent.alpha = 0
        dataStore.lastAbsPosition = absTargetPosition
    [ startValue, endValue ]

  CalculateAttributeValues: (attribute, startValue, endValue, node, options, dataStore) ->
    result = null
    if attribute is "position"
      result = enchant.animation.interval.CalculateRelativePosition(startValue, endValue, node, options.startPositionType, options.endPositionType, options.startPositionAnchor, options.endPositionAnchor, options.target)
    else result = enchant.animation.interval.CalculateDynamicRotation(startValue, endValue, node, options.facingOption, options.target, dataStore)  if attribute is "rotation"
    result

enchant.animation.interval.AttributeInterval = enchant.Class.create(
  initialize: (node, attribute, startValue, endValue, duration, tween, options) ->
    @_node = node
    @_startValue = startValue
    @_endValue = endValue
    @_duration = duration
    @_attribute = attribute
    @_frameNo = 0
    @_tween = tween
    @_dataStore = {}
    @_options = options

  isDone: ->
    @_frameNo >= @_duration

  reset: ->
    @_frameNo = 0
    @start()

  start: ->
    @_node[@_attribute] = @_startValue
    @_frameNo = 0
    @updateValue()

  finish: ->

  updateValue: ->
    startValue = @_startValue
    endValue = @_endValue
    result = enchant.animation.interval.CalculateAttributeValues(@_attribute, @_startValue, @_endValue, @_node, @_options, @_dataStore)
    if result
      startValue = result[0]
      endValue = result[1]
    value = startValue
    value = enchant.animation.interval.Completement(startValue, endValue, @_frameNo / @_duration)  if @_tween
    @_node[@_attribute] = value

  update: ->
    unless @isDone()
      @_frameNo++
      @updateValue()
)
enchant.animation.interval.Wait = enchant.Class.create(
  initialize: (duration) ->
    @_duration = duration
    @_frameNo = 0

  isDone: ->
    @_frameNo >= @_duration

  reset: ->
    @_frameNo = 0
    @start()

  start: ->
    @_frameNo = 0

  finish: ->

  update: ->
    @_frameNo++  unless @isDone()
)
enchant.animation.interval.SourceInterval = enchant.Class.create(
  initialize: (sprite, sourceKeykeyframes, target) ->
    @_sprite = sprite
    @_interval = null
    @_sourceKeykeyframes = enchant.utils.clone(sourceKeykeyframes)
    @_frameNo = 0
    @_index = 0
    @_frameDuration = 0
    @_duration = 0
    @_target = target
    @_lastAnimationId = ""
    for key of @_sourceKeykeyframes
      @_duration += @_sourceKeykeyframes[key].duration

  isDone: ->
    @_frameNo >= @_duration

  _clearSetting: ->
    @_sprite.srcRect = null
    @_sprite.srcPath = null
    if @_interval
      @_interval = null
      @_sprite.removeAllChildren()

  _updateKeyframe: (keyframe) ->
    @_clearSetting()  unless @_lastAnimationId is keyframe.id
    @_lastAnimationId = keyframe.id
    @_sprite.priority = (if keyframe.priority then keyframe.priority else 0.5)
    @_sprite.blendType = (if keyframe.blendType then keyframe.blendType else "none")
    if keyframe.type is "image"
      @_sprite.srcPath = keyframe.id + ".png"
      @_sprite.srcRect = keyframe.rect
      @_sprite.center = keyframe.center
    else
      if @_interval
        @_interval.update()
      else
        if keyframe.emitter
          transform = null
          if @_sprite.parent
            transform = @_sprite.parent.transform
            transform = enchant.matrix.getNodeTransformMatirx(@_sprite.position[0], @_sprite.position[1], @_sprite.rotation * Math.PI / 180, @_sprite.scale[0], @_sprite.scale[1])
            transform = enchant.matrix.matrixMultiply(transform, @_sprite.parent.transform)
          animation = enchant.animation.animationManager.CreateAnimation(enchant.loader.getAnimation(keyframe.id), false, transform, @_sprite.alpha, @_sprite.priority, @_target)
          if keyframe.maxEmitSpeed > 0
            speed = keyframe.minEmitSpeed + (keyframe.maxEmitSpeed - keyframe.minEmitSpeed) * Math.random()
            angle = keyframe.minEmitAngle + (keyframe.maxEmitAngle - keyframe.minEmitAngle) * Math.random()
            rad = (angle / 180) * Math.PI
            animation.node.velocity[0] = speed * Math.cos(rad)
            animation.node.velocity[1] = speed * Math.sin(rad)
          enchant.animation.animationManager.start animation
        else
          if keyframe.id
            @_sprite.updateTransform()
            animation = enchant.animation.animationManager.CreateAnimation(enchant.loader.getAnimation(keyframe.id), true, null, 1, 0.5, @_target)
            @_sprite.addChild animation.node
            @_interval = animation.interval
            @_interval.start()

  reset: ->
    @_frameNo = 0
    @_index = 0
    @_frameDuration = 0
    @_interval.reset()  if @_interval

  start: ->
    keyframe = @_sourceKeykeyframes[0]
    @_updateKeyframe keyframe

  finish: ->
    @_clearSetting()

  update: ->
    if @isDone()
      @_clearSetting()
    else
      @_frameDuration++
      @_frameNo++
      keyframe = @_sourceKeykeyframes[@_index]
      @_updateKeyframe keyframe
      if @_frameDuration >= keyframe.duration
        @_index++
        @_frameDuration = 0
)
enchant.animation.interval.Loop = enchant.Class.create(
  initialize: (interval, loopCount) ->
    @_loopCounter = 0
    @_loopCount = loopCount
    @_interval = interval

  isDone: ->
    _isDone = false
    if @_loopCount is 0
      _isDone = false
    else
      _isDone = @_interval.isDone() and @_loopCounter >= @_loopCount - 1
    _isDone

  reset: ->
    @_loopCounter = 0
    @_interval.reset()

  start: ->
    @_interval.start()

  finish: ->
    @_interval.finish()

  update: ->
    unless @isDone()
      @_interval.update()
      if @_interval.isDone()
        @_loopCounter++
        @_interval.reset()  if @_loopCount is 0 or @_loopCounter < @_loopCount
)
enchant.animation.interval.Sequence = enchant.Class.create(
  initialize: (intervals) ->
    @_intervals = intervals
    @_index = 0
    length = @_intervals.length
    @_lastInterval = @_intervals[length - 1]

  isDone: ->
    @_lastInterval.isDone()

  reset: ->
    @_index = 0
    for i of @_intervals
      @_intervals[i].reset()
    @start()

  start: ->
    @_intervals[0].start()

  finish: ->
    @_intervals[@_index].finish()

  update: ->
    unless @isDone()
      currentInterval = @_intervals[@_index]
      currentInterval.update()
      if @isDone()
        @finish()
      else @_index++  if currentInterval.isDone()
)
enchant.animation.interval.Parallel = enchant.Class.create(
  initialize: (intervals) ->
    @_intervals = intervals

  isDone: ->
    isDone = true
    for i of @_intervals
      unless @_intervals[i].isDone()
        isDone = false
        break
    isDone

  reset: ->
    for i of @_intervals
      @_intervals[i].reset()
    @start()

  start: ->
    for i of @_intervals
      @_intervals[i].start()

  finish: ->
    for i of @_intervals
      @_intervals[i].finish()

  update: ->
    unless @isDone()
      for i of @_intervals
        @_intervals[i].update()
      @finish()  if @isDone()
)