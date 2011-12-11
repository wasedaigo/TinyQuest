describe "canvas", ->
  surface = new enchant.Surface(640, 480)
  describe "Sprite", ->
    it "property check", ->
      sprite = new roga.canvas.Sprite("", [ 5, 14, 25, 30 ])
      expect(sprite.getSrcRect()[0]).toBe 5
      expect(sprite.getSrcRect()[1]).toBe 14
      expect(sprite.getSrcRect()[2]).toBe 25
      expect(sprite.getSrcRect()[3]).toBe 30

    it "setter/geter check", ->
      sprite = new roga.canvas.Sprite("", [ 5, 14, 25, 30 ])
      sprite.setPosition [ 5, 2 ]
      sprite.setScale [ 3, 2 ]
      sprite.setHue [ 2, 3, 2 ]
      sprite.setAlpha 0.4
      sprite.setPriority 0.3
      sprite.setRotation 122
      sprite.setParent 0
      sprite.setSrcRect [ 5, 2, 3, 2 ]
      sprite.setSrcPath "tes"
      sprite.setSize [ 3, 5 ]
      expect(sprite.getPosition()).toEqual [ 5, 2 ]
      expect(sprite.getScale()).toEqual [ 3, 2 ]
      expect(sprite.getHue()).toEqual [ 2, 3, 2 ]
      expect(sprite.getAlpha()).toEqual 0.4
      expect(sprite.getPriority()).toEqual 0.3
      expect(sprite.getRotation()).toEqual 122
      expect(sprite.getParent()).toEqual 0
      expect(sprite.getSrcRect()).toEqual [ 5, 2, 3, 2 ]
      expect(sprite.getSrcPath()).toEqual "tes"
      expect(sprite.getSize()).toEqual [ 3, 5 ]

    it "positionType check", ->
      sprite = new roga.canvas.Sprite("", [ 5, 14, 25, 30 ])
      sprite.setSize [ 30, 50 ]
      sprite.setPosition [ 0, 10 ]
      sprite.setScale [ 2, 3 ]
      sprite.setCenter [ -5, 5 ]
      expect(sprite.getOffsetByPositionAnchor([ -1, 1 ])).toEqual [ -40, 15 ]
      expect(sprite.getOffsetByPositionAnchor([ 0, 1 ])).toEqual [ -10, 15 ]
      expect(sprite.getOffsetByPositionAnchor([ 1, 1 ])).toEqual [ 20, 15 ]
      expect(sprite.getOffsetByPositionAnchor([ -1, 0 ])).toEqual [ -40, -60 ]
      expect(sprite.getOffsetByPositionAnchor([ 0, 0 ])).toEqual [ -10, -60 ]
      expect(sprite.getOffsetByPositionAnchor([ 1, 0 ])).toEqual [ 20, -60 ]
      expect(sprite.getOffsetByPositionAnchor([ -1, -1 ])).toEqual [ -40, -135 ]
      expect(sprite.getOffsetByPositionAnchor([ 0, -1 ])).toEqual [ -10, -135 ]
      expect(sprite.getOffsetByPositionAnchor([ 1, -1 ])).toEqual [ 20, -135 ]

  describe "Stage children", ->
    sprite = new roga.canvas.Sprite("", [ 5, 14, 25, 30 ])
    Stage = new roga.canvas.Stage(game, surface)
    it "addChild()", ->
        Stage.setRoot sprite
        expect(Stage._root).toBe sprite

  describe "Node children", ->
    node1 = new roga.canvas.Node()
    node2 = new roga.canvas.Node()
    it "addChild()", ->
      node1.addChild node2
      expect(node2.getParent()).toBe node1
      expect(node1._children.length).toBe 1

    it "removeChild()", ->
      node1.removeChild node2
      expect(node2.getParent()).toBe null
      expect(node1._children.length).toBe 0