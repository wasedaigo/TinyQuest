(function() {
  var GAME_HEIGHT, GAME_WIDTH, currentAnimation, currentAnimationFileName, game, root, target;

  enchant();

  GAME_WIDTH = parseInt($("#enchant-stage").width());

  GAME_HEIGHT = parseInt($("#enchant-stage").height());

  game = new enchant.Game(GAME_WIDTH, GAME_HEIGHT);

  enchant.loader.setRootPath("../../static/assets");

  currentAnimationFileName = null;

  currentAnimation = null;

  target = null;

  root = new enchant.canvas.Node();

  window.onload = function() {
    game.fps = 40;
    game.preload("../../static/assets/images/Images/Enemies/death_wind.png");
    game.preload("../../static/assets/images/bg/bg0001.png");
    game.onload = function() {
      var moveVelocityX, moveVelocityY, s, sceneGraph, stage, surface;
      stage = new Group();
      surface = new Surface(GAME_WIDTH, GAME_HEIGHT);
      sceneGraph = new enchant.canvas.SceneGraph(game, surface);
      root.position = [0, 0];
      root.scale = [scale, scale];
      sceneGraph.setRoot(root);
      s = new enchant.Sprite(GAME_WIDTH, GAME_HEIGHT);
      s.image = surface;
      stage.addChild(s);
      game.rootScene.addChild(stage);
      target = new enchant.canvas.Sprite();
      target.srcPath = "Images/Enemies/death_wind.png";
      target.srcRect = [0, 0, 90, 92];
      target.size = [90, 92];
      target.position = [GAME_WIDTH / (4 * root.scale[0]), GAME_HEIGHT / (2 * root.scale[1])];
      target.origin = [GAME_WIDTH / (4 * root.scale[0]), GAME_HEIGHT / (2 * root.scale[1])];
      target.priority = 0.49;
      moveVelocityX = 1;
      moveVelocityY = 1;
      enchant.animation.animationManager.initialize(root);
      surface.context.fillStyle = "rgba(120,120,120,1)";
      return this.addEventListener("enterframe", function() {
        surface.context.setTransform(1, 0, 0, 1, 0, 0);
        surface.context.fillRect(0, 0, GAME_WIDTH, GAME_HEIGHT);
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
            if (target.position[0] >= GAME_WIDTH / (3 * root.scale[0])) {
              moveVelocityX *= -1;
            }
            if (target.position[0] <= GAME_HEIGHT / (8 * root.scale[0])) {
              moveVelocityX *= -1;
            }
            if (target.position[1] >= GAME_HEIGHT / (2 * root.scale[1])) {
              moveVelocityY *= -1;
            }
            if (target.position[1] <= GAME_HEIGHT / (8 * root.scale[1])) {
              moveVelocityY *= -1;
            }
            target.position[0] += moveVelocityX;
            target.position[1] += moveVelocityY;
          }
          enchant.animation.animationManager.update();
          sceneGraph.update();
        }
        surface.context.globalCompositeOperation = "source-over";
        return surface.context.globalAlpha = 1;
      });
    };
    return enchant.Game.instance.start();
  };

  window.playAnimation = function(id) {
    var filename;
    filename = "../../static/assets/animations/" + id + ".json";
    currentAnimationFileName = null;
    currentAnimation = null;
    return enchant.loader.load([filename], function() {
      var animations, files, i, images, json;
      json = enchant.loader.get(filename);
      files = [];
      animations = json.dependencies.animations;
      i = 0;
      while (i < animations.length) {
        files.push("../../static/assets/animations/" + animations[i] + ".json");
        i++;
      }
      images = json.dependencies.images;
      i = 0;
      while (i < images.length) {
        files.push("../../static/assets/images/" + images[i] + ".png");
        i++;
      }
      currentAnimationFileName = filename;
      return enchant.loader.load(files, function() {
        var animation, interval;
        interval = null;
        root.removeAllChildren();
        root.addChild(target);
        animation = enchant.animation.animationManager.CreateAnimation(enchant.loader.get(currentAnimationFileName), false, null, 1, 0.5, {
          node: target,
          origin: enchant.utils.clone(target.origin)
        });
        animation.node.position = [GAME_WIDTH / (2 * root.scale[0]), GAME_HEIGHT / (2 * root.scale[1])];
        return enchant.animation.animationManager.start(animation);
      });
    });
  };

}).call(this);
