enchant.animation = 
{
    CreateAnimation: function (root, data, isSubAnimation) {
        var timelines = data["timelines"];
        var parallels = [];
        for (var timelineNo in timelines) {
            var timeline = timelines[timelineNo];
            
            var sprite = new enchant.canvas.Sprite();
            var sequences = [];
            var attributes = ["rotation", "position", "alpha", "scale"];
            for (var i in attributes) {
                var attribute = attributes[i];
                var sequence = enchant.animation.CreateAttributeTween(sprite, attribute, timeline[attribute]);
                if (sequence) {
                    sequences.push(sequence);
                }
            }
			var sourceInterval = new enchant.animation.interval.SourceInterval(sprite, timeline["source"]);
			sequences.push(sourceInterval);
            parallels.push(new enchant.animation.interval.Parallel(sequences));
            root.addChild(sprite);
        }
        var parallelInterval = new enchant.animation.interval.Parallel(parallels);
        
        var interval = null;
        if (isSubAnimation) {
            interval = new enchant.animation.interval.Loop(parallelInterval, 0);
        } else {
            interval = parallelInterval;
        }
        return interval;
    },
    // Create attribute tween out of given keyframe data
    CreateAttributeTween: function (node, attribute, keyframes) {
        if (keyframes.length == 0) {
            return null;
        } else {
            var intervals = [];
            for(var i = 0; i < keyframes.length; i++) {
                var frame = keyframes[i];
                
                var interval = null;
                if (frame.wait) {
                    var duration = frame.duration;

                    interval = new enchant.animation.interval.Wait(duration);
                } else {
                    var startValue = frame.startValue;
                    var endValue = frame.endValue;
                    var duration = frame.duration;
    
                    interval = new enchant.animation.interval.Interval(node, attribute, startValue, endValue, duration);
                }
                intervals.push(interval);
            }

            return new enchant.animation.interval.Sequence(intervals);
        }
    }  
}

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
    reset: function() {
        this._frameNo = 0;
        this.start();
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

// Wait component of intervals
enchant.animation.interval.Wait = enchant.Class.create({
    initialize: function(duration) {
        this._duration = duration;
        this._frameNo = 0;
    },
    isDone: function() {
        return this._frameNo >= this._duration
    },
    reset: function() {
        this._frameNo = 0;
        this.start();
    }, 
    start: function() {
        this._frameNo = 0;
    },
    update: function() {
        if (!this.isDone()) {
            this._frameNo++;
        }
    }
});

// Source file keykeyframes, this changes image and source rect of sprites
enchant.animation.interval.SourceInterval = enchant.Class.create({
    initialize: function(sprite, sourceKeykeyframes) {
        this._sprite = sprite;
        this._interval = null;
        this._sourceKeykeyframes = sourceKeykeyframes;
        this._frameNo = 0;
        this._index = 0;
        this._frameDuration = 0;
        this._duration = 0;
        for (var key in sourceKeykeyframes) {
            this._duration += sourceKeykeyframes[key].duration;  
        }
    },
    isDone: function() {
        return this._frameNo >= this._duration;
    },
    _clearSetting: function() {
        this._sprite.srcRect = null;
        this._sprite.srcPath = null;
        if (this._interval) {
            this._interval = null;
            this._sprite.removeAllChildren();
        }
    },
    _updateKeyframe: function(keyframe) {
        // Empty frame found, reset the setting
        if (keyframe.id == "") {
            this._clearSetting();
        }
        
        if (keyframe.type == "image") {
            // Display static image
            this._clearSetting();
            
            this._sprite.srcPath = keyframe.id + ".png";
            this._sprite.srcRect = keyframe.rect;
            this._sprite.center = keyframe.center;
            
        } else {
            // Display nested animations
            if (this._interval) {
                this._interval.update();
            } else {
                this._clearSetting();
                
                if (keyframe.emitter) {
                    // Emit new animation (emitted animation won't be controled by this instance anymore)
                } else {
                    // No animation node is generaetd yet, let's generate it
                    // If no ID exists, ignore it (Which usually means an empty keyframe)
                    if (keyframe.id) {
                        this._interval = enchant.animation.CreateAnimation(this._sprite, enchant.loader.getAnimation(keyframe.id), true);
                        this._interval.start();
                    }
                }
            }
        }
    },
    reset: function() {
        this._frameNo = 0;
        this._index = 0;
        this._frameDuration = 0;
        if (this._interval) {
            this._interval.reset();
        }
    }, 
    start: function() {
        var keyframe = this._sourceKeykeyframes[0];
        this._updateKeyframe(keyframe);
    },
    update: function() {
       if (this.isDone()) {
            this._clearSetting();
        } else {
            this._frameDuration++;
            this._frameNo++;

            var keyframe = this._sourceKeykeyframes[this._index];
            this._updateKeyframe(keyframe);

            if (this._frameDuration >= keyframe.duration) {
                this._index++;
                this._frameDuration = 0;
            }
        }
    }
});

// Specify loop time of intervals
enchant.animation.interval.Loop = enchant.Class.create({
    initialize: function(interval, loopCount) {
        this._loopCounter = 0;
        this._loopCount = loopCount;
        this._interval = interval;
    },
    isDone: function() {
        var _isDone = false;
        if (this._loopCount == 0) {
            // Infinite loop never ends
            _isDone = false
        } else {
            // This is how to determine whether the interval is in the last frame or not
            _isDone = this._interval.isDone() && this._loopCounter >= this._loopCount - 1;
        }
        return _isDone;
    },
    reset: function() {
        this._loopCounter = 0;
        this._interval.reset();
    }, 
    start: function() {
        this._interval.start();
    },
    update: function() {
        if (!this.isDone()) {
            this._interval.update();
            if (this._interval.isDone()) {
                this._loopCounter++;
                if (this._loopCount == 0 || this._loopCounter < this._loopCount) {
                    // Repeat this interval again, since this is a subanimation
                    this._interval.reset();
                }
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
    reset: function() {
        this._index = 0;
        for (var i in this._intervals) {
            this._intervals[i].reset();
        }
        this.start();
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
    reset: function() {
        for (var i in this._intervals) {
            this._intervals[i].reset();
        }
        this.start();
    }, 
    start: function() {
        for (var i in this._intervals) {
            this._intervals[i].start();
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
