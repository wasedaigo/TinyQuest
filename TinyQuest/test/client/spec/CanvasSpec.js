
  describe("canvas", function() {
    var surface;
    surface = new enchant.Surface(640, 480);
    describe("Sprite", function() {
      it("property check", function() {
        var sprite;
        sprite = new enchant.canvas.Sprite("", [5, 14, 25, 30]);
        expect(sprite.srcRect[0]).toBe(5);
        expect(sprite.srcRect[1]).toBe(14);
        expect(sprite.srcRect[2]).toBe(25);
        return expect(sprite.srcRect[3]).toBe(30);
      });
      it("setter/geter check", function() {
        var sprite;
        sprite = new enchant.canvas.Sprite("", [5, 14, 25, 30]);
        sprite.position = [5, 2];
        sprite.scale = [3, 2];
        sprite.hue = [2, 3, 2];
        sprite.alpha = 0.4;
        sprite.priority = 0.3;
        sprite.rotation = 122;
        sprite.parent = 0;
        sprite.srcRect = [5, 2, 3, 2];
        sprite.srcPath = "tes";
        sprite.size = [3, 5];
        expect(sprite.position).toEqual([5, 2]);
        expect(sprite.scale).toEqual([3, 2]);
        expect(sprite.hue).toEqual([2, 3, 2]);
        expect(sprite.alpha).toEqual(0.4);
        expect(sprite.priority).toEqual(0.3);
        expect(sprite.rotation).toEqual(122);
        expect(sprite.parent).toEqual(0);
        expect(sprite.srcRect).toEqual([5, 2, 3, 2]);
        expect(sprite.srcPath).toEqual("tes");
        return expect(sprite.size).toEqual([3, 5]);
      });
      return it("positionType check", function() {
        var sprite;
        sprite = new enchant.canvas.Sprite("", [5, 14, 25, 30]);
        sprite.size = [30, 50];
        sprite.position = [0, 10];
        sprite.scale = [2, 3];
        sprite.center = [-5, 5];
        expect(sprite.getOffsetByPositionAnchor([-1, 1])).toEqual([-40, 15]);
        expect(sprite.getOffsetByPositionAnchor([0, 1])).toEqual([-10, 15]);
        expect(sprite.getOffsetByPositionAnchor([1, 1])).toEqual([20, 15]);
        expect(sprite.getOffsetByPositionAnchor([-1, 0])).toEqual([-40, -60]);
        expect(sprite.getOffsetByPositionAnchor([0, 0])).toEqual([-10, -60]);
        expect(sprite.getOffsetByPositionAnchor([1, 0])).toEqual([20, -60]);
        expect(sprite.getOffsetByPositionAnchor([-1, -1])).toEqual([-40, -135]);
        expect(sprite.getOffsetByPositionAnchor([0, -1])).toEqual([-10, -135]);
        return expect(sprite.getOffsetByPositionAnchor([1, -1])).toEqual([20, -135]);
      });
    });
    describe("SceneGraph children", function() {
      var sceneGraph, sprite;
      sprite = new enchant.canvas.Sprite("", [5, 14, 25, 30]);
      sceneGraph = new enchant.canvas.SceneGraph(game, surface);
      return it("addChild()", function() {
        sceneGraph.setRoot(sprite);
        return expect(sceneGraph._root).toBe(sprite);
      });
    });
    return describe("Node children", function() {
      var node1, node2;
      node1 = new enchant.canvas.Node();
      node2 = new enchant.canvas.Node();
      it("addChild()", function() {
        node1.addChild(node2);
        expect(node2.parent).toBe(node1);
        return expect(node1._children.length).toBe(1);
      });
      return it("removeChild()", function() {
        node1.removeChild(node2);
        expect(node2.parent).toBe(null);
        return expect(node1._children.length).toBe(0);
      });
    });
  });
