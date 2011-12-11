testFrame = (root, interval, testData, i, j) ->
    expect(root._children[j].getPosition()).toEqual testData[j].position[i]
    expect(testRound(root._children[j].getScale())).toEqual testData[j].scale[i]
    expect(testRound(root._children[j].getAlpha())).toEqual testData[j].alpha[i]
    expect(root._children[j].getHue()).toEqual testData[j].hue[i]
    expect(testRound(root._children[j].getRotation())).toEqual testData[j].rotation[i]
    if root._children[j].getSrcRect()
        expect(testRound(root._children[j].getSrcRect())).toEqual testData[j].srcRect[i]
    else
        expect(root._children[j].getSrcRect()).toEqual null
    expect(root._children[j].getSrcPath()).toEqual testData[j].srcPath[i]
    expect(testRound(root._children[j].getCenter())).toEqual testData[j].center[i]
    expect(interval.isDone()).toBe testData[j].isDone[i]

describe "Animation", ->
  describe "TestInterval", ->
    it "AlphaInterval", ->
      node = new enchant.canvas.Node
      interval = new enchant.animation.interval.AttributeInterval(node, "alpha", 0.0, 1.0, 5, true)
      expect(node.getAlpha()).toBe 1
      expect(interval.isDone()).toBe false
      interval.start()
      expect(node.getAlpha()).toEqual 0.0
      expect(interval.isDone()).toBe false
      interval.update()
      expect(node.getAlpha()).toBe 0.2
      expect(interval.isDone()).toBe false
      interval.update()
      expect(node.getAlpha()).toBe 0.4
      expect(interval.isDone()).toBe false
      interval.update()
      expect(node.getAlpha()).toBe 0.6
      expect(interval.isDone()).toBe false
      interval.update()
      expect(node.getAlpha()).toBe 0.8
      expect(interval.isDone()).toBe false
      interval.update()
      expect(node.getAlpha()).toBe 1.0
      expect(interval.isDone()).toBe true
      interval.update()
      expect(node.getAlpha()).toBe 1.0
      expect(interval.isDone()).toBe true
      interval.reset()
      expect(node.getAlpha()).toEqual 0.0
      expect(interval.isDone()).toBe false

    it "PositionInterval", ->
      node = new enchant.canvas.Node()
      interval = new enchant.animation.interval.AttributeInterval(node, "position", [ 10, 10 ], [ 2, 6 ], 4, true,
        startRelative: false
        endRelative: false
        target: null
      )
      expect(node.getPosition()).toEqual [ 0, 0 ]
      expect(interval.isDone()).toBe false
      interval.start()
      expect(node.getPosition()).toEqual [ 10, 10 ]
      expect(interval.isDone()).toBe false
      interval.update()
      expect(node.getPosition()).toEqual [ 8, 9 ]
      expect(interval.isDone()).toBe false
      interval.update()
      expect(node.getPosition()).toEqual [ 6, 8 ]
      expect(interval.isDone()).toBe false
      interval.update()
      expect(node.getPosition()).toEqual [ 4, 7 ]
      expect(interval.isDone()).toBe false
      interval.update()
      expect(node.getPosition()).toEqual [ 2, 6 ]
      expect(interval.isDone()).toBe true
      interval.reset()
      expect(node.getPosition()).toEqual [ 10, 10 ]
      expect(interval.isDone()).toBe false

    it "ScaleInterval", ->
      node = new enchant.canvas.Node()
      interval = new enchant.animation.interval.AttributeInterval(node, "scale", [ 10, 10 ], [ 2, 6 ], 4, true)
      expect(node.getScale()).toEqual [ 1, 1 ]
      expect(interval.isDone()).toBe false
      interval.start()
      expect(node.getScale()).toEqual [ 10, 10 ]
      expect(interval.isDone()).toBe false
      interval.update()
      expect(node.getScale()).toEqual [ 8, 9 ]
      expect(interval.isDone()).toBe false
      interval.update()
      expect(node.getScale()).toEqual [ 6, 8 ]
      expect(interval.isDone()).toBe false
      interval.update()
      expect(node.getScale()).toEqual [ 4, 7 ]
      expect(interval.isDone()).toBe false
      interval.update()
      expect(node.getScale()).toEqual [ 2, 6 ]
      expect(interval.isDone()).toBe true
      interval.reset()
      expect(node.getScale()).toEqual [ 10, 10 ]
      expect(interval.isDone()).toBe false

    it "HueInterval", ->
      node = new enchant.canvas.Node()
      interval = new enchant.animation.interval.AttributeInterval(node, "hue", [ 10, 10, 20 ], [ 2, 6, 40 ], 4, true)
      expect(node.getHue()).toEqual [ 0, 0, 0 ]
      expect(interval.isDone()).toBe false
      interval.start()
      expect(node.getHue()).toEqual [ 10, 10, 20 ]
      expect(interval.isDone()).toBe false
      interval.update()
      expect(node.getHue()).toEqual [ 8, 9, 25 ]
      expect(interval.isDone()).toBe false
      interval.update()
      expect(node.getHue()).toEqual [ 6, 8, 30 ]
      expect(interval.isDone()).toBe false
      interval.update()
      expect(node.getHue()).toEqual [ 4, 7, 35 ]
      expect(interval.isDone()).toBe false
      interval.update()
      expect(node.getHue()).toEqual [ 2, 6, 40 ]
      expect(interval.isDone()).toBe true
      interval.reset()
      expect(node.getHue()).toEqual [ 10, 10, 20 ]
      expect(interval.isDone()).toBe false

    it "RotationInterval", ->
      node = new enchant.canvas.Node()
      interval = new enchant.animation.interval.AttributeInterval(node, "rotation", 360, 0, 3, true,
        facingOption: null
      )
      expect(node.getRotation()).toBe 0
      expect(interval.isDone()).toBe false
      interval.start()
      expect(node.getRotation()).toEqual 360
      expect(interval.isDone()).toBe false
      interval.update()
      expect(node.getRotation()).toBe 240
      expect(interval.isDone()).toBe false
      interval.update()
      expect(node.getRotation()).toBe 120
      expect(interval.isDone()).toBe false
      interval.update()
      expect(node.getRotation()).toBe 0
      expect(interval.isDone()).toBe true
      interval.reset()
      expect(node.getRotation()).toEqual 360
      expect(interval.isDone()).toBe false

    it "Wait", ->
      interval = new enchant.animation.interval.Wait(5)
      expect(interval.isDone()).toBe false
      interval.update()
      expect(interval.isDone()).toBe false
      interval.update()
      expect(interval.isDone()).toBe false
      interval.update()
      expect(interval.isDone()).toBe false
      interval.update()
      expect(interval.isDone()).toBe false
      interval.update()
      expect(interval.isDone()).toBe true
      interval.reset()
      expect(interval.isDone()).toBe false

    it "SourceInterval", ->
      sprite = new enchant.canvas.Sprite(null, [ 5, 14, 25, 30 ])
      keyframes = [
        {
            frameNo: 0,
            rect: [ 10, 10, 32, 48 ],
            id: "test",
            duration: 3,
            type: "image",
            center: [ 1, 1 ]
        },
        {
            frameNo: 3,
            rect: [ 20, 10, 22, 48 ],
            id: "test2",
            duration: 2,
            type: "image",
            center: [ 2, 2 ]
        },
        {
            frameNo: 5,
            rect: [ 20, 20, 22, 48 ],
            id: "test3",
            duration: 1,
            type: "image",
            center: [ 3, 3 ]
        },
        {
            frameNo: 6,
            rect: [ 20, 20, 22, 48 ],
            id: "test4",
            duration: 2,
            type: "image",
            center: [ 4, 4 ]
        }
      ]
      interval = new enchant.animation.interval.SourceInterval(sprite, keyframes)
      expect(sprite.getSrcPath()).toEqual null
      expect(interval.isDone()).toBe false
      interval.start()
      expect(sprite.getSrcPath()).toEqual "test.png"
      expect(sprite.getSrcRect()).toEqual [ 10, 10, 32, 48 ]
      expect(sprite.getCenter()).toEqual [ 1, 1 ]
      expect(interval.isDone()).toBe false
      interval.update()
      expect(sprite.getSrcPath()).toEqual "test.png"
      expect(sprite.getSrcRect()).toEqual [ 10, 10, 32, 48 ]
      expect(sprite.getCenter()).toEqual [ 1, 1 ]
      expect(interval.isDone()).toBe false
      interval.update()
      expect(sprite.getSrcPath()).toEqual "test.png"
      expect(sprite.getSrcRect()).toEqual [ 10, 10, 32, 48 ]
      expect(sprite.getCenter()).toEqual [ 1, 1 ]
      expect(interval.isDone()).toBe false
      interval.update()
      expect(sprite.getSrcPath()).toEqual "test.png"
      expect(sprite.getSrcRect()).toEqual [ 10, 10, 32, 48 ]
      expect(sprite.getCenter()).toEqual [ 1, 1 ]
      expect(interval.isDone()).toBe false
      interval.update()
      expect(sprite.getSrcPath()).toEqual "test2.png"
      expect(sprite.getSrcRect()).toEqual [ 20, 10, 22, 48 ]
      expect(sprite.getCenter()).toEqual [ 2, 2 ]
      expect(interval.isDone()).toBe false
      interval.update()
      expect(sprite.getSrcPath()).toEqual "test2.png"
      expect(sprite.getSrcRect()).toEqual [ 20, 10, 22, 48 ]
      expect(sprite.getCenter()).toEqual [ 2, 2 ]
      expect(interval.isDone()).toBe false
      interval.update()
      expect(sprite.getSrcPath()).toEqual "test3.png"
      expect(sprite.getSrcRect()).toEqual [ 20, 20, 22, 48 ]
      expect(sprite.getCenter()).toEqual [ 3, 3 ]
      expect(interval.isDone()).toBe false
      interval.update()
      expect(sprite.getSrcPath()).toEqual "test4.png"
      expect(sprite.getSrcRect()).toEqual [ 20, 20, 22, 48 ]
      expect(sprite.getCenter()).toEqual [ 4, 4 ]
      expect(interval.isDone()).toBe false
      interval.update()
      expect(sprite.getSrcPath()).toEqual "test4.png"
      expect(sprite.getSrcRect()).toEqual [ 20, 20, 22, 48 ]
      expect(sprite.getCenter()).toEqual [ 4, 4 ]
      expect(interval.isDone()).toBe true

    it "Sequence1", ->
      node = new enchant.canvas.Node()
      interval1 = new enchant.animation.interval.AttributeInterval(node, "alpha", 0.1, 1.0, 3, true)
      interval2 = new enchant.animation.interval.Wait(2)
      interval3 = new enchant.animation.interval.AttributeInterval(node, "alpha", 0.7, 0.0, 2, true)
      sequence = new enchant.animation.interval.Sequence([ interval1, interval2, interval3 ])
      sequence.start()
      expect(node.getAlpha()).toBe 0.1
      expect(sequence.isDone()).toBe false
      sequence.update()
      expect(node.getAlpha()).toBe 0.4
      expect(sequence.isDone()).toBe false
      sequence.update()
      expect(node.getAlpha()).toBe 0.7
      expect(sequence.isDone()).toBe false
      sequence.update()
      expect(node.getAlpha()).toBe 1.0
      expect(sequence.isDone()).toBe false
      sequence.update()
      expect(node.getAlpha()).toBe 1.0
      expect(sequence.isDone()).toBe false
      sequence.update()
      expect(node.getAlpha()).toBe 1.0
      expect(sequence.isDone()).toBe false
      sequence.update()
      expect(node.getAlpha()).toBe 0.35
      expect(sequence.isDone()).toBe false
      sequence.update()
      expect(node.getAlpha()).toBe 0.0
      expect(sequence.isDone()).toBe true
      sequence.reset()
      expect(node.getAlpha()).toBe 0.1
      expect(sequence.isDone()).toBe false

    it "Sequence2", ->
      node = new enchant.canvas.Node()
      interval1 = new enchant.animation.interval.AttributeInterval(node, "alpha", 1, 0.5, 1, false)
      interval2 = new enchant.animation.interval.AttributeInterval(node, "alpha", 0.5, 0.3, 1, false)
      interval3 = new enchant.animation.interval.AttributeInterval(node, "alpha", 0.3, 0.1, 1, true)
      interval4 = new enchant.animation.interval.AttributeInterval(node, "alpha", 0.1, 0.1, 1, true)
      sequence = new enchant.animation.interval.Sequence([ interval1, interval2, interval3, interval4 ])
      sequence.start()
      expect(node.getAlpha()).toBe 1
      expect(sequence.isDone()).toBe false
      sequence.update()
      expect(node.getAlpha()).toBe 1
      expect(sequence.isDone()).toBe false
      sequence.update()
      expect(node.getAlpha()).toBe 0.5
      expect(sequence.isDone()).toBe false
      sequence.update()
      expect(node.getAlpha()).toBe 0.1
      expect(sequence.isDone()).toBe false
      sequence.update()
      expect(node.getAlpha()).toBe 0.1
      expect(sequence.isDone()).toBe true

    it "Parallel", ->
      node = new enchant.canvas.Node()
      interval1 = new enchant.animation.interval.AttributeInterval(node, "alpha", 0.1, 1.0, 3, true)
      interval2 = new enchant.animation.interval.AttributeInterval(node, "rotation", 0, 180, 5, true,
        facingOption: null
      )
      parallel = new enchant.animation.interval.Parallel([ interval1, interval2 ])
      parallel.start()
      expect(node.getAlpha()).toBe 0.1
      expect(node.getRotation()).toBe 0
      expect(parallel.isDone()).toBe false
      parallel.update()
      expect(node.getAlpha()).toBe 0.4
      expect(node.getRotation()).toBe 36
      expect(parallel.isDone()).toBe false
      parallel.update()
      expect(node.getAlpha()).toBe 0.7
      expect(node.getRotation()).toBe 72
      expect(parallel.isDone()).toBe false
      parallel.update()
      expect(node.getAlpha()).toBe 1.0
      expect(node.getRotation()).toBe 108
      expect(parallel.isDone()).toBe false
      parallel.update()
      expect(node.getAlpha()).toBe 1.0
      expect(node.getRotation()).toBe 144
      expect(parallel.isDone()).toBe false
      parallel.update()
      expect(node.getAlpha()).toBe 1.0
      expect(node.getRotation()).toBe 180
      expect(parallel.isDone()).toBe true
      parallel.reset()
      expect(node.getAlpha()).toBe 0.1
      expect(node.getRotation()).toBe 0
      expect(parallel.isDone()).toBe false

    it "Loop count 2", ->
      node = new enchant.canvas.Node()
      interval1 = new enchant.animation.interval.AttributeInterval(node, "alpha", 0.1, 1.0, 1, true)
      interval2 = new enchant.animation.interval.Wait(1)
      interval3 = new enchant.animation.interval.AttributeInterval(node, "alpha", 0.7, 0.0, 1, true)
      sequence = new enchant.animation.interval.Sequence([ interval1, interval2, interval3 ])
      loop_ = new enchant.animation.interval.Loop(sequence, 2)
      loop_.start()
      expect(node.getAlpha()).toBe 0.1
      expect(loop_.isDone()).toBe false
      loop_.update()
      expect(node.getAlpha()).toBe 1.0
      expect(loop_.isDone()).toBe false
      loop_.update()
      expect(node.getAlpha()).toBe 1.0
      expect(loop_.isDone()).toBe false
      loop_.update()
      expect(node.getAlpha()).toBe 0.1
      expect(loop_.isDone()).toBe false
      loop_.update()
      expect(node.getAlpha()).toBe 1.0
      expect(loop_.isDone()).toBe false
      loop_.update()
      expect(node.getAlpha()).toBe 1
      expect(loop_.isDone()).toBe false
      loop_.update()
      expect(node.getAlpha()).toBe 0
      expect(loop_.isDone()).toBe true

    it "Loop count infinite", ->
      node = new enchant.canvas.Node()
      interval1 = new enchant.animation.interval.AttributeInterval(node, "alpha", 0.1, 1.0, 1, true)
      interval2 = new enchant.animation.interval.Wait(1)
      interval3 = new enchant.animation.interval.AttributeInterval(node, "alpha", 0.7, 0.0, 1, true)
      sequence = new enchant.animation.interval.Sequence([ interval1, interval2, interval3 ])
      loop_ = new enchant.animation.interval.Loop(sequence, 0)
      loop_.start()
      expect(node.getAlpha()).toBe 0.1
      expect(loop_.isDone()).toBe false
      i = 0

      while i < 4
        loop_.update()
        expect(node.getAlpha()).toBe 1.0
        expect(loop_.isDone()).toBe false
        loop_.update()
        expect(node.getAlpha()).toBe 1.0
        expect(loop_.isDone()).toBe false
        loop_.update()
        expect(node.getAlpha()).toBe 0.1
        expect(loop_.isDone()).toBe false
        i++

    animationTestResult = Smoke01: [
        {
            position: [ [ 0, 0 ], [ 0, 0 ], [ 0, 0 ], [ 0, 0 ], [ 0, 0 ], [ 0, 0 ], [ 0, 0 ], [ 0, 0 ], [ 0, 0 ], [ 0, 0 ], [ 0, 0 ], [ 0, 0 ], [ 0, 0 ], [ 0, 0 ], [ 0, 0 ], [ 0, 0 ], [ 0, 0 ] ],
            scale: [ [ 0.5, 0.5 ], [ 0.62, 0.62 ], [ 0.74, 0.74 ], [ 0.86, 0.86 ], [ 0.98, 0.98 ], [ 1.1, 1.1 ], [ 1.14, 1.14 ], [ 1.19, 1.19 ], [ 1.23, 1.23 ], [ 1.28, 1.28 ], [ 1.32, 1.32 ], [ 1.37, 1.37 ], [ 1.41, 1.41 ], [ 1.46, 1.46 ], [ 1.5, 1.5 ], [ 1.5, 1.5 ], [ 1.5, 1.5 ] ],
            alpha: [ 1, 0.93, 0.86, 0.79, 0.71, 0.64, 0.57, 0.5, 0.43, 0.36, 0.29, 0.21, 0.14, 0.07, 0.0, 0.0, 0.0 ],
            hue: [ [ 0, 0, 0 ], [ 0, 0, 0 ], [ 0, 0, 0 ], [ 0, 0, 0 ], [ 0, 0, 0 ], [ 0, 0, 0 ], [ 0, 0, 0 ], [ 0, 0, 0 ], [ 0, 0, 0 ], [ 0, 0, 0 ], [ 0, 0, 0 ], [ 0, 0, 0 ], [ 0, 0, 0 ], [ 0, 0, 0 ], [ 0, 0, 0 ], [ 0, 0, 0 ], [ 0, 0, 0 ] ],
            rotation: [ 0, 2.14, 4.29, 6.43, 8.57, 10.71, 12.86, 15, 17.14, 19.29, 21.43, 23.57, 25.71, 27.86, 30, 30, 30 ],
            srcRect: [ [ 0, 0, 32, 32 ], [ 0, 0, 32, 32 ], [ 0, 0, 32, 32 ], [ 0, 0, 32, 32 ], [ 0, 0, 32, 32 ], [ 0, 0, 32, 32 ], [ 0, 0, 32, 32 ], [ 0, 0, 32, 32 ], [ 0, 0, 32, 32 ], [ 0, 0, 32, 32 ], [ 0, 0, 32, 32 ], [ 0, 0, 32, 32 ], [ 0, 0, 32, 32 ], [ 0, 0, 32, 32 ], [ 0, 0, 32, 32 ], [ 0, 0, 32, 32 ], [ 0, 0, 32, 32 ] ],
            srcPath: [ "test.png", "test.png", "test.png", "test.png", "test.png", "test.png", "test.png", "test.png", "test.png", "test.png", "test.png", "test.png", "test.png", "test.png", "test.png", "test.png", null ],
            isDone: [ false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true ],
            center: [ [ 0, 0 ], [ 0, 0 ], [ 0, 0 ], [ 0, 0 ], [ 0, 0 ], [ 0, 0 ], [ 0, 0 ], [ 0, 0 ], [ 0, 0 ], [ 0, 0 ], [ 0, 0 ], [ 0, 0 ], [ 0, 0 ], [ 0, 0 ], [ 0, 0 ], [ 0, 0 ], [ 0, 0 ] ]
        } 
    ]

    it "AnimationTest1", ->
      animation = enchant.animation.animationManager.CreateAnimation(enchant.loader.getAnimation("Smoke01"), false)
      interval = animation.interval
      root = animation.node
      expect(root._children.length).toBe 1
      interval.start()
      frameCount = 17
      i = 0

      while i < frameCount
        if i is 0
          interval.start()
        else
          interval.update()
        testFrame root, interval, animationTestResult["Smoke01"], i, 0
        i++
    
    it "AnimationTest2", ->
      animation = enchant.animation.animationManager.CreateAnimation(enchant.loader.getAnimation("SmokeRing"), false)
      interval = animation.interval
      root = animation.node
      expect(root._children.length).toBe 8
      frameCount = 17
      timelineCount = 8
      temp = [ [ 22, 22 ], [ -22, 22 ], [ 0, 30 ], [ 0, -30 ], [ 22, -22 ], [ 30, 0 ], [ -30, 0 ], [ -22, -22 ] ]
      testData = []
      j = 0
    
      while j < timelineCount
        t =
          position: []
          scale: []
          alpha: []
          hue: []
          rotation: []
          srcRect: []
          srcPath: []
          center: []
          isDone: []
    
        testData.push t
        j++
      i = 0
    
      while i < frameCount
        j = 0
    
        while j < timelineCount
          if i < 4
            testData[j]["position"].push [ temp[j][0] * i / 4, temp[j][1] * i / 4 ]
          else
            testData[j]["position"].push [ temp[j][0], temp[j][1] ]
          testData[j]["scale"].push [ 1, 1 ]
          testData[j]["alpha"].push 1
          testData[j]["hue"].push [ 0, 0, 0 ]
          testData[j]["center"].push [ 0, 0 ]
          testData[j]["rotation"].push 0
          testData[j]["srcPath"].push null
          testData[j]["srcRect"].push null
          testData[j]["isDone"].push i is frameCount - 1
          j++
        i++
      i = 0
    
      while i < frameCount
        if i is 0
          interval.start()
        else
          interval.update()
        j = 0
    
        while j < timelineCount
          testFrame root, interval, testData, i, j
          j++
        i++
      j = 0
    
      while j < timelineCount
        expect(interval._intervals[j]._intervals[1]._interval).toBe null
        j++