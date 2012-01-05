enchant.utils = 
{
    clone : function(src) {
      var newObj = (src instanceof Array) ? [] : {};
      for (i in src) {
        if (i == 'clone') continue;
        if (src[i] && typeof src[i] == "object") {
          newObj[i] = enchant.utils.clone(src[i]);
        } else newObj[i] = src[i]
      } return newObj;
    }
};

enchant.animation = {};
enchant.animation.animationManager = 
{
    initialize: function (root) {
        this.root = root;
        this.instances = [];
    },
    start: function (animation) {
        this.instances.push(animation);
        this.root.addChild(animation.node);
        animation.interval.start();
    },
    update: function () {
        for (var i = this.instances.length - 1; i >= 0; i-- ) {
            var instance = this.instances[i];
            instance.interval.update();
            if (instance.interval.isDone()) {
                this.root.removeChild(instance.node);
                this.instances.splice(i, 1);
            }
        }
    },
    CreateAnimation: function (data, isSubAnimation, baseTransform, baseAlpha, basePriority, target) {
        var timelines = data["timelines"];
        var parallels = [];
        var node = new enchant.canvas.Node(baseTransform);
        node.alpha = baseAlpha;
        node.priority = basePriority;
        
        for (var timelineNo in timelines) {
            var timeline = timelines[timelineNo];
            
            var sprite = new enchant.canvas.Sprite();
            var sequences = [];
            var attributes = ["alpha", "scale", "position", "rotation"];
            for (var i in attributes) {
                var attribute = attributes[i];
                
                var sequence = enchant.animation.animationManager.CreateAttributeTween(sprite, attribute, timeline[attribute], target);
                if (sequence) {
                    sequences.push(sequence);
                }
            }
			var sourceInterval = new enchant.animation.interval.SourceInterval(sprite, timeline["source"], target);
			sequences.push(sourceInterval);
            parallels.push(new enchant.animation.interval.Parallel(sequences));
            node.addChild(sprite);
        }
        var parallelInterval = new enchant.animation.interval.Parallel(parallels);
        
        var interval = null;
        if (isSubAnimation) {
            interval = new enchant.animation.interval.Loop(parallelInterval, 0);
        } else {
            interval = parallelInterval;
        }
        
        return {"interval" : interval, "node" : node};
    },
    // Create attribute tween out of given keyframe data
    CreateAttributeTween: function (node, attribute, keyframes, target) {
        if (keyframes.length == 0) {
            return null;
        } else {
            var intervals = [];
            for(var i = 0; i < keyframes.length; i++) {
                var frame = keyframes[i];
                var tween = frame.tween;
                var interval = null;
                if (frame.wait) {
                    var duration = frame.duration;

                    interval = new enchant.animation.interval.Wait(duration);
                } else {
                    var startValue = frame.startValue;
                    var endValue = frame.endValue;
                    var duration = frame.duration;
                    var options = {};
                    // PositionTween does something special
                    if (attribute == "position") {
                        options.startPositionAnchor = frame.startPositionAnchor;
                        options.endPositionAnchor = frame.endPositionAnchor;
                        options.startPositionType = frame.startPositionType;
                        options.endPositionType = frame.endPositionType;
                        options.target = target;
                    } else if (attribute == "rotation") {
                        options.facingOption = frame.facingOption;
                        options.target = target;
                    }

                    interval = new enchant.animation.interval.AttributeInterval(node, attribute, startValue, endValue, duration, tween, options);
                }
                intervals.push(interval);
            }

            return new enchant.animation.interval.Sequence(intervals);
        }
    }  
};

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
                value = startValue[i] + (endValue[i] - startValue[i]) * proportion;
                result.push(value);
            } 
        }

        return result;
    },
    _GetRelativePosition: function (node, target, positionType, positionAnchor, offset) {
        var result = offset;
        
        if (positionType == "RelativeToTarget") {
            var anchorOffset = target.node.getOffsetByPositionAnchor(positionAnchor);
            var tempPos = [target.node.position[0], target.node.position[1]];
            tempPos[0] = tempPos[0] + offset[0] + anchorOffset[0];
            tempPos[1] = tempPos[1] + offset[1] + anchorOffset[1];
            var targetPosition = enchant.matrix.transformPoint(tempPos, target.node.parent.transform);
            var invertMatrix = enchant.matrix.createInverseMatrix(node.parent.transform, 3);
            result = enchant.matrix.transformPoint(targetPosition, invertMatrix);

        } else if (positionType == "RelativeToTargetOrigin") {
            var anchorOffset = target.node.getOffsetByPositionAnchor(positionAnchor);
            var tempPos = [target.origin[0], target.origin[1]];
            tempPos[0] = tempPos[0] + offset[0] + anchorOffset[0];
            tempPos[1] = tempPos[1] + offset[1] + anchorOffset[1];
            var targetPosition = enchant.matrix.transformPoint(tempPos, target.node.parent.transform);
            var invertMatrix = enchant.matrix.createInverseMatrix(node.parent.transform, 3);
            result = enchant.matrix.transformPoint(targetPosition, invertMatrix);
            
        }
        return result;
    },
    CalculateRelativePosition: function (startValue, endValue, node, startPositionType, endPositionType, startPositionAnchor, endPositionAnchor, target) {
        var resultStartValue = startValue;
        var resultEndValue = endValue;
        if (target  && node.parent) {
            // [OPTIMIZABLE]
            resultStartValue = _GetRelativePosition(node, target, startPositionType, startPositionAnchor, startValue);
            resultEndValue = _GetRelativePosition(node, target, endPositionType, endPositionAnchor, endValue);
        }

        return [resultStartValue, resultEndValue];
    },
    CalculateDynamicRotation: function (startValue, endValue, node, facingOption, target, dataStore) {
        if (target) {
            var invertMatrix = null;
            if (facingOption == "FaceToDir") {
                var absStartPosition;
                var absTargetPosition;
                if (node.parent) {
                    absStartPosition = enchant.matrix.transformPoint(node.position, node.parent.transform);
                    absTargetPosition = enchant.matrix.transformPoint(target.position, node.parent.transform);
                } else {
                    absStartPosition = node.position;
                    absTargetPosition = target.position;
                }
                var dx = absTargetPosition[0] - absStartPosition.position[0];
                var dy = absTargetPosition[1] - absStartPosition.position[1];
                startValue = (Math.atan2(dy,dx) / Math.PI)  * 180;
                endValue = startValue;
                
            } else if (facingOption == "FaceToMov") {
                var absStartPosition = dataStore.lastAbsPosition ? dataStore.lastAbsPosition : [0, 0];
                var absTargetPosition = node.position;
                if (node.parent) {
                    absTargetPosition = enchant.matrix.transformPoint(node.position, node.parent.transform);
                }
                var dx = absTargetPosition[0] - absStartPosition[0];
                var dy = absTargetPosition[1] - absStartPosition[1];
                
                startValue += (Math.atan2(dy,dx) / Math.PI)  * 180;
                endValue = startValue;
                if (dataStore.ignore) {
                    node.parent.alpha = dataStore.lastAlpha;
                    
                } else {
                    dataStore.ignore = true;
                    dataStore.lastAlpha = node.parent.alpha;
                    node.parent.alpha = 0;
                }

                // Extra data for FaceToMov option
                dataStore.lastAbsPosition = absTargetPosition;
            }
        }

        return [startValue, endValue]
    },
    CalculateAttributeValues: function (attribute, startValue, endValue, node, options, dataStore) {
        var result = null;
        if (attribute == "position") {
            result = enchant.animation.interval.CalculateRelativePosition(startValue, endValue, node, options.startPositionType, options.endPositionType, options.startPositionAnchor, options.endPositionAnchor, options.target);
        } else if (attribute == "rotation") {
            result = enchant.animation.interval.CalculateDynamicRotation(startValue, endValue, node, options.facingOption, options.target, dataStore);
        }
        
        return result
    }
}

// Linear interval for simple parameter
enchant.animation.interval.AttributeInterval = enchant.Class.create({
    initialize: function(node, attribute, startValue, endValue, duration, tween, options) {
        this._node = node;
        this._startValue = startValue;
        this._endValue = endValue;
        this._duration = duration;
        this._attribute = attribute;    
        this._frameNo = 0;
        this._tween = tween;
        this._dataStore = {};
        this._options = options;
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
        this.updateValue();
    },
    finish: function() {
    },
    updateValue : function() {
        var startValue = this._startValue;
        var endValue = this._endValue;
        
        var result = enchant.animation.interval.CalculateAttributeValues(this._attribute, this._startValue, this._endValue, this._node, this._options, this._dataStore);
        if (result) {
            startValue = result[0];
            endValue = result[1];
        }

        var value = startValue;
        if (this._tween) {
            value = enchant.animation.interval.Completement(startValue, endValue, this._frameNo / this._duration);
        }
        this._node[this._attribute] = value;
    },
    update: function() {
        if (!this.isDone()) {
            this._frameNo++;
            this.updateValue();
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
    finish: function() {
    },
    update: function() {
        if (!this.isDone()) {
            this._frameNo++;
        }
    }
});

// Source file keykeyframes, this changes image and source rect of sprites
enchant.animation.interval.SourceInterval = enchant.Class.create({
    initialize: function(sprite, sourceKeykeyframes, target) {
        this._sprite = sprite;
        this._interval = null;
        this._sourceKeykeyframes = enchant.utils.clone(sourceKeykeyframes);
        this._frameNo = 0;
        this._index = 0;
        this._frameDuration = 0;
        this._duration = 0;
        this._target = target;
        this._lastAnimationId = "";
        for (var key in this._sourceKeykeyframes) {
            this._duration += this._sourceKeykeyframes[key].duration;  
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
        if (this._lastAnimationId != keyframe.id) {
            this._clearSetting();
        }
        this._lastAnimationId = keyframe.id;
    
        this._sprite.priority = keyframe.priority ? keyframe.priority : 0.5;
        this._sprite.blendType = keyframe.blendType ? keyframe.blendType : "None";
        if (keyframe.type == "Image") {
            this._sprite.srcPath = keyframe.id + ".png";
            this._sprite.srcRect = keyframe.rect;
            this._sprite.center = keyframe.center;
        } else {
            // Display nested animations
            if (this._interval) {
                this._interval.update();
            } else {
                if (keyframe.emitter) {
                    // Calculate a new transformation matrix, at which the new animation to be emitted
                    var transform = null;
                    if (this._sprite.parent) {
                        var transform = this._sprite.parent.transform;
                        transform = enchant.matrix.getNodeTransformMatirx(this._sprite.position[0], this._sprite.position[1], this._sprite.rotation * Math.PI / 180, this._sprite.scale[0], this._sprite.scale[1]);       
                        transform = enchant.matrix.matrixMultiply(transform, this._sprite.parent.transform);
                    }

                    // Emit the new animation (emitted animation won't be controled by this instance anymore)
                    var animation = enchant.animation.animationManager.CreateAnimation(enchant.loader.getAnimation(keyframe.id), false, transform, this._sprite.alpha, this._sprite.priority, this._target);
                    
                    // Apply emit velocity
                    if (keyframe.maxEmitSpeed > 0) {
                        var speed = keyframe.minEmitSpeed + (keyframe.maxEmitSpeed - keyframe.minEmitSpeed) * Math.random();
                        var angle = keyframe.minEmitAngle + (keyframe.maxEmitAngle - keyframe.minEmitAngle) * Math.random();
                        var rad = (angle / 180) * Math.PI;
                        animation.node.velocity[0] = speed * Math.cos(rad);
                        animation.node.velocity[1] = speed * Math.sin(rad);
                    }
                    enchant.animation.animationManager.start(animation);
                } else {
                    // No animation node is generaetd yet, let's generate it
                    // If no ID exists, ignore it (Which usually means an empty keyframe)
                    if (keyframe.id) {
                        // Update the sprite's transform
                        this._sprite.updateTransform();
                        var animation = enchant.animation.animationManager.CreateAnimation(enchant.loader.getAnimation(keyframe.id), true, null, 1, 0.5, this._target);
                        this._sprite.addChild(animation.node);
                        this._interval = animation.interval;
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
    finish: function() {
        this._clearSetting();
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
    finish: function() {
        this._interval.finish();
    },
    update: function() {
        if (!this.isDone()) {
            this._interval.update();
            if (this._interval.isDone()) {
                this._loopCounter++;
                if (this._loopCount == 0 || this._loopCounter < this._loopCount) {
                    // Repeat this interval again, since this is a subanimation
                    this._interval.reset();
                    //this._interval.update();
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
    finish: function() {
        this._intervals[this._index].finish();
    },
    update: function() {
        if (!this.isDone()) {
            var currentInterval = this._intervals[this._index];
            currentInterval.update();
            if (this.isDone()) {
                this.finish();
            } else if (currentInterval.isDone()) {
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
    finish: function() {
        for (var i in this._intervals) {
            this._intervals[i].finish();
        }
    },
    update: function() {
        if (!this.isDone()) {
            for (var i in this._intervals) {
                this._intervals[i].update();
            }
            if (this.isDone()) {
                this.finish();
            }
        }
    }
});
