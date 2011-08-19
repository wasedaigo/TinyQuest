enchant();

var currentAnimation = null;
var game = new Game(640, 480);
window.onload = function() {
    game.fps = 60;
    game.preload('../../static/assets/images/bg/bg0001.png');
    game.onload = function() {
        var stage = new Group();

        var surface = new Surface(640,480);
        var sceneGraph = new enchant.canvas.SceneGraph(game, surface);
        var glSprite = new enchant.canvas.Sprite("bg/bg0001.png", new enchant.geom.Size(32, 32), new enchant.geom.Rectangle(0, 0, 32, 32));
        sceneGraph.setRoot(glSprite);

        var s = new enchant.Sprite(640,480);
        s.image = surface
        stage.addChild(s);
        game.rootScene.addChild(stage);

        // Update
        this.addEventListener('enterframe', function() {
            surface.context.clearRect(0, 0,640, 480);
            if (currentAnimation) {
                sceneGraph.update();
                glSprite.angle = 45;
                glSprite.scale = new enchant.geom.Point(2, 2);
            }
        });
    };
    game.pause();
};

var playAnimation = function(data) {
    currentAnimation = data;
    game.start();
};
