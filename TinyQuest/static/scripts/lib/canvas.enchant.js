enchant.canvas = {};

enchant.canvas.SceneGraph = enchant.Class.create({
    initialize: function(game, surface) {
        assert(surface);
        this._game = game;
        this._surface = surface;
        this._children = [];
    },

    addChild: function(node) {
        this._children.push(node);
    },
    
    removeChild: function(node) {
        for(i = 0; i < this._children.length; i++){
            if(this._children[i] == node){
                this._children.splice(i,1);
                break;
            }
        }
    },

    update: function() {
       for (var key in this._children) {
            var node = this._children[key];
            node.draw(this._game.assets, this._surface);
       }
    }
});

enchant.canvas.AnimationNode = enchant.Class.create({
    initialize: function(animationData) {
        this.rootSprite = null; 
    }
});

enchant.canvas.Sprite = enchant.Class.create({
    initialize: function(width, height) {
        this.rect = new enchant.geom.Rectangle(0, 0, width, height);
        this.anchor = new enchant.geom.Point(0, 0);
        this.scale = new enchant.geom.Point(1, 1);
        this.alpha = 0.0;
        this.priority = 0.5;
        this.hueR = 0;
        this.hueG = 0;
        this.hueB = 0;
        this.angle = 0.0;
        this.children = [];
    },

    update: function(context) {
        
    },

    draw: function(assets, surface) {
        var src = assets['../../static/images/bg/bg0001.png'];
        surface.draw(src);
    }
});