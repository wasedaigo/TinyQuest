enchant.geom = {};

enchant.geom.Size = enchant.Class.create({
    initialize: function(width, height) {
        this.width = width;
        this.height = height;
    }
});

enchant.geom.Point = enchant.Class.create({
    initialize: function(x, y, width, height) {
        this.x = x;
        this.y = y;
    }
});

enchant.geom.Rectangle = enchant.Class.create({
    initialize: function(x, y, width, height) {
        this.x = x;
        this.y = y;
        this.width = width;
        this.height = height;
    },
    right: {
        get: function() {
            return this.x + this.width;
        }
    },
    bottom: {
        get: function() {
            return this.y + this.height;
        }
    }
});