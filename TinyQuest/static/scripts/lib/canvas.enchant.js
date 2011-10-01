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
        this._root.updateTransform(null);
        this._root.update();
        this._root.draw(this._game.assets, this._surface, 1);
    }
});

enchant.canvas.Node = enchant.Class.create({
    prioritySortFunc: function(a, b) {
        return a.priority - b.priority;
    },
    initialize: function(baseTransform) {
        this.position = [0, 0];
        this.velocity = [0, 0];
        this.size = [1, 1];
        this.scale = [1, 1];
        this.alpha = 1.0;
        this.priority = 0.5;
        this.blendType = null;
        this.hue = [0, 0, 0];
        this.rotation = 0;
        this.parent = null;
        this._children = [];
        this._transform = null;
        this._hasBaseTransform = false;
        
        if (baseTransform) {
            this._hasBaseTransform = true;
            this._transform = baseTransform;
        }
    },
    children: { 
        get: function() {
            return this._children;
        },
        set: function(children) {
            console.log("Sprite : children cannot be directly set!");
        }
    },
    transform: { 
        get: function() {
            // Initialize transform if it is not generated yet
            if (!this._transform) {
                if (this.parent) {
                    this.updateTransform(this.parent.transform);
                } else {
                    this.updateTransform(null);
                }
            }
            return this._transform;
        },
        set: function(transform) {
            this._transform = transform;
        }
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
        this._children.length = 0;
    },
    applyTransform: function(context) { 
        var t = this._transform;
        context.setTransform(t[0][0], t[0][1], t[1][0], t[1][1],t[2][0],t[2][1]);
    },
    sortByPriority: function() {
        this._children.sort(this.prioritySortFunc);
    },
    updateTransform: function(parentTransform) {
        if (this._hasBaseTransform) {
            // Calculate a matrix to calculate world velocity
            var matrix = enchant.matrix.createMatrixIdentity(3);
            matrix[0][0] = parentTransform[0][0];
            matrix[0][1] = parentTransform[0][1];
            matrix[1][0] = parentTransform[1][0];
            matrix[1][1] = parentTransform[1][1];
            var velocity = enchant.matrix.transformPoint(this.velocity, matrix);
            this._transform[2][0] +=  velocity[0];
            this._transform[2][1] +=  velocity[1];
        } else {
            var transform = enchant.matrix.getNodeTransformMatirx(this.position[0], this.position[1], this.rotation * Math.PI / 180, this.scale[0], this.scale[1]);       
            if (parentTransform) {
                transform = enchant.matrix.matrixMultiply(transform, parentTransform);
            }
            this._transform = transform;
        }
        
        for (var i = 0; i < this._children.length; i++) {
            this._children[i].updateTransform(this._transform);
        }
    },
    update: function() {
        this.sortByPriority();
        for (var i = 0; i < this._children.length; i++) {
            this._children[i].update();
        }
    },
    draw: function(assets, surface, alpha) {
        // If the node is not visible, simply skip all rendering for this
        if (alpha == 0 ) {return;}

        if (this._children.length > 0) {
            this.applyTransform(surface.context);
            for (var i = 0; i < this._children.length; i++) {
                this._children[i].draw(assets, surface, alpha * this.alpha);
            }
        }
    }
});

enchant.canvas.Sprite = enchant.Class.create({
    initialize: function(srcPath, srcRect) {
        this.node = new enchant.canvas.Node();
        this.srcPath = srcPath ? srcPath : null;
        this.srcRect = srcRect ? srcRect : null;
        this.center = [0, 0];
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
    size: {
        get: function() {
            return this.node.size;
        },
        set: function(value) {
            this.node.size = value;
        }
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
    blendType: {
        get: function() {
            return this.node.blendType;
        },
        set: function(value) {
            this.node.blendType = value;
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
    children: { 
        get: function() {
            return this.node.children;
        },
        set: function(children) {
            this.node.children = children;
        }
    },
    
    transform: { 
        get: function() {
            return this.node.transform;
        },
        set: function(transform) {
            this.node.transform = transform;
        }
    },
    applyTransform: function(context) { 
        this.node.applyTransform(context);
    },
    updateTransform: function(parentTransform) {
        if (this.srcPath) {
            var transform = enchant.matrix.getImageTransformMatirx(-this.position[0], -this.position[1], this.rotation * Math.PI / 180, this.scale[0], this.scale[1]);
            if (parentTransform) {
                transform = enchant.matrix.matrixMultiply(transform, parentTransform);
            }
            this.transform = transform;
            for (var i = 0; i < this.children.length; i++) {
                this.children[i].updateTransform(this.transform);
            }
        } else {
            this.node.updateTransform(parentTransform);
        }
    },
    update: function() {
        this.node.sortByPriority();
        if (this.srcPath) {
            for (var i = 0; i < this.children.length; i++) {
                this.children[i].update();
            }
        } else {
            this.node.update();
        }
    },
    draw: function(assets, surface, alpha) {
        // If the node is not visible, simply skip all rendering for this
        if (alpha == 0 ) {return;}

        if (this.srcPath) {
            var key = '../../static/assets/images/' + this.srcPath;
            var src = assets[key];
            assert(src !== undefined, "No file found at path = " + key);
            
            this.applyTransform(surface.context);
            surface.context.globalAlpha = alpha;
            
            if (this.blendType == "add") {
                surface.context.globalCompositeOperation = "lighter";
            } else { 
                surface.context.globalCompositeOperation = "source-over";
            }

            assert(typeof(this.srcRect[0]) == "number", "1");
            assert(typeof(this.srcRect[1]) == "number", "2");
            assert(typeof(this.srcRect[2]) == "number", "3");
            assert(typeof(this.srcRect[3]) == "number", "4");
            assert(typeof(this.position[0]) == "number", "5");
            assert(typeof(this.position[1]) == "number", "6");

            // Displaying floating point position can potentially blur the image on Canvas Element.
            // Here we are rounding floating point position into integer.
            var posX = Math.floor(this.position[0] - this.center[0] - this.srcRect[2] / 2);
            var posY = Math.floor(this.position[1] - this.center[1] - this.srcRect[3] / 2);
            // Hack: UV clips one extra pixel on the right, temporary workaround is here
            var uvCutX = 1;
            var uvCutY = 0;

            surface.draw(src, this.srcRect[0], this.srcRect[1], this.srcRect[2] - uvCutX, this.srcRect[3] - uvCutY, posX, posY, this.srcRect[2], this.srcRect[3]);
        } else {
            this.node.draw(assets, surface, alpha * this.node.alpha);
        }
    }
});