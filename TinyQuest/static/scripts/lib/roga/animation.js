
  roga.animation = {};

  roga.animation.animationManager = {
    initialize: function(root) {
      this.root = root;
      return this.instances = [];
    },
    start: function(animation) {
      this.instances.push(animation);
      this.root.addChild(animation.node);
      return animation.interval.start();
    },
    update: function() {
      var i, instance, _results;
      i = this.instances.length - 1;
      _results = [];
      while (i >= 0) {
        instance = this.instances[i];
        instance.interval.update();
        if (instance.interval.isDone()) {
          this.root.removeChild(instance.node);
          this.instances.splice(i, 1);
        }
        _results.push(i--);
      }
      return _results;
    },
    CreateAnimation: function(data, isSubAnimation, baseTransform, baseAlpha, basePriority, target) {
      var attribute, attributes, i, interval, node, parallelInterval, parallels, sequence, sequences, sourceInterval, sprite, timeline, timelineNo, timelines;
      timelines = data["timelines"];
      parallels = [];
      node = new roga.canvas.Node(baseTransform);
      node.setAlpha(baseAlpha);
      node.setPriority(basePriority);
      for (timelineNo in timelines) {
        timeline = timelines[timelineNo];
        sprite = new roga.canvas.Sprite();
        sequences = [];
        attributes = ["alpha", "scale", "position", "rotation"];
        for (i in attributes) {
          attribute = attributes[i];
          sequence = roga.animation.animationManager.CreateAttributeTween(sprite, attribute, timeline[attribute], target);
          if (sequence) sequences.push(sequence);
        }
        sourceInterval = new roga.animation.interval.SourceInterval(sprite, timeline["source"], target);
        sequences.push(sourceInterval);
        parallels.push(new roga.animation.interval.Parallel(sequences));
        node.addChild(sprite);
      }
      parallelInterval = new roga.animation.interval.Parallel(parallels);
      interval = null;
      if (isSubAnimation) {
        interval = new roga.animation.interval.Loop(parallelInterval, 0);
      } else {
        interval = parallelInterval;
      }
      return {
        interval: interval,
        node: node
      };
    },
    CreateAttributeTween: function(node, attribute, keyframes, target) {
      var duration, endValue, frame, i, interval, intervals, options, startValue, tween;
      if (keyframes.length === 0) {
        return null;
      } else {
        intervals = [];
        i = 0;
        while (i < keyframes.length) {
          frame = keyframes[i];
          tween = frame.tween;
          interval = null;
          if (frame.wait) {
            duration = frame.duration;
            interval = new roga.animation.interval.Wait(duration);
          } else {
            startValue = frame.startValue;
            endValue = frame.endValue;
            duration = frame.duration;
            options = {};
            if (attribute === "position") {
              options.startPositionAnchor = frame.startPositionAnchor;
              options.endPositionAnchor = frame.endPositionAnchor;
              options.startPositionType = frame.startPositionType;
              options.endPositionType = frame.endPositionType;
              options.target = target;
            } else if (attribute === "rotation") {
              options.facingOption = frame.facingOption;
              options.target = target;
            }
            interval = new roga.animation.interval.AttributeInterval(node, attribute, startValue, endValue, duration, tween, options);
          }
          intervals.push(interval);
          i++;
        }
        return new roga.animation.interval.Sequence(intervals);
      }
    }
  };

  roga.animation.interval = {
    Completement: function(startValue, endValue, proportion) {
      var i, result, value;
      result = null;
      if (typeof startValue === "number") {
        result = startValue + (endValue - startValue) * proportion;
      } else if (typeof startValue === "object") {
        result = [];
        i = 0;
        while (i < startValue.length) {
          value = startValue[i] + (endValue[i] - startValue[i]) * proportion;
          result.push(value);
          i++;
        }
      }
      return result;
    },
    _GetRelativePosition: function(node, target, positionType, positionAnchor, offset) {
      var anchorOffset, invertMatrix, result, targetPosition, tempPos;
      result = offset;
      if (positionType === "relativeToTarget") {
        anchorOffset = target.node.getOffsetByPositionAnchor(positionAnchor);
        tempPos = [target.node.getPosition()[0], target.node.getPosition()[1]];
        tempPos[0] = tempPos[0] + offset[0] + anchorOffset[0];
        tempPos[1] = tempPos[1] + offset[1] + anchorOffset[1];
        targetPosition = roga.matrix.transformPoint(tempPos, target.node.getParent().getTransform());
        invertMatrix = roga.matrix.createInverseMatrix(node.getParent().getTransform(), 3);
        result = roga.matrix.transformPoint(targetPosition, invertMatrix);
      } else if (positionType === "relativeToTargetOrigin") {
        anchorOffset = target.node.getOffsetByPositionAnchor(positionAnchor);
        tempPos = [target.origin[0], target.origin[1]];
        tempPos[0] = tempPos[0] + offset[0] + anchorOffset[0];
        tempPos[1] = tempPos[1] + offset[1] + anchorOffset[1];
        targetPosition = roga.matrix.transformPoint(tempPos, target.node.getParent().getTransform());
        invertMatrix = roga.matrix.createInverseMatrix(node.getParent().getTransform(), 3);
        result = roga.matrix.transformPoint(targetPosition, invertMatrix);
      }
      return result;
    },
    CalculateRelativePosition: function(startValue, endValue, node, startPositionType, endPositionType, startPositionAnchor, endPositionAnchor, target) {
      var resultEndValue, resultStartValue;
      resultStartValue = startValue;
      resultEndValue = endValue;
      if (target && node.getParent()) {
        resultStartValue = this._GetRelativePosition(node, target, startPositionType, startPositionAnchor, startValue);
        resultEndValue = this._GetRelativePosition(node, target, endPositionType, endPositionAnchor, endValue);
      }
      return [resultStartValue, resultEndValue];
    },
    CalculateDynamicRotation: function(startValue, endValue, node, facingOption, target, dataStore) {
      var absStartPosition, absTargetPosition, dx, dy, invertMatrix;
      if (target) {
        invertMatrix = null;
        if (facingOption === "faceToDir") {
          absStartPosition = void 0;
          absTargetPosition = void 0;
          if (node.getParent()) {
            absStartPosition = roga.matrix.transformPoint(node.getPosition(), node.getParent().getTransform());
            absTargetPosition = roga.matrix.transformPoint(target.node.getPosition(), node.getParent().getTransform());
          } else {
            absStartPosition = node.getPosition();
            absTargetPosition = target.node.getPosition();
          }
          dx = absTargetPosition[0] - absStartPosition.getPosition()[0];
          dy = absTargetPosition[1] - absStartPosition.getPosition()[1];
          startValue = (Math.atan2(dy, dx) / Math.PI) * 180;
          endValue = startValue;
        } else if (facingOption === "faceToMov") {
          absStartPosition = (dataStore.lastAbsPosition ? dataStore.lastAbsPosition : [0, 0]);
          absTargetPosition = node.getPosition();
          if (node.getParent()) {
            absTargetPosition = roga.matrix.transformPoint(node.getPosition(), node.getParent().getTransform());
          }
          dx = absTargetPosition[0] - absStartPosition[0];
          dy = absTargetPosition[1] - absStartPosition[1];
          startValue += (Math.atan2(dy, dx) / Math.PI) * 180;
          endValue = startValue;
          if (dataStore.ignore) {
            node.getParent().setAlpha(dataStore.lastAlpha);
          } else {
            dataStore.ignore = true;
            dataStore.lastAlpha = node.getParent().getAlpha();
            node.getParent().setAlpha(0);
          }
          dataStore.lastAbsPosition = absTargetPosition;
        }
      }
      return [startValue, endValue];
    },
    CalculateAttributeValues: function(attribute, startValue, endValue, node, options, dataStore) {
      var result;
      result = null;
      if (attribute === "position") {
        result = roga.animation.interval.CalculateRelativePosition(startValue, endValue, node, options.startPositionType, options.endPositionType, options.startPositionAnchor, options.endPositionAnchor, options.target);
      } else {
        if (attribute === "rotation") {
          result = roga.animation.interval.CalculateDynamicRotation(startValue, endValue, node, options.facingOption, options.target, dataStore);
        }
      }
      return result;
    }
  };

  this.module("roga.animation.interval", function() {
    this.AttributeInterval = (function() {

      function AttributeInterval(node, attribute, startValue, endValue, duration, tween, options) {
        this._node = node;
        this._startValue = startValue;
        this._endValue = endValue;
        this._duration = duration;
        this._attribute = attribute;
        this._frameNo = 0;
        this._tween = tween;
        this._dataStore = {};
        this._options = options;
      }

      AttributeInterval.prototype.isDone = function() {
        return this._frameNo >= this._duration;
      };

      AttributeInterval.prototype.reset = function() {
        this._frameNo = 0;
        return this.start();
      };

      AttributeInterval.prototype.start = function() {
        this._node.setAttribute(this._attribute, this._startValue);
        this._frameNo = 0;
        return this.updateValue();
      };

      AttributeInterval.prototype.finish = function() {};

      AttributeInterval.prototype.updateValue = function() {
        var endValue, result, startValue, value;
        startValue = this._startValue;
        endValue = this._endValue;
        result = roga.animation.interval.CalculateAttributeValues(this._attribute, this._startValue, this._endValue, this._node, this._options, this._dataStore);
        if (result) {
          startValue = result[0];
          endValue = result[1];
        }
        value = startValue;
        if (this._tween) {
          value = roga.animation.interval.Completement(startValue, endValue, this._frameNo / this._duration);
        }
        return this._node.setAttribute(this._attribute, value);
      };

      AttributeInterval.prototype.update = function() {
        if (!this.isDone()) {
          this._frameNo++;
          return this.updateValue();
        }
      };

      return AttributeInterval;

    })();
    this.Wait = (function() {

      function Wait(duration) {
        this._duration = duration;
        this._frameNo = 0;
      }

      Wait.prototype.isDone = function() {
        return this._frameNo >= this._duration;
      };

      Wait.prototype.reset = function() {
        this._frameNo = 0;
        return this.start();
      };

      Wait.prototype.start = function() {
        return this._frameNo = 0;
      };

      Wait.prototype.finish = function() {};

      Wait.prototype.update = function() {
        if (!this.isDone()) return this._frameNo++;
      };

      return Wait;

    })();
    this.SourceInterval = (function() {

      function SourceInterval(sprite, sourceKeykeyframes, target) {
        var key;
        this._sprite = sprite;
        this._interval = null;
        this._sourceKeykeyframes = roga.Helper.instance().clone(sourceKeykeyframes);
        this._frameNo = 0;
        this._index = 0;
        this._frameDuration = 0;
        this._duration = 0;
        this._target = target;
        this._lastAnimationId = "";
        for (key in this._sourceKeykeyframes) {
          this._duration += this._sourceKeykeyframes[key].duration;
        }
      }

      SourceInterval.prototype.isDone = function() {
        return this._frameNo >= this._duration;
      };

      SourceInterval.prototype._clearSetting = function() {
        this._sprite.setSrcRect(null);
        this._sprite.setSrcPath(null);
        if (this._interval) {
          this._interval = null;
          return this._sprite.removeAllChildren();
        }
      };

      SourceInterval.prototype._updateKeyframe = function(keyframe) {
        var angle, animation, rad, speed, transform;
        if (this._lastAnimationId !== keyframe.id) this._clearSetting();
        this._lastAnimationId = keyframe.id;
        this._sprite.setPriority(keyframe.priority || 0.5);
        this._sprite.setBlendType(keyframe.blendType || "none");
        if (keyframe.type === "image") {
          this._sprite.setSrcPath(keyframe.id + ".png");
          this._sprite.setSrcRect(keyframe.rect);
          return this._sprite.setCenter(keyframe.center);
        } else {
          if (this._interval) {
            return this._interval.update();
          } else {
            if (keyframe.emitter) {
              transform = null;
              if (this._sprite.getParent()) {
                transform = this._sprite.getParent().getTransform();
                transform = roga.matrix.getNodeTransformMatirx(this._sprite.getPosition()[0], this._sprite.getPosition()[1], this._sprite.getRotation() * Math.PI / 180, this._sprite.getScale()[0], this._sprite.getScale()[1]);
                transform = roga.matrix.matrixMultiply(transform, this._sprite.getParent().getTransform());
              }
              animation = roga.animation.animationManager.CreateAnimation(enchant.loader.getAnimation(keyframe.id), false, transform, this._sprite.getAlpha(), this._sprite.getPriority(), this._target);
              if (keyframe.maxEmitSpeed > 0) {
                speed = keyframe.minEmitSpeed + (keyframe.maxEmitSpeed - keyframe.minEmitSpeed) * Math.random();
                angle = keyframe.minEmitAngle + (keyframe.maxEmitAngle - keyframe.minEmitAngle) * Math.random();
                rad = (angle / 180) * Math.PI;
                animation.node.setVelocity([speed * Math.cos(rad), speed * Math.sin(rad)]);
              }
              return roga.animation.animationManager.start(animation);
            } else {
              if (keyframe.id) {
                this._sprite.updateTransform();
                animation = roga.animation.animationManager.CreateAnimation(enchant.loader.getAnimation(keyframe.id), true, null, 1, 0.5, this._target);
                this._sprite.addChild(animation.node);
                this._interval = animation.interval;
                return this._interval.start();
              }
            }
          }
        }
      };

      SourceInterval.prototype.reset = function() {
        this._frameNo = 0;
        this._index = 0;
        this._frameDuration = 0;
        if (this._interval) return this._interval.reset();
      };

      SourceInterval.prototype.start = function() {
        var keyframe;
        keyframe = this._sourceKeykeyframes[0];
        return this._updateKeyframe(keyframe);
      };

      SourceInterval.prototype.finish = function() {
        return this._clearSetting();
      };

      SourceInterval.prototype.update = function() {
        var keyframe;
        if (this.isDone()) {
          return this._clearSetting();
        } else {
          this._frameDuration++;
          this._frameNo++;
          keyframe = this._sourceKeykeyframes[this._index];
          this._updateKeyframe(keyframe);
          if (this._frameDuration >= keyframe.duration) {
            this._index++;
            return this._frameDuration = 0;
          }
        }
      };

      return SourceInterval;

    })();
    this.Loop = (function() {

      function Loop(interval, loopCount) {
        this._loopCounter = 0;
        this._loopCount = loopCount;
        this._interval = interval;
      }

      Loop.prototype.isDone = function() {
        var _isDone;
        _isDone = false;
        if (this._loopCount === 0) {
          _isDone = false;
        } else {
          _isDone = this._interval.isDone() && this._loopCounter >= this._loopCount - 1;
        }
        return _isDone;
      };

      Loop.prototype.reset = function() {
        this._loopCounter = 0;
        return this._interval.reset();
      };

      Loop.prototype.start = function() {
        return this._interval.start();
      };

      Loop.prototype.finish = function() {
        return this._interval.finish();
      };

      Loop.prototype.update = function() {
        if (!this.isDone()) {
          this._interval.update();
          if (this._interval.isDone()) {
            this._loopCounter++;
            if (this._loopCount === 0 || this._loopCounter < this._loopCount) {
              return this._interval.reset();
            }
          }
        }
      };

      return Loop;

    })();
    this.Sequence = (function() {

      function Sequence(intervals) {
        var length;
        this._intervals = intervals;
        this._index = 0;
        length = this._intervals.length;
        this._lastInterval = this._intervals[length - 1];
      }

      Sequence.prototype.isDone = function() {
        return this._lastInterval.isDone();
      };

      Sequence.prototype.reset = function() {
        var i;
        this._index = 0;
        for (i in this._intervals) {
          this._intervals[i].reset();
        }
        return this.start();
      };

      Sequence.prototype.start = function() {
        return this._intervals[0].start();
      };

      Sequence.prototype.finish = function() {
        return this._intervals[this._index].finish();
      };

      Sequence.prototype.update = function() {
        var currentInterval;
        if (!this.isDone()) {
          currentInterval = this._intervals[this._index];
          currentInterval.update();
          if (this.isDone()) {
            return this.finish();
          } else {
            if (currentInterval.isDone()) return this._index++;
          }
        }
      };

      return Sequence;

    })();
    return this.Parallel = (function() {

      function Parallel(intervals) {
        this._intervals = intervals;
      }

      Parallel.prototype.isDone = function() {
        var i, isDone;
        isDone = true;
        for (i in this._intervals) {
          if (!this._intervals[i].isDone()) {
            isDone = false;
            break;
          }
        }
        return isDone;
      };

      Parallel.prototype.reset = function() {
        var i;
        for (i in this._intervals) {
          this._intervals[i].reset();
        }
        return this.start();
      };

      Parallel.prototype.start = function() {
        var i, _results;
        _results = [];
        for (i in this._intervals) {
          _results.push(this._intervals[i].start());
        }
        return _results;
      };

      Parallel.prototype.finish = function() {
        var i, _results;
        _results = [];
        for (i in this._intervals) {
          _results.push(this._intervals[i].finish());
        }
        return _results;
      };

      Parallel.prototype.update = function() {
        var i;
        if (!this.isDone()) {
          for (i in this._intervals) {
            this._intervals[i].update();
          }
          if (this.isDone()) return this.finish();
        }
      };

      return Parallel;

    })();
  });