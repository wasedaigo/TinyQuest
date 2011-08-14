enchant();

window.onload = function() {
    var game = new Game(640, 480);
    game.fps = 60;
    game.preload('../../static/images/bg/bg0001.png');
    game.onload = function() {
        var stage = new Group();

        var surface = new Surface(640,480);
        var sceneGraph = new enchant.canvas.SceneGraph(game, surface);
        var glSprite = new enchant.canvas.Sprite(10, 20);
        sceneGraph.addChild(glSprite);

        var s = new enchant.Sprite(640,480);
        s.image = surface
        stage.addChild(s);
        game.rootScene.addChild(stage);

        // Update
        this.addEventListener('enterframe', function() {
            sceneGraph.update();
        });
    };
    game.start();
};
