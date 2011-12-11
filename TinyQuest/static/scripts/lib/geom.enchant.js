
  enchant.geom = {};

  enchant.geom.Size = (function() {

    function _Class(width, height) {
      this.width = width;
      this.height = height;
    }

    return _Class;

  })();

  enchant.geom.Point = (function() {

    function _Class(x, y) {
      this.x = x;
      this.y = y;
    }

    return _Class;

  })();

  enchant.geom.Rectangle = (function() {

    function _Class(x, y, width, height) {
      this.x = x;
      this.y = y;
      this.width = width;
      this.height = height;
    }

    _Class.prototype.getLeft = function() {
      return this.x;
    };

    _Class.prototype.getTop = function() {
      return this.y;
    };

    _Class.prototype.getRight = function() {
      return this.x + this.width;
    };

    _Class.prototype.getBottom = function() {
      return this.y + this.height;
    };

    return _Class;

  })();
