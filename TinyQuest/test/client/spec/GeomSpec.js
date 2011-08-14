describe("Player", function() {

  var point = new enchant.geom.Point(30, -226);
  describe("Rectangle(30, -226)", function() {
    it("property check", function() {
      expect(point.x).toBe(30);
      expect(point.y).toBe(-226);
    });
  });
  
  var size = new enchant.geom.Size(20, 36);
  describe("Size(20, 36)", function() {
    it("property check", function() {
      expect(size.width).toBe(20);
      expect(size.height).toBe(36);
    });
  });
  
  var rectangle = new enchant.geom.Rectangle(10, 20, 240, 320);
  describe("Rectangle(10, 20, 240, 320)", function() {
    it("property check", function() {
      expect(rectangle.x).toBe(10);
      expect(rectangle.y).toBe(20);
      expect(rectangle.width).toBe(240);
      expect(rectangle.height).toBe(320);
      expect(rectangle.right).toBe(250);
      expect(rectangle.bottom).toBe(340);
    });
  });
});