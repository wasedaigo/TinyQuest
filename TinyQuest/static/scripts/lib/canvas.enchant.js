enchant.canvas = {};

enchant.canvas.SceneGraph = enchant.Class.create({
    initialize: function(game, surface) {
        assert(surface);
        this._game = game;
        this._surface = surface;
        this._root = null;
    },

    setRoot: function(node) {
        this._root = node;
    },

    update: function() {
        assert(this._root);
        this._root.draw(this._game.assets, this._surface);
    }
});

enchant.canvas.Node = enchant.Class.create({
    initialize: function() {
        this.x = 0;
        this.y = 0;
        this.scale = new enchant.geom.Point(1, 1);
        this.alpha = 0.0;
        this.priority = 0.5;
        this.hueR = 0;
        this.hueG = 0;
        this.hueB = 0;
        this.angle = 0.0;

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

    update: function(context) { 
    },

    draw: function(assets, surface) {
        surface.context.save();
        surface.context.translate(this.x, this.y);
        surface.context.rotate(this.angle * Math.PI / 180);
        surface.context.scale(this.scale.x, this.scale.y);
        for (var key in this._children) {
            var node = this._children[key];
            node.draw(assets, surface);
        }
        surface.context.restore();
    }
});

enchant.canvas.Sprite = enchant.Class.create(enchant.canvas.Node, {
    initialize: function(filename, size, srcRect) {
        this.super = Node.call(this);

        this._filename = filename;
        this.size = size;

        this._srcRect = srcRect;
        this._anchor = new enchant.geom.Point(0.5, 0.5);
        this._centerX = this._anchor.x * this.size.width;
        this._centerY = this._anchor.y * this.size.height;
        this._children = [];
    },

    update: function(context) { 
    },

    draw: function(assets, surface) {
        var key = '../../static/assets/images/' + this._filename;
        var src = assets[key];
        assert(src);
        var radian = this.angle * Math.PI / 180;

        surface.context.save();
        surface.context.translate(this._centerX, this._centerY);
        surface.context.rotate(this.angle * Math.PI / 180);
        surface.context.translate(-this._centerX, -this._centerY);
        surface.context.scale(this.scale.x, this.scale.y);
        surface.draw(src, this._srcRect.x, this._srcRect.y, this._srcRect.width, this._srcRect.height, this.x, this.y, this.size.width, this.size.height);
        surface.context.restore();
    }
});