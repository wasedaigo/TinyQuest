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

    removeAllChildren: function() {
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
            surface.context.translate(this.position[0], this.position[1]);
            surface.context.rotate(this.rotation * Math.PI / 180);
            surface.context.scale(this.scale[0], this.scale[1]);
            for (var key in this._children) {
                var node = this._children[key];
                node.draw(assets, surface);
            }
            surface.context.restore();
        }
    }
});

enchant.canvas.Sprite = enchant.Class.create({
    initialize: function(srcPath, srcRect) {
        this.node = new enchant.canvas.Node();
        this.srcPath = srcPath ? srcPath : null;
        this.srcRect = srcRect ? srcRect : null;

        this.center = [0, 0];
        if (this.srcRect) {
            this._anchor = [0.5, 0.5];
            this.center = [this._anchor[0] * this.srcRect[2], this._anchor[1] * this.srcRect[3]];
        }
    },

    addChild: function(node) {
        this.node.addChild(node);
    },

    removeChild: function(node) {
        this.node.removeChild(node);
    },

    removeAllChildren: function() {
        this.node.removeAllChildren();
    },
    
    update: function(context) { 
    },
    position: {
        get: function() {
            return this.node.position;
        },
        set: function(value) {
            this.node.position = value;
        }
    },   
    alpha: {
        get: function() {
            return this.node.alpha;
        },
        set: function(value) {
            this.node.alpha = value;
        }
    },    
    priority: {
        get: function() {
            return this.node.priority;
        },
        set: function(value) {
            this.node.priority = value;
        }
    },
    hue: {
        get: function() {
            return this.node.hue;
        },
        set: function(value) {
            this.node.hue = value;
        }
    }, 
    parent: {
        get: function() {
            return this.node.parent;
        },
        set: function(value) {
            this.node.parent = value;
        }
    },  
    rotation: {
        get: function() {
            return this.node.rotation;
        },
        set: function(value) {
            return this.node.rotation = value;
        }
    },
    scale: {
        get: function() {
            return this.node.scale;
        },
        set: function(value) {
            this.node.scale = value;
        }
    },
    draw: function(assets, surface) {

        if (this.srcPath) {
            var key = '../../static/assets/images/' + this.srcPath;
            var src = assets[key];
            surface.context.save();
            var dx = this.position[0] + this.srcRect[2] / 2;
            var dy = this.position[1] + this.srcRect[3] / 2;
            surface.context.translate(dx, dy);
            surface.context.rotate(this.rotation * Math.PI / 180);
            surface.context.scale(this.scale[0], this.scale[1]);
            surface.context.translate(-dx, -dy);
            
            surface.context.globalAlpha = this.alpha;

            assert(typeof(this.srcRect[0]) == "number", "1");
            assert(typeof(this.srcRect[1]) == "number", "2");
            assert(typeof(this.srcRect[2]) == "number", "3");
            assert(typeof(this.srcRect[3]) == "number", "4");
            assert(typeof(this.position[0]) == "number", "5");
            assert(typeof(this.position[1]) == "number", "6");
            
            surface.draw(src, this.srcRect[0], this.srcRect[1], this.srcRect[2], this.srcRect[3], this.position[0] - this.center[0], this.position[1] - this.center[1], this.srcRect[2], this.srcRect[3]);
            surface.context.restore();
        } else {
            this.node.draw(assets, surface);
        }
    }
});