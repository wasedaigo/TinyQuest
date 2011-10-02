enchant();

var GAME_WIDTH = 640;
var GAME_HEIGHT = 640;
var game = new enchant.Game(GAME_WIDTH, GAME_HEIGHT);
enchant.loader.setRootPath('../../static/assets');
var currentAnimationFileName = null;
var currentAnimation = null;
var target;
var root = new enchant.canvas.Node();
window.onload = function() {
    game.fps = 40;

    game.preload('../../static/assets/images/Images/Enemies/death_wind.png');
    game.preload('../../static/assets/images/bg/bg0001.png');
    game.onload = function() {


        var stage = new Group();

        var surface = new Surface(GAME_WIDTH,GAME_HEIGHT);
        var sceneGraph = new enchant.canvas.SceneGraph(game, surface);

        root.position = [0, 0];
        root.scale = [2, 2];
        sceneGraph.setRoot(root);

        var s = new enchant.Sprite(GAME_WIDTH,GAME_HEIGHT);
        s.image = surface
        stage.addChild(s);
        game.rootScene.addChild(stage);
            target = new enchant.canvas.Sprite();
            target.srcPath = 'Images/Enemies/death_wind.png';
            target.srcRect = [0, 0, 90, 92];
            target.size = [90, 92];
            target.position = [60, GAME_HEIGHT / 4 - 60];
        var moveVelocityX = 1;
        var moveVelocityY = 1;
        enchant.animation.animationManager.initialize(root);
        // Update
        this.addEventListener('enterframe', function() {
            
            surface.context.setTransform(1, 0, 0, 1, 0, 0);
            surface.context.fillStyle = 'rgba(120,120,120,1)';
            surface.context.fillRect(0, 0,GAME_WIDTH, GAME_HEIGHT);
            s.image.context.beginPath();
            s.image.context.lineWidth = 1.0;
            s.image.context.moveTo(0, GAME_HEIGHT / 2 - 0.5);
            s.image.context.lineTo(GAME_WIDTH, GAME_HEIGHT / 2 - 0.5);
            s.image.context.stroke();

            s.image.context.beginPath();
            s.image.context.lineWidth = 1.0;
            s.image.context.moveTo(GAME_WIDTH / 2 - 0.5, 0);
            s.image.context.lineTo(GAME_WIDTH / 2 - 0.5, GAME_HEIGHT);
            s.image.context.stroke();

            if (currentAnimationFileName) {
                if (target) {
                    if (target.position[0] >= 120) {
                        moveVelocityX *= -1;
                    }
                    
                    if (target.position[0] <= 40) {
                        moveVelocityX *= -1;
                    }
                    
                    if (target.position[1] >= 240) {
                        moveVelocityY *= -1;
                    }
                    
                    if (target.position[1] <= 60) {
                        moveVelocityY *= -1;
                    }
                    target.position[0] += moveVelocityX;
                    target.position[1] += moveVelocityY;
                }
                
                enchant.animation.animationManager.update();
                sceneGraph.update();
            }
            surface.context.globalCompositeOperation = "source-over";
            surface.context.globalAlpha = 1;
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
            

            root.addChild(target);

            var animation = enchant.animation.animationManager.CreateAnimation(enchant.loader.get(currentAnimationFileName), false, null, 1, {"node":target, "origin":enchant.utils.clone(target.position)});
            animation.node.position = [GAME_WIDTH / 4, GAME_HEIGHT / 4];
            enchant.animation.animationManager.start(animation); 
        });
    });
    
};
