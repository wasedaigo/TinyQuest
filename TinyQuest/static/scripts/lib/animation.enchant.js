enchant.animation = {};

// Interval helper modules
enchant.animation.interval  = 
{
    Completement: function (startValue, endValue, proportion) {
        var result = null;
        if (typeof(startValue) == "number") {
            result = startValue + (endValue - startValue) * proportion;
        } else if (typeof(startValue) == "object") {
            result = [];
            for (var i = 0; i < startValue.length; i++) {
                var value = startValue[i] + (endValue[i] - startValue[i]) * proportion;
                result.push(value);
            } 
        }

        return result;
    }  
}

// Linear interval for simple parameter
enchant.animation.interval.Interval = enchant.Class.create({
    initialize: function(node, attribute, startValue, endValue, duration) {
        this._node = node;
        this._startValue = startValue;
        this._endValue = endValue;
        this._duration = duration;
        this._attribute = attribute;
        this._frameNo = 0;
    },
    isDone: function() {
        return this._frameNo >= this._duration
    },
    start: function() {
        this._node[this._attribute] = this._startValue;
        this._frameNo = 0;
    },
    update: function() {
        if (!this.isDone()) {
            this._frameNo++;
            var value = enchant.animation.interval.Completement(this._startValue, this._endValue, this._frameNo / this._duration);
            this._node[this._attribute] = value;
        }
    }
});

// Source file keyframes, this changes image and source rect of sprites
enchant.animation.interval.SourceInterval = enchant.Class.create({
    initialize: function(sprite, sourceKeyframes) {
        this._sprite = sprite;
        this._sourceKeyframes = sourceKeyframes;
        this._frameNo = 0;
        this._index = 0;
        this._duration = 0;
        this._frameDuration = 0;
        for (var key in sourceKeyframes) {
            this._duration += sourceKeyframes[key].duration;  
        }
    },
    isDone: function() {
        return this._frameNo >= this._duration;
    },
    start: function() {
        var keyframe = this._sourceKeyframes[0];

        this._sprite.srcPath = keyframe.path;
        this._sprite.srcRect = keyframe.rect;
    },
    update: function() {
            if (!this.isDone()) {
            this._frameDuration++;
            this._frameNo++;

            var keyframe = this._sourceKeyframes[this._index];
        
            this._sprite.srcPath = keyframe.path;
            this._sprite.srcRect = keyframe.rect;
            if (this._frameDuration >= keyframe.duration) {
                this._index++;
                this._frameDuration = 0;
            }
        }
    }
});

// Run intervals in sequence
enchant.animation.interval.Sequence = enchant.Class.create({
    initialize: function(intervals) {
        this._intervals = intervals;
        this._index = 0;
        var length = this._intervals.length;
        this._lastInterval = this._intervals[length - 1];
    },
    isDone: function() {
        return this._lastInterval.isDone();
    },
    start: function() {
        this._intervals[0].start();
    },
    update: function() {
        if (!this.isDone()) {
            var currentInterval = this._intervals[this._index];
            currentInterval.update();
            if (currentInterval.isDone()) {
                this._index++;
            }
        }
    }
});

// Run intervals in parallel
enchant.animation.interval.Parallel = enchant.Class.create({
    initialize: function(intervals) {
        this._intervals = intervals;
    },
    
    // Check whether all intervals are done
    isDone: function() {
        var isDone = true;
        
        for (var i in this._intervals) {
            if (!this._intervals[i].isDone()) {
                isDone = false;
                break;
            }
        }

        return isDone;
    },
    start: function() {
        for (var interval in this._intervals) {
            this._intervals[0].start();
        }
    },
    update: function() {
        if (!this.isDone()) {
            for (var i in this._intervals) {
                this._intervals[i].update();
            }
        }
    }
});

// Static methods
enchant.animation.Animation = 
{
    LoadAnimation: function (data) {
        
        for (var timelineNo in data) {
            var keyframeset = data[timelineNo];
            
            var alphaKeyframes = keyframeset.alpha;
            for (var keyframeNo in alphaKeyframes) {
                var alphaKeyframe = alphaKeyframes[keyframeNo];
                
                var startValue = alphaKeyframe.value;
                var endValue = 0;
                if (alphaKeyframe.tween && alphaKeyframes.length < keyframeNo) {
                    endValue = alphaKeyframes[keyframeNo + 1].value;
                } else {
                    endValue = startValue;
                }
            }
        }
    }  
}

