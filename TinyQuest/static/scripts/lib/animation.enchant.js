enchant.animation = {};

enchant.animation.Animation = enchant.Class.create({
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

// Static methods
enchant.animation.Animation = 
{
    LoadAnimation: function (data) {
        for (var i in data) {
            var timeline = data[i];
            loadAlphaKeyframes(timeline["alpha"]);
        }   
    },

    loadAlphaKeyframes: function (keyframes) {
        for (var i in keyframes) {
            console.log(i);
            console.log(keyframes[i]);
            
        }
    }    
}