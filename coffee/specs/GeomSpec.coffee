describe "Geom", ->
  point = new roga.geom.Point(30, -226)
  describe "Rectangle(30, -226)", ->
    it "property check", ->
      expect(point.x).toBe 30
      expect(point.y).toBe -226

  size = new roga.geom.Size(20, 36)
  describe "Size(20, 36)", ->
    it "property check", ->
      expect(size.width).toBe 20
      expect(size.height).toBe 36

  rectangle = new roga.geom.Rectangle(10, 20, 240, 320)
  describe "Rectangle(10, 20, 240, 320)", ->
    it "property check", ->
      expect(rectangle.x).toBe 10
      expect(rectangle.y).toBe 20
      expect(rectangle.width).toBe 240
      expect(rectangle.height).toBe 320
      expect(rectangle.getLeft()).toBe 10
      expect(rectangle.getTop()).toBe 20
      expect(rectangle.getRight()).toBe 250
      expect(rectangle.getBottom()).toBe 340