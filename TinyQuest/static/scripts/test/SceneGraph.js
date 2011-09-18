enchant();


var game = new enchant.Game(320, 320);
enchant.loader.setRootPath('../../static/assets');
var currentAnimationFileName = null;
var currentAnimation = null;
var root = new enchant.canvas.Node();
window.onload = function() {
    game.fps = 45;

    game.preload('../../static/assets/images/bg/bg0001.png');
    game.onload = function() {
        var stage = new Group();

        var surface = new Surface(320,320);
        var sceneGraph = new enchant.canvas.SceneGraph(game, surface);
        
        root.position = [160, 160];
        root.scale = [2, 2];
        sceneGraph.setRoot(root);

        var s = new enchant.Sprite(320,320);
        s.image = surface
        stage.addChild(s);
        game.rootScene.addChild(stage);

        enchant.animation.animationManager.initialize(root);
        // Update
        this.addEventListener('enterframe', function() {
            
            surface.context.setTransform(1, 0, 0, 1, 0, 0);
            surface.context.clearRect(0, 0,320, 320);
            
            s.image.context.beginPath();
            s.image.context.lineWidth = 1.0;
            s.image.context.moveTo(0, 159.5);
            s.image.context.lineTo(320, 159.5);
            s.image.context.stroke();

            s.image.context.beginPath();
            s.image.context.lineWidth = 1.0;
            s.image.context.moveTo(159.5, 0);
            s.image.context.lineTo(159.5, 320);
            s.image.context.stroke();

            if (currentAnimationFileName) {
                sceneGraph.update();
                enchant.animation.animationManager.update();
            }
        });
    };
    enchant.Game.instance.start();
};

var playAnimation = function(id) {
    var filename = '../../static/assets/animations/' + id + ".json"
    currentAnimationFileName = null;
    currentAnimation = null;
    // Load animation file
    enchant.loader.load([filename], function() {
        json = enchant.loader.get(filename);
        var files = [];
        
        // List all animations to be loaded
        var animations = json.dependencies.animations;
        for (var i = 0; i < animations.length; i++) {
            files.push('../../static/assets/animations/' + animations[i] + ".json");
        }
        // List all images to be loaded
        var images = json.dependencies.images;
        for (var i = 0; i < images.length; i++) {
            files.push('../../static/assets/images/' + images[i] + ".png");
        }
        currentAnimationFileName = filename;

        enchant.loader.load(files, function() {
            interval = null;
            root.removeAllChildren();
            var animation = enchant.animation.animationManager.CreateAnimation(enchant.loader.get(currentAnimationFileName));
            enchant.animation.animationManager.start(animation); 
        });
    });
    
};
