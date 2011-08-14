enchant('geom');

window.onload = function() {
    var game = new Game(640, 480);
    game.fps = 60;
    game.preload('static/map1.gif', 'static/chara0.gif');
    game.onload = function() {
        var rect = new Rectangle(0, 0, 640, 480);
        var stage = new Group();
        var s = new Sprite(640,480);

        s.image = new Surface(640,480);
        s.image.context.globalAlpha = 1;
        
        for (var i = 0; i < 1; i++) {
            s.image.context.beginPath();
            s.image.context.lineWidth = 1.0;
            s.image.context.moveTo(0.5, 120.5 + 20 * i);
            s.image.context.lineTo(640.5, 120.5 + 20 * i);
            s.image.context.stroke();
        }
        stage.addChild(s);
        game.rootScene.addChild(stage);
    };
    game.start();
};
