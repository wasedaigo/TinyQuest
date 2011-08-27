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
        this.position = [0, 0];
        this.scale = [1, 1];
        this.alpha = 1.0;
        this.priority = 0.5;
        this.hue = [0, 0, 0];
        this.rotation = 0;
        this.parent = null;

        this._children = [];
    },

    addChild: function(node) {
        this._children.push(node);
        node.parent = this;
    },

    removeChild: function(node) {
        for(i = 0; i < this._children.length; i++){
            if(this._children[i] == node){
                node.parent = null;
                this._children.splice(i,1);
                break;
            }
        }
    },

    removeAllChilden: function(node) {
        for(i = 0; i < this._children.length; i++){
            this._children[i].parent = null;
        }
        this._children = [];
    },
    
    update: function(context) { 
    },

    draw: function(assets, surface) {
        if (this._children.length > 0) {
            surface.context.save();
            surface.context.translate(this.pos[0], this.pos[1]);
            surface.context.rotate(this.angle * Math.PI / 180);
            surface.context.scale(this.scale[0], this.scale[1]);
            for (var key in this._children) {
                var node = this._children[key];
                node.draw(assets, surface);
            }
            surface.context.restore();
        }
    }
});

enchant.canvas.Sprite = enchant.Class.create(enchant.canvas.Node, {
    initialize: function(srcPath, srcRect) {
        this.super = enchant.canvas.Node.call(this);

        this.srcPath = srcPath ? srcPath : null;
        this.srcRect = srcRect ? srcRect : null;

        this._center = [0, 0];
        if (this.srcRect) {
            this._anchor = new enchant.geom.Point(0.5, 0.5);
            this._centerX = this._anchor.x * this.srcRect[2];
            this._centerY = this._anchor.y * this.srcRect[3];
        }
        this._children = [];
    },

    update: function(context) { 
    },

    draw: function(assets, surface) {
        this.super = enchant.canvas.Node.draw(this);

        var key = '../../static/assets/images/' + this.srcPath;
        var src = assets[key];
        assert(src);
        var radian = this.angle * Math.PI / 180;

        surface.context.save();
        surface.context.translate(this._centerX, this._centerY);
        surface.context.rotate(this.angle * Math.PI / 180);
        surface.context.translate(-this._centerX, -this._centerY);
        surface.context.scale(this.scale.x, this.scale.y);
        surface.draw(src, this.srcRect[0], this.srcRect[1], this.srcRect[2], this.srcRect[3], this.x, this.y, this.srcRect[2], this.srcRect[3]);
        surface.context.restore();
    }
});