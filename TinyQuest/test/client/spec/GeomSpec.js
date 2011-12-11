
  describe("Geom", function() {
    var point, rectangle, size;
    point = new roga.geom.Point(30, -226);
    describe("Rectangle(30, -226)", function() {
      return it("property check", function() {
        expect(point.x).toBe(30);
        return expect(point.y).toBe(-226);
      });
    });
    size = new roga.geom.Size(20, 36);
    describe("Size(20, 36)", function() {
      return it("property check", function() {
        expect(size.width).toBe(20);
        return expect(size.height).toBe(36);
      });
    });
    rectangle = new roga.geom.Rectangle(10, 20, 240, 320);
    return describe("Rectangle(10, 20, 240, 320)", function() {
      return it("property check", function() {
        expect(rectangle.x).toBe(10);
        expect(rectangle.y).toBe(20);
        expect(rectangle.width).toBe(240);
        expect(rectangle.height).toBe(320);
        expect(rectangle.getLeft()).toBe(10);
        expect(rectangle.getTop()).toBe(20);
        expect(rectangle.getRight()).toBe(250);
        return expect(rectangle.getBottom()).toBe(340);
      });
    });
  });
