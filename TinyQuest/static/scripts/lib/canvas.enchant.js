(function() {
  var __hasProp = Object.prototype.hasOwnProperty, __extends = function(child, parent) { for (var key in parent) { if (__hasProp.call(parent, key)) child[key] = parent[key]; } function ctor() { this.constructor = child; } ctor.prototype = parent.prototype; child.prototype = new ctor; child.__super__ = parent.prototype; return child; };

  enchant.canvas = {};

  enchant.canvas.SceneGraph = (function() {

    function _Class(game, surface) {
      assert(surface);
      this._game = game;
      this._surface = surface;
      this._root = null;
    }

    _Class.prototype.setRoot = function(node) {
      return this._root = node;
    };

    _Class.prototype.prioritySortFunc = function(a, b) {
      return a.getAbsPriority() - b.getAbsPriority();
    };

    _Class.prototype.update = function() {
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
    };

    return _Class;

  })();

  enchant.canvas.Node = (function() {

    function _Class(baseTransform) {
      this._position = [0, 0];
      this._velocity = [0, 0];
      this._size = [1, 1];
      this._scale = [1, 1];
      this._alpha = 1.0;
      this._absAlpha = 1.0;
      this._depth = 0;
      this._priority = 0.5;
      this._absPriority = 0.5;
      this._blendType = null;
      this._hue = [0, 0, 0];
      this._rotation = 0;
      this._parent = null;
      this._children = [];
      this._transform = null;
      this._hasBaseTransform = false;
      this._center = [0, 0];
      if (baseTransform) {
        this._hasBaseTransform = true;
        this._transform = baseTransform;
      }
    }

    _Class.prototype.getAttribute = function(name) {
      return this["_" + name];
    };

    _Class.prototype.setAttribute = function(name, value) {
      return this["_" + name] = value;
    };

    _Class.prototype.getSize = function() {
      return this._size;
    };

    _Class.prototype.setSize = function(value) {
      return this._size = value;
    };

    _Class.prototype.getPosition = function() {
      return this._position;
    };

    _Class.prototype.setPosition = function(value) {
      return this._position = value;
    };

    _Class.prototype.getVelocity = function() {
      return this._velocity;
    };

    _Class.prototype.setVelocity = function(value) {
      return this._velocity = value;
    };

    _Class.prototype.getAlpha = function() {
      return this._alpha;
    };

    _Class.prototype.setAlpha = function(value) {
      return this._alpha = value;
    };

    _Class.prototype.getAbsAlpha = function() {
      return this._absAlpha;
    };

    _Class.prototype.getBlendType = function() {
      return this._blendType;
    };

    _Class.prototype.setBlendType = function(value) {
      return this._blendType = value;
    };

    _Class.prototype.getCenter = function() {
      return this._center;
    };

    _Class.prototype.setCenter = function(value) {
      return this._center = value;
    };

    _Class.prototype.getDepth = function() {
      return this._depth;
    };

    _Class.prototype.setDepth = function() {
      return this._depth = value;
    };

    _Class.prototype.getPriority = function() {
      return this._priority;
    };

    _Class.prototype.setPriority = function(value) {
      return this._priority = value;
    };

    _Class.prototype.getAbsPriority = function() {
      return this._absPriority;
    };

    _Class.prototype.getHue = function() {
      return this._hue;
    };

    _Class.prototype.setHue = function(value) {
      return this._hue = value;
    };

    _Class.prototype.getParent = function() {
      return this._parent;
    };

    _Class.prototype.setParent = function(value) {
      return this._parent = value;
    };

    _Class.prototype.getRotation = function() {
      return this._rotation;
    };

    _Class.prototype.setRotation = function(value) {
      return this._rotation = value;
    };

    _Class.prototype.getScale = function() {
      return this._scale;
    };

    _Class.prototype.setScale = function(value) {
      return this._scale = value;
    };

    _Class.prototype.getChildren = function() {
      return this._children;
    };

    _Class.prototype.getOffsetByPositionAnchor = function(positionAnchor) {
      var centerX, centerY;
      centerX = this._size[0] / 2 + this._center[0];
      centerY = this._size[1] / 2 + this._center[1];
      return [centerX + (positionAnchor[0] * (this._size[0] / 2) - centerX) * this._scale[0], centerY + (positionAnchor[1] * (this._size[1] / 2) - centerY) * this._scale[1]];
    };

    _Class.prototype.getTransform = function() {
      if (!this._transform) {
        if (this._parent) {
          this.updateTransform();
        } else {
          this.updateTransform();
        }
      }
      return this._transform;
    };

    _Class.prototype.setTransform = function(transform) {
      return {
        set: function(transform) {
          return this._transform = transform;
        }
      };
    };

    _Class.prototype.addChild = function(node) {
      this._children.push(node);
      return node.setParent(this);
    };

    _Class.prototype.removeChild = function(node) {
      var i, _results;
      i = 0;
      _results = [];
      while (i < this._children.length) {
        if (this._children[i] === node) {
          node.setParent(null);
          this._children.splice(i, 1);
          break;
        }
        _results.push(i++);
      }
      return _results;
    };

    _Class.prototype.removeAllChildren = function() {
      var i;
      i = 0;
      while (i < this._children.length) {
        this._children[i].setParent(null);
        i++;
      }
      return this._children.length = 0;
    };

    _Class.prototype.applyTransform = function(context) {
      var t;
      t = this._transform;
      return context.setTransform(t[0][0], t[0][1], t[1][0], t[1][1], t[2][0], t[2][1]);
    };

    _Class.prototype.updateTransform = function() {
      var i, matrix, parentTransform, transform, velocity, _results;
      parentTransform = null;
      if (this._parent) parentTransform = this._parent.getTransform();
      if (this._hasBaseTransform) {
        matrix = enchant.matrix.createMatrixIdentity(3);
        matrix[0][0] = parentTransform[0][0];
        matrix[0][1] = parentTransform[0][1];
        matrix[1][0] = parentTransform[1][0];
        matrix[1][1] = parentTransform[1][1];
        velocity = enchant.matrix.transformPoint(this._velocity, matrix);
        this._transform[2][0] += velocity[0];
        this._transform[2][1] += velocity[1];
      } else {
        transform = enchant.matrix.getNodeTransformMatirx(this._position[0], this._position[1], this._rotation * Math.PI / 180, this._scale[0], this._scale[1]);
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
    };

    _Class.prototype.updateAttributes = function() {
      var i, _results;
      if (this._parent) {
        this._depth = this._parent.getDepth() + 1;
        this._absAlpha = this._alpha * this._parent.getAlpha();
        this._absPriority = 2 * this._priority * this._parent.getAbsPriority();
      } else {
        this._depth = 0;
        this._absAlpha = this._alpha;
        this._absPriority = this._priority;
      }
      i = 0;
      _results = [];
      while (i < this._children.length) {
        this._children[i].updateAttributes();
        _results.push(i++);
      }
      return _results;
    };

    _Class.prototype.registerDrawCommand = function(drawCommands) {
      var i, _results;
      if (this._absAlpha <= 0) return;
      i = 0;
      _results = [];
      while (i < this._children.length) {
        this._children[i].registerDrawCommand(drawCommands);
        _results.push(i++);
      }
      return _results;
    };

    return _Class;

  })();

  enchant.canvas.Sprite = (function() {

    __extends(_Class, enchant.canvas.Node);

    function _Class(srcPath, srcRect) {
      _Class.__super__.constructor.call(this);
      this._srcPath = srcPath || null;
      this._srcRect = srcRect || null;
    }

    _Class.prototype.getSrcPath = function() {
      return this._srcPath;
    };

    _Class.prototype.setSrcPath = function(value) {
      return this._srcPath = value;
    };

    _Class.prototype.getSrcRect = function() {
      return this._srcRect;
    };

    _Class.prototype.setSrcRect = function(value) {
      return this._srcRect = value;
    };

    _Class.prototype.updateTransform = function() {
      var i, parentTransform, transform, _results;
      parentTransform = null;
      if (this._parent) parentTransform = this._parent.getTransform();
      if (this._srcPath) {
        transform = enchant.matrix.getImageTransformMatirx(-this._position[0], -this._position[1], this._rotation * Math.PI / 180, this._scale[0], this._scale[1]);
        if (parentTransform) {
          transform = enchant.matrix.matrixMultiply(transform, parentTransform);
        }
        this._transform = transform;
        i = 0;
        _results = [];
        while (i < this._children.length) {
          this._children[i].updateTransform();
          _results.push(i++);
        }
        return _results;
      } else {
        return _Class.__super__.updateTransform.call(this);
      }
    };

    _Class.prototype.registerDrawCommand = function(drawCommands) {
      if (this.getAbsAlpha() === 0) return;
      if (this._srcPath) {
        return drawCommands.push(this);
      } else {
        return _Class.__super__.registerDrawCommand.call(this, drawCommands);
      }
    };

    _Class.prototype.draw = function(assets, surface) {
      var key, operation, posX, posY, src, uvCutX, uvCutY;
      if (this.getAbsAlpha() === 0) return;
      key = "../../static/assets/images/" + this._srcPath;
      src = assets[key];
      assert(src !== undefined, "No file found at path = " + key);
      if (!this._srcRect) this._srcRect = [0, 0, src.width, src.height];
      if (!this._size) this._size = [this._srcRect[2], this._srcRect[3]];
      this.applyTransform(surface.context);
      surface.context.globalAlpha = this.getAbsAlpha();
      operation = void 0;
      if (this._blendType === "add") {
        operation = "lighter";
      } else {
        operation = "source-over";
      }
      if (operation !== surface.context.globalCompositeOperation) {
        surface.context.globalCompositeOperation = operation;
      }
      assert(typeof this._srcRect[0] === "number", "1");
      assert(typeof this._srcRect[1] === "number", "2");
      assert(typeof this._srcRect[2] === "number", "3");
      assert(typeof this._srcRect[3] === "number", "4");
      assert(typeof this._position[0] === "number", "5");
      assert(typeof this._position[1] === "number", "6");
      posX = Math.floor(this._position[0] - this._center[0] - this._srcRect[2] / 2);
      posY = Math.floor(this._position[1] - this._center[1] - this._srcRect[3] / 2);
      uvCutX = 1;
      uvCutY = 0;
      return surface.draw(src, this._srcRect[0], this._srcRect[1], this._srcRect[2] - uvCutX, this._srcRect[3] - uvCutY, posX, posY, this._srcRect[2], this._srcRect[3]);
    };

    return _Class;

  })();

}).call(this);
