enchant();

var currentAnimation = null;
var game = new enchant.Game(640, 480);
window.onload = function() {
    game.fps = 60;
    game.preload('../../static/assets/images/bg/bg0001.png');
    game.onload = function() {
        var stage = new Group();

        var surface = new Surface(640,480);
        var sceneGraph = new enchant.canvas.SceneGraph(game, surface);
        var glSprite = new enchant.canvas.Sprite("bg/bg0001.png", [0, 0, 32, 32]);
        sceneGraph.setRoot(glSprite);

        var s = new enchant.Sprite(640,480);
        s.image = surface
        stage.addChild(s);
        game.rootScene.addChild(stage);

        // Update
        this.addEventListener('enterframe', function() {
            surface.context.clearRect(0, 0,640, 480);
            //if (currentAnimation) {
                sceneGraph.update();
                glSprite.rotation += 1;
                glSprite.scale = [1, 1];
            //}
        });
    };
    game.pause();
};

var playAnimation = function(data) {
    currentAnimation = data;
    
    enchant.loader.load([], function() {
        enchant.Game.instance.start();
    });
    
};
