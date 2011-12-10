
  enchant.canvas = {};

  enchant.canvas.SceneGraph = enchant.Class.create({
    initialize: function(game, surface) {
      assert(surface);
      this._game = game;
      this._surface = surface;
      return this._root = null;
    },
    setRoot: function(node) {
      return this._root = node;
    },
    prioritySortFunc: function(a, b) {
      return a.absPriority - b.absPriority;
    },
    update: function() {
      var drawCommmands, i, _results;
      assert(this._root);
      this._root.updateTransform();
      this._root.updateAttributes();
      drawCommmands = [];
      this._root.registerDrawCommand(drawCommmands);
      drawCommmands.sort(this.prioritySortFunc);
      i = 0;
      _results = [];
      while (i < drawCommmands.length) {
        drawCommmands[i].draw(this._game.assets, this._surface);
        _results.push(i++);
      }
      return _results;
    }
  });

  enchant.canvas.Node = enchant.Class.create({
    initialize: function(baseTransform) {
      this.position = [0, 0];
      this.velocity = [0, 0];
      this.size = [1, 1];
      this.scale = [1, 1];
      this.alpha = 1.0;
      this.absAlpha = 1.0;
      this.depth = 0;
      this.priority = 0.5;
      this.absPriority = 0.5;
      this.blendType = null;
      this.hue = [0, 0, 0];
      this.rotation = 0;
      this.parent = null;
      this._children = [];
      this._transform = null;
      this._hasBaseTransform = false;
      this.center = [0, 0];
      if (baseTransform) {
        this._hasBaseTransform = true;
        return this._transform = baseTransform;
      }
    },
    children: {
      get: function() {
        return this._children;
      },
      set: function(children) {
        return console.log("Sprite : children cannot be directly set!");
      }
    },
    getOffsetByPositionAnchor: function(positionAnchor) {
      var centerX, centerY;
      centerX = this.size[0] / 2 + this.center[0];
      centerY = this.size[1] / 2 + this.center[1];
      return [centerX + (positionAnchor[0] * (this.size[0] / 2) - centerX) * this.scale[0], centerY + (positionAnchor[1] * (this.size[1] / 2) - centerY) * this.scale[1]];
    },
    transform: {
      get: function() {
        if (!this._transform) {
          if (this.parent) {
            this.updateTransform();
          } else {
            this.updateTransform();
          }
        }
        return this._transform;
      },
      set: function(transform) {
        return this._transform = transform;
      }
    },
    addChild: function(node) {
      this._children.push(node);
      return node.parent = this;
    },
    removeChild: function(node) {
      var i, _results;
      i = 0;
      _results = [];
      while (i < this._children.length) {
        if (this._children[i] === node) {
          node.parent = null;
          this._children.splice(i, 1);
          break;
        }
        _results.push(i++);
      }
      return _results;
    },
    removeAllChildren: function() {
      var i;
      i = 0;
      while (i < this._children.length) {
        this._children[i].parent = null;
        i++;
      }
      return this._children.length = 0;
    },
    applyTransform: function(context) {
      var t;
      t = this._transform;
      return context.setTransform(t[0][0], t[0][1], t[1][0], t[1][1], t[2][0], t[2][1]);
    },
    updateTransform: function() {
      var i, matrix, parentTransform, transform, velocity, _results;
      parentTransform = null;
      if (this.parent) parentTransform = this.parent.transform;
      if (this._hasBaseTransform) {
        matrix = enchant.matrix.createMatrixIdentity(3);
        matrix[0][0] = parentTransform[0][0];
        matrix[0][1] = parentTransform[0][1];
        matrix[1][0] = parentTransform[1][0];
        matrix[1][1] = parentTransform[1][1];
        velocity = enchant.matrix.transformPoint(this.velocity, matrix);
        this._transform[2][0] += velocity[0];
        this._transform[2][1] += velocity[1];
      } else {
        transform = enchant.matrix.getNodeTransformMatirx(this.position[0], this.position[1], this.rotation * Math.PI / 180, this.scale[0], this.scale[1]);
        if (parentTransform) {
          transform = enchant.matrix.matrixMultiply(transform, parentTransform);
        }
        this._transform = transform;
      }
      i = 0;
      _results = [];
      while (i < this._children.length) {
        this._children[i].updateTransform();
        _results.push(i++);
      }
      return _results;
    },
    updateAttributes: function() {
      var i, _results;
      if (this.parent) {
        this.depth = this.parent.depth + 1;
        this.absAlpha = this.alpha * this.parent.alpha;
        this.absPriority = 2 * this.priority * this.parent.absPriority;
      } else {
        this.depth = 0;
        this.absAlpha = this.alpha;
        this.absPriority = this.priority;
      }
      i = 0;
      _results = [];
      while (i < this._children.length) {
        this._children[i].updateAttributes();
        _results.push(i++);
      }
      return _results;
    },
    registerDrawCommand: function(drawCommands) {
      var i, _results;
      if (this.absAlpha <= 0) return;
      i = 0;
      _results = [];
      while (i < this._children.length) {
        this._children[i].registerDrawCommand(drawCommands);
        _results.push(i++);
      }
      return _results;
    }
  });

  enchant.canvas.Sprite = enchant.Class.create({
    initialize: function(srcPath, srcRect) {
      this.node = new enchant.canvas.Node();
      this.srcPath = (srcPath ? srcPath : null);
      return this.srcRect = (srcRect ? srcRect : null);
    },
    addChild: function(node) {
      return this.node.addChild(node);
    },
    removeChild: function(node) {
      return this.node.removeChild(node);
    },
    removeAllChildren: function() {
      return this.node.removeAllChildren();
    },
    getOffsetByPositionAnchor: function(positionAnchor) {
      return this.node.getOffsetByPositionAnchor(positionAnchor);
    },
    size: {
      get: function() {
        return this.node.size;
      },
      set: function(value) {
        return this.node.size = value;
      }
    },
    position: {
      get: function() {
        return this.node.position;
      },
      set: function(value) {
        return this.node.position = value;
      }
    },
    alpha: {
      get: function() {
        return this.node.alpha;
      },
      set: function(value) {
        return this.node.alpha = value;
      }
    },
    absAlpha: {
      get: function() {
        return this.node.absAlpha;
      }
    },
    blendType: {
      get: function() {
        return this.node.blendType;
      },
      set: function(value) {
        return this.node.blendType = value;
      }
    },
    center: {
      get: function() {
        return this.node.center;
      },
      set: function(value) {
        return this.node.center = value;
      }
    },
    depth: {
      get: function() {
        return this.node.depth;
      },
      set: function(value) {
        return this.node.depth = value;
      }
    },
    priority: {
      get: function() {
        return this.node.priority;
      },
      set: function(value) {
        return this.node.priority = value;
      }
    },
    absPriority: {
      get: function() {
        return this.node.absPriority;
      }
    },
    hue: {
      get: function() {
        return this.node.hue;
      },
      set: function(value) {
        return this.node.hue = value;
      }
    },
    parent: {
      get: function() {
        return this.node.parent;
      },
      set: function(value) {
        return this.node.parent = value;
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
        return this.node.scale = value;
      }
    },
    children: {
      get: function() {
        return this.node.children;
      },
      set: function(children) {
        return this.node.children = children;
      }
    },
    transform: {
      get: function() {
        return this.node.transform;
      },
      set: function(transform) {
        return this.node.transform = transform;
      }
    },
    applyTransform: function(context) {
      return this.node.applyTransform(context);
    },
    updateTransform: function() {
      var i, parentTransform, transform, _results;
      parentTransform = null;
      if (this.parent) parentTransform = this.parent.transform;
      if (this.srcPath) {
        transform = enchant.matrix.getImageTransformMatirx(-this.position[0], -this.position[1], this.rotation * Math.PI / 180, this.scale[0], this.scale[1]);
        if (parentTransform) {
          transform = enchant.matrix.matrixMultiply(transform, parentTransform);
        }
        this.transform = transform;
        i = 0;
        _results = [];
        while (i < this.children.length) {
          this.children[i].updateTransform();
          _results.push(i++);
        }
        return _results;
      } else {
        return this.node.updateTransform();
      }
    },
    updateAttributes: function() {
      return this.node.updateAttributes();
    },
    registerDrawCommand: function(drawCommands) {
      if (this.absAlpha <= 0) return;
      if (this.srcPath) {
        return drawCommands.push(this);
      } else {
        return this.node.registerDrawCommand(drawCommands);
      }
    },
    draw: function(assets, surface) {
      var key, operation, posX, posY, src, uvCutX, uvCutY;
      if (this.absAlpha === 0) return;
      key = "../../static/assets/images/" + this.srcPath;
      src = assets[key];
      assert(src !== undefined, "No file found at path = " + key);
      if (!this.srcRect) this.srcRect = [0, 0, src.width, src.height];
      if (!this.size) this.size = [this.srcRect[2], this.srcRect[3]];
      this.applyTransform(surface.context);
      surface.context.globalAlpha = this.absAlpha;
      operation = void 0;
      if (this.blendType === "add") {
        operation = "lighter";
      } else {
        operation = "source-over";
      }
      if (operation !== surface.context.globalCompositeOperation) {
        surface.context.globalCompositeOperation = operation;
      }
      assert(typeof this.srcRect[0] === "number", "1");
      assert(typeof this.srcRect[1] === "number", "2");
      assert(typeof this.srcRect[2] === "number", "3");
      assert(typeof this.srcRect[3] === "number", "4");
      assert(typeof this.position[0] === "number", "5");
      assert(typeof this.position[1] === "number", "6");
      posX = Math.floor(this.position[0] - this.center[0] - this.srcRect[2] / 2);
      posY = Math.floor(this.position[1] - this.center[1] - this.srcRect[3] / 2);
      uvCutX = 1;
      uvCutY = 0;
      return surface.draw(src, this.srcRect[0], this.srcRect[1], this.srcRect[2] - uvCutX, this.srcRect[3] - uvCutY, posX, posY, this.srcRect[2], this.srcRect[3]);
    }
  });
