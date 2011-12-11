
  this.module("roga.geom", function() {
    this.Size = (function() {

      function Size(width, height) {
        this.width = width;
        this.height = height;
      }

      return Size;

    })();
    this.Point = (function() {

      function Point(x, y) {
        this.x = x;
        this.y = y;
      }

      return Point;

    })();
    return this.Rectangle = (function() {

      function Rectangle(x, y, width, height) {
        this.x = x;
        this.y = y;
        this.width = width;
        this.height = height;
      }

      Rectangle.prototype.getLeft = function() {
        return this.x;
      };

      Rectangle.prototype.getTop = function() {
        return this.y;
      };

      Rectangle.prototype.getRight = function() {
        return this.x + this.width;
      };

      Rectangle.prototype.getBottom = function() {
        return this.y + this.height;
      };

      return Rectangle;

    })();
  });
