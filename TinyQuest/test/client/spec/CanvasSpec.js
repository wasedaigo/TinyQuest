describe("canvas", function() {
    var surface = new enchant.Surface(640,480);
    var sprite = new enchant.canvas.Sprite("", [10, 20], [5, 14, 25, 30]);
    describe("Sprite", function() {
        it("property check", function() {
          expect(sprite.size[0]).toBe(10);
          expect(sprite.size[1]).toBe(20);
          expect(sprite.srcRect[0]).toBe(5);
          expect(sprite.srcRect[1]).toBe(14);
          expect(sprite.srcRect[2]).toBe(25);
          expect(sprite.srcRect[3]).toBe(30);
        });
    });

    describe("SceneGraph children", function() {
        var sceneGraph = new enchant.canvas.SceneGraph(game, surface);
        it("addChild()", function() {
            sceneGraph.setRoot(sprite);
            expect(sceneGraph._root).toBe(sprite);
        });
    });

    describe("Node children", function() {
        var node1 = new enchant.canvas.Node();
        var node2 = new enchant.canvas.Node();
        it("addChild()", function() {
            node1.addChild(node2);
            expect(node1._children.length).toBe(1);
        });
        it("removeChild()", function() {
            node1.removeChild(node2);
            expect(node1._children.length).toBe(0);
        });
    });
    
});
