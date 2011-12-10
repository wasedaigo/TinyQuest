describe "Geom", ->
  point = new enchant.geom.Point(30, -226)
  describe "Rectangle(30, -226)", ->
    it "property check", ->
      expect(point.x).toBe 30
      expect(point.y).toBe -226

  size = new enchant.geom.Size(20, 36)
  describe "Size(20, 36)", ->
    it "property check", ->
      expect(size.width).toBe 20
      expect(size.height).toBe 36

  rectangle = new enchant.geom.Rectangle(10, 20, 240, 320)
  describe "Rectangle(10, 20, 240, 320)", ->
    it "property check", ->
      expect(rectangle.x).toBe 10
      expect(rectangle.y).toBe 20
      expect(rectangle.width).toBe 240
      expect(rectangle.height).toBe 320
      expect(rectangle.right).toBe 250
      expect(rectangle.bottom).toBe 340