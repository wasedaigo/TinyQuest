describe("canvas.Sprite", function() {
    var surface = new enchant.Surface(640,480);
    var sprite = new enchant.canvas.Sprite(10, 20);
    describe("Sprite(10, 20)", function() {
        it("property check", function() {
          expect(sprite.rect.width).toBe(10);
          expect(sprite.rect.height).toBe(20);
        });
    });

    describe("SceneGraph children", function() {
        var sceneGraph = new enchant.canvas.SceneGraph(surface);
        it("addChild()", function() {
            sceneGraph.addChild(sprite);
            expect(sceneGraph._children.length).toBe(1);
        });
        it("removeChild()", function() {
            sceneGraph.removeChild(sprite);
            expect(sceneGraph._children.length).toBe(0);
        });
    });
});
