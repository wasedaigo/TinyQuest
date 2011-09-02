function testRound(value) {
	if (typeof(value) == "object") {
		for (var i in value) {
			value[i] *= 100;
			value[i] = Math.round(value[i]);
			value[i] /= 100;
		}

	} else {
		value *= 100;
		value = Math.round(value);
		value /= 100;
	}
	return value;
}

function testFrame(root, interval, testData, i, j) {
    expect(root._children[j].position).toEqual(testData[j].position[i]);
    expect(testRound(root._children[j].scale)).toEqual(testData[j].scale[i]);
    expect(testRound(root._children[j].alpha)).toEqual(testData[j].alpha[i]);
    expect(root._children[j].hue).toEqual(testData[j].hue[i]);
    expect(testRound(root._children[j].rotation)).toEqual(testData[j].rotation[i]);

    if (root._children[j].srcRect) {
        expect(testRound(root._children[j].srcRect)).toEqual(testData[j].srcRect[i]);
    } else {
        expect(root._children[j].srcRect).toEqual(null);
    }
    expect(root._children[j].srcPath).toEqual(testData[j].srcPath[i]);
    expect(testRound(root._children[j].center)).toEqual(testData[j].center[i]);
    expect(interval.isDone()).toBe(testData[j].isDone[i]);
}

enchant.loader.setRootPath("");

enchant.loader.setAnimation("Smoke01", 
    {"dependencies":{"animations":[],"images":["test"]},"timelines":[{"position":[],"scale":[{"duration":5,"endValue":[1.10000002384186,1.10000002384186],"tween":true,"startValue":[0.5,0.5],"frameNo":0},{"duration":9,"endValue":[1.5,1.5],"tween":true,"startValue":[1.10000002384186,1.10000002384186],"frameNo":5},{"duration":1,"endValue":[1.5,1.5],"startValue":[1.5,1.5],"frameNo":14}],"hue":[],"source":[{"duration":15,"center":[0,0],"id":"test","type":"image","frameNo":0,"rect":[0,0,32,32],"relative":false}],"rotation":[{"duration":14,"endValue":30,"tween":true,"startValue":0,"frameNo":0},{"duration":1,"endValue":30,"startValue":30,"frameNo":14}],"alpha":[{"duration":14,"endValue":0.0,"tween":true,"startValue":1.0,"frameNo":0},{"duration":1,"endValue":0.0,"startValue":0.0,"frameNo":14}]}]}
);
enchant.loader.setAnimation("SmokeRing", 
    {"dependencies":{"animations":["Smoke01"],"images":[]},"timelines":[{"position":[{"duration":4,"endValue":[22,22],"tween":true,"startValue":[0,0],"frameNo":0},{"duration":11,"tween":false,"endValue":[22,22],"startValue":[22,22],"frameNo":4}],"scale":[],"hue":[],"source":[{"duration":4,"emitter":false,"id":"Smoke01","type":"animation","frameNo":0,"relative":false},{"duration":11,"emitter":false,"id":"Smoke01","type":"animation","frameNo":4,"relative":false}],"rotation":[],"alpha":[]},{"position":[{"duration":4,"endValue":[-22,22],"tween":true,"startValue":[0,0],"frameNo":0},{"duration":11,"tween":false,"endValue":[-22,22],"startValue":[-22,22],"frameNo":4}],"scale":[],"hue":[],"source":[{"duration":4,"emitter":false,"id":"Smoke01","type":"animation","frameNo":0,"relative":false},{"duration":11,"emitter":false,"id":"Smoke01","type":"animation","frameNo":4,"relative":false}],"rotation":[],"alpha":[]},{"position":[{"duration":4,"endValue":[0,30],"tween":true,"startValue":[0,0],"frameNo":0},{"duration":11,"tween":false,"endValue":[0,30],"startValue":[0,30],"frameNo":4}],"scale":[],"hue":[],"source":[{"duration":4,"emitter":false,"id":"Smoke01","type":"animation","frameNo":0,"relative":false},{"duration":11,"emitter":false,"id":"Smoke01","type":"animation","frameNo":4,"relative":false}],"rotation":[],"alpha":[]},{"position":[{"duration":4,"endValue":[0,-30],"tween":true,"startValue":[0,0],"frameNo":0},{"duration":11,"tween":false,"endValue":[0,-30],"startValue":[0,-30],"frameNo":4}],"scale":[],"hue":[],"source":[{"duration":4,"emitter":false,"id":"Smoke01","type":"animation","frameNo":0,"relative":false},{"duration":11,"emitter":false,"id":"Smoke01","type":"animation","frameNo":4,"relative":false}],"rotation":[],"alpha":[]},{"position":[{"duration":4,"endValue":[22,-22],"tween":true,"startValue":[0,0],"frameNo":0},{"duration":11,"tween":false,"endValue":[22,-22],"startValue":[22,-22],"frameNo":4}],"scale":[],"hue":[],"source":[{"duration":4,"emitter":false,"id":"Smoke01","type":"animation","frameNo":0,"relative":false},{"duration":11,"emitter":false,"id":"Smoke01","type":"animation","frameNo":4,"relative":false}],"rotation":[],"alpha":[]},{"position":[{"duration":4,"endValue":[30,0],"tween":true,"startValue":[0,0],"frameNo":0},{"duration":11,"tween":false,"endValue":[30,0],"startValue":[30,0],"frameNo":4}],"scale":[],"hue":[],"source":[{"duration":4,"emitter":false,"id":"Smoke01","type":"animation","frameNo":0,"relative":false},{"duration":11,"emitter":false,"id":"Smoke01","type":"animation","frameNo":4,"relative":false}],"rotation":[],"alpha":[]},{"position":[{"duration":4,"endValue":[-30,0],"tween":true,"startValue":[0,0],"frameNo":0},{"duration":11,"tween":false,"endValue":[-30,0],"startValue":[-30,0],"frameNo":4}],"scale":[],"hue":[],"source":[{"duration":4,"emitter":false,"id":"Smoke01","type":"animation","frameNo":0,"relative":false},{"duration":11,"emitter":false,"id":"Smoke01","type":"animation","frameNo":4,"relative":false}],"rotation":[],"alpha":[]},{"position":[{"duration":4,"endValue":[-22,-22],"tween":true,"startValue":[0,0],"frameNo":0},{"duration":11,"tween":false,"endValue":[-22,-22],"startValue":[-22,-22],"frameNo":4}],"scale":[],"hue":[],"source":[{"duration":4,"emitter":false,"id":"Smoke01","type":"animation","frameNo":0,"relative":false},{"duration":11,"emitter":false,"id":"Smoke01","type":"animation","frameNo":4,"relative":false}],"rotation":[],"alpha":[]}]}
);
enchant.loader.setAnimation("SingleSpinAxe", 
    {"dependencies":{"animations":[],"images":["Battle/Skills/Weapon_AxeBase"]},"timelines":[{"position":[{"duration":7,"endValue":[0,0],"startValue":[0,0],"frameNo":0},{"duration":7,"endValue":[0,0],"startValue":[0,0],"frameNo":7}],"scale":[],"hue":[],"source":[{"duration":7,"center":[0,0],"id":"Battle/Skills/Weapon_AxeBase","type":"image","frameNo":0,"rect":[0,0,32,32],"relative":false}],"rotation":[{"duration":7,"endValue":315,"tween":true,"startValue":0,"frameNo":0},{"duration":7,"endValue":315,"startValue":315,"frameNo":7}],"alpha":[]}]}
);
enchant.loader.setAnimation("CycloneAxe", 
    {"dependencies":{"animations":["Battle/Skills/Battler/SmallJump","Battle/Skills/Axe/SpinAxeY"],"images":["Battle/Skills/Battler_Base","Battle/Skills/Weapon_AxeBase"]},"timelines":[{"position":[{"duration":2,"endValue":[0,0],"startValue":[0,0],"frameNo":0},{"duration":10,"endValue":[-16,0],"tween":true,"startValue":[0,0],"frameNo":2},{"duration":5,"endValue":[-16,0],"startValue":[-16,0],"frameNo":12},{"duration":10,"endValue":[0,0],"tween":true,"startValue":[-16,0],"frameNo":17},{"duration":19,"endValue":[0,0],"startValue":[0,0],"frameNo":27}],"scale":[],"hue":[],"source":[{"duration":28,"emitter":false,"id":"Battle/Skills/Battler/SmallJump","type":"animation","frameNo":0,"relative":false},{"duration":18,"center":[0,0],"id":"Battle/Skills/Battler_Base","type":"image","frameNo":28,"rect":[288,0,32,32],"relative":false}],"rotation":[],"alpha":[]},{"position":[{"duration":7,"wait":true,"frameNo":0},{"duration":13,"endValue":[0,0],"tween":true,"startValue":[-8,-36],"frameNo":7},{"duration":21,"endValue":[0,0],"tween":true,"startValue":[0,0],"frameNo":20},{"duration":41,"endValue":[0,0],"tween":true,"startValue":[0,0],"frameNo":41}],"scale":[],"hue":[],"source":[{"duration":7,"wait":true,"frameNo":0},{"duration":41,"emitter":false,"id":"Battle/Skills/Axe/SpinAxeY","type":"animation","frameNo":7,"relative":false}],"rotation":[],"alpha":[]}]}
);


describe("Animation", function() {
    
    describe("TestInterval", function() {
        it("AlphaInterval", function() {
            var node = new enchant.canvas.Node();
            var interval = new enchant.animation.interval.Interval(node, "alpha", 0.0, 1.0, 5, true);

            expect(node.alpha).toBe(1);
            expect(interval.isDone()).toBe(false);
            
            interval.start();
            expect(node.alpha).toEqual(0.0);
            expect(interval.isDone()).toBe(false);
            
            interval.update();
            expect(node.alpha).toBe(0.2);
            expect(interval.isDone()).toBe(false);
            
            interval.update();
            expect(node.alpha).toBe(0.4);
            expect(interval.isDone()).toBe(false);
            
            interval.update();
            expect(node.alpha).toBe(0.6);
            expect(interval.isDone()).toBe(false);
            
            interval.update();
            expect(node.alpha).toBe(0.8);
            expect(interval.isDone()).toBe(false);
            
            interval.update();
            expect(node.alpha).toBe(1.0);
            expect(interval.isDone()).toBe(true);
            
            interval.update();
            expect(node.alpha).toBe(1.0);
            expect(interval.isDone()).toBe(true);
            
            interval.reset();
            expect(node.alpha).toEqual(0.0);
            expect(interval.isDone()).toBe(false);
        });

        it("PositionInterval", function() {
            var node = new enchant.canvas.Node();
            var interval = new enchant.animation.interval.Interval(node, "position", [10, 10], [2, 6], 4, true);

            expect(node.position).toEqual([0, 0]);
            expect(interval.isDone()).toBe(false);
            
            interval.start();
            expect(node.position).toEqual([10, 10]);
            expect(interval.isDone()).toBe(false);
            
            interval.update();
            expect(node.position).toEqual([8, 9]);
            expect(interval.isDone()).toBe(false);
            
            interval.update();
            expect(node.position).toEqual([6, 8]);
            expect(interval.isDone()).toBe(false);
            
            interval.update();
            expect(node.position).toEqual([4, 7]);
            expect(interval.isDone()).toBe(false);

            interval.update();
            expect(node.position).toEqual([2, 6]);
            expect(interval.isDone()).toBe(true);
            
            interval.reset();
            expect(node.position).toEqual([10, 10]);
            expect(interval.isDone()).toBe(false);
        }); 
        it("ScaleInterval", function() {
            var node = new enchant.canvas.Node();
            var interval = new enchant.animation.interval.Interval(node, "scale", [10, 10], [2, 6], 4, true);

            expect(node.scale).toEqual([1, 1]);
            expect(interval.isDone()).toBe(false);
            
            interval.start();
            expect(node.scale).toEqual([10, 10]);
            expect(interval.isDone()).toBe(false);
            
            interval.update();
            expect(node.scale).toEqual([8, 9]);
            expect(interval.isDone()).toBe(false);
            
            interval.update();
            expect(node.scale).toEqual([6, 8]);
            expect(interval.isDone()).toBe(false);
            
            interval.update();
            expect(node.scale).toEqual([4, 7]);
            expect(interval.isDone()).toBe(false);

            interval.update();
            expect(node.scale).toEqual([2, 6]);
            expect(interval.isDone()).toBe(true);
            
            interval.reset();
            expect(node.scale).toEqual([10, 10]);
            expect(interval.isDone()).toBe(false);
        });   

        it("HueInterval", function() {
            var node = new enchant.canvas.Node();
            var interval = new enchant.animation.interval.Interval(node, "hue", [10, 10, 20], [2, 6, 40], 4, true);

            expect(node.hue).toEqual([0, 0, 0]);
            expect(interval.isDone()).toBe(false);

            interval.start();
            expect(node.hue).toEqual([10, 10, 20]);
            expect(interval.isDone()).toBe(false);
            
            interval.update();
            expect(node.hue).toEqual([8, 9, 25]);
            expect(interval.isDone()).toBe(false);
            
            interval.update();
            expect(node.hue).toEqual([6, 8, 30]);
            expect(interval.isDone()).toBe(false);
            
            interval.update();
            expect(node.hue).toEqual([4, 7, 35]);
            expect(interval.isDone()).toBe(false);

            interval.update();
            expect(node.hue).toEqual([2, 6, 40]);
            expect(interval.isDone()).toBe(true);
            
            interval.reset();
            expect(node.hue).toEqual([10, 10, 20]);
            expect(interval.isDone()).toBe(false);
        });  
        
        it("RotationInterval", function() {
            var node = new enchant.canvas.Node();
            var interval = new enchant.animation.interval.Interval(node, "rotation", 360, 0, 3, true);

            expect(node.rotation).toBe(0);
            expect(interval.isDone()).toBe(false);

            interval.start();
            expect(node.rotation).toEqual(360);
            expect(interval.isDone()).toBe(false);
                        
            interval.update();
            expect(node.rotation).toBe(240);
            expect(interval.isDone()).toBe(false);
            
            interval.update();
            expect(node.rotation).toBe(120);
            expect(interval.isDone()).toBe(false);
            
            interval.update();
            expect(node.rotation).toBe(0);
            expect(interval.isDone()).toBe(true);
            
            interval.reset();
            expect(node.rotation).toEqual(360);
            expect(interval.isDone()).toBe(false);
        }); 
        
        it("Wait", function() {

            var interval = new enchant.animation.interval.Wait(5);

            expect(interval.isDone()).toBe(false);
            
            interval.update();
            expect(interval.isDone()).toBe(false);
            
            interval.update();
            expect(interval.isDone()).toBe(false);
            
            interval.update();
            expect(interval.isDone()).toBe(false);

            interval.update();
            expect(interval.isDone()).toBe(false);
            
            interval.update();
            expect(interval.isDone()).toBe(true);
           
            interval.reset();
            expect(interval.isDone()).toBe(false);
        });

        it("SourceInterval", function() {
            
            var sprite = new enchant.canvas.Sprite(null, [5, 14, 25, 30]);
            var keyframes = [
                {
                    "frameNo" : 0,
                    "rect" : [10, 10, 32, 48],
                    "id" : "test",
                    "duration" : 3,
                    "type" : "image",
                    "center" : [1, 1]
                },
                {
                    "frameNo" : 3,
                    "rect" : [20, 10, 22, 48],
                    "id" : "test2",
                    "duration" : 2,
                    "type" : "image",
                    "center" : [2, 2]
                },
                {
                    "frameNo" : 5,
                    "rect" : [20, 20, 22, 48],
                    "id" : "test3",
                    "duration" : 1,
                    "type" : "image",
                    "center" : [3, 3]
                },
                {
                    "frameNo" : 6,
                    "rect" : [20, 20, 22, 48],
                    "id" : "test4",
                    "duration" : 2,
                    "type" : "image",
                    "center" : [4, 4]
                }
            ];

            var interval = new enchant.animation.interval.SourceInterval(sprite, keyframes);

            expect(sprite.srcPath).toEqual(null);
            expect(interval.isDone()).toBe(false);

            interval.start();
            expect(sprite.srcPath).toEqual("test.png");
            expect(sprite.srcRect).toEqual([10, 10, 32, 48]);
            expect(sprite.center).toEqual([1, 1]);
            expect(interval.isDone()).toBe(false);
            
            interval.update();
            expect(sprite.srcPath).toEqual("test.png");
            expect(sprite.srcRect).toEqual([10, 10, 32, 48]);
            expect(sprite.center).toEqual([1, 1]);
            expect(interval.isDone()).toBe(false);
            
            interval.update();
            expect(sprite.srcPath).toEqual("test.png");
            expect(sprite.srcRect).toEqual([10, 10, 32, 48]);
            expect(sprite.center).toEqual([1, 1]);
            expect(interval.isDone()).toBe(false);
            
            interval.update();
            expect(sprite.srcPath).toEqual("test.png");
            expect(sprite.srcRect).toEqual([10, 10, 32, 48]);
            expect(sprite.center).toEqual([1, 1]);
            expect(interval.isDone()).toBe(false);

            interval.update();
            expect(sprite.srcPath).toEqual("test2.png");
            expect(sprite.srcRect).toEqual([20, 10, 22, 48]);
            expect(sprite.center).toEqual([2, 2]);
            expect(interval.isDone()).toBe(false);
            
            interval.update();
            expect(sprite.srcPath).toEqual("test2.png");
            expect(sprite.srcRect).toEqual([20, 10, 22, 48]);
            expect(sprite.center).toEqual([2, 2]);
            expect(interval.isDone()).toBe(false);
            
            interval.update();
            expect(sprite.srcPath).toEqual("test3.png");
            expect(sprite.srcRect).toEqual([20, 20, 22, 48]);
            expect(sprite.center).toEqual([3, 3]);
            expect(interval.isDone()).toBe(false);
            
            interval.update();
            expect(sprite.srcPath).toEqual("test4.png");
            expect(sprite.srcRect).toEqual([20, 20, 22, 48]);
            expect(sprite.center).toEqual([4, 4]);
            expect(interval.isDone()).toBe(false);
               
            interval.update();
            expect(sprite.srcPath).toEqual("test4.png");
            expect(sprite.srcRect).toEqual([20, 20, 22, 48]);
            expect(sprite.center).toEqual([4, 4]);
            expect(interval.isDone()).toBe(true);
        });
        it("Sequence1", function() {
            var node = new enchant.canvas.Node();
            var interval1 = new enchant.animation.interval.Interval(node, "alpha", 0.1, 1.0, 3, true);
            var interval2 = new enchant.animation.interval.Wait(2);
            var interval3 = new enchant.animation.interval.Interval(node, "alpha", 0.7, 0.0, 2, true);
            
            var sequence = new enchant.animation.interval.Sequence([interval1, interval2, interval3]);
            
            sequence.start();
            expect(node.alpha).toBe(0.1);
            expect(sequence.isDone()).toBe(false);
            
            sequence.update();
            expect(node.alpha).toBe(0.4);
            expect(sequence.isDone()).toBe(false);
            
            sequence.update();
            expect(node.alpha).toBe(0.7);
            expect(sequence.isDone()).toBe(false);
            
            sequence.update();
            expect(node.alpha).toBe(1.0);
            expect(sequence.isDone()).toBe(false);
            
            sequence.update();
            expect(node.alpha).toBe(1.0);
            expect(sequence.isDone()).toBe(false);
            
            sequence.update();
            expect(node.alpha).toBe(1.0);
            expect(sequence.isDone()).toBe(false);
            
            sequence.update();
            expect(node.alpha).toBe(0.35);
            expect(sequence.isDone()).toBe(false);
            
            sequence.update();
            expect(node.alpha).toBe(0.0);
            expect(sequence.isDone()).toBe(true);
            
            sequence.reset();
            expect(node.alpha).toBe(0.1);
            expect(sequence.isDone()).toBe(false);
            
        }); 
        it("Sequence2", function() {
            var node = new enchant.canvas.Node();
            var interval1 = new enchant.animation.interval.Interval(node, "alpha", 1, 0.5, 1, false);
            var interval2 = new enchant.animation.interval.Interval(node, "alpha", 0.5, 0.3, 1, false);
            var interval3 = new enchant.animation.interval.Interval(node, "alpha", 0.3, 0.1, 1, true);
            var interval4 = new enchant.animation.interval.Interval(node, "alpha", 0.1, 0.1, 1, true);

            var sequence = new enchant.animation.interval.Sequence([interval1, interval2, interval3, interval4]);
            
            sequence.start();
            expect(node.alpha).toBe(1);
            expect(sequence.isDone()).toBe(false);
            
            sequence.update();
            expect(node.alpha).toBe(0.5);
            expect(sequence.isDone()).toBe(false);
            
            sequence.update();
            expect(node.alpha).toBe(0.3);
            expect(sequence.isDone()).toBe(false);
   
            sequence.update();
            expect(node.alpha).toBe(0.1);
            expect(sequence.isDone()).toBe(false);    
                     
            sequence.update();
            expect(node.alpha).toBe(0.1);
            expect(sequence.isDone()).toBe(true);    
        }); 
        it("Parallel", function() {
            var node = new enchant.canvas.Node();
            var interval1 = new enchant.animation.interval.Interval(node, "alpha", 0.1, 1.0, 3, true);
            var interval2 = new enchant.animation.interval.Interval(node, "rotation", 0, 180, 5, true);
            
            var parallel = new enchant.animation.interval.Parallel([interval1, interval2]);
            
            parallel.start();
            expect(node.alpha).toBe(0.1);
            expect(node.rotation).toBe(0);
            expect(parallel.isDone()).toBe(false);
            
            parallel.update();
            expect(node.alpha).toBe(0.4);
            expect(node.rotation).toBe(36);
            expect(parallel.isDone()).toBe(false);
            
            parallel.update();
            expect(node.alpha).toBe(0.7);
            expect(node.rotation).toBe(72);
            expect(parallel.isDone()).toBe(false);
            
            parallel.update();
            expect(node.alpha).toBe(1.0);
            expect(node.rotation).toBe(108);
            expect(parallel.isDone()).toBe(false);
    
            parallel.update();
            expect(node.alpha).toBe(1.0);
            expect(node.rotation).toBe(144);
            expect(parallel.isDone()).toBe(false);
            
            parallel.update();
            expect(node.alpha).toBe(1.0);
            expect(node.rotation).toBe(180);
            expect(parallel.isDone()).toBe(true);
            
            parallel.reset();
            expect(node.alpha).toBe(0.1);
            expect(node.rotation).toBe(0);
            expect(parallel.isDone()).toBe(false);
        }); 
        it("Loop count 2", function() {
            var node = new enchant.canvas.Node();
            var interval1 = new enchant.animation.interval.Interval(node, "alpha", 0.1, 1.0, 1, true);
            var interval2 = new enchant.animation.interval.Wait(1);
            var interval3 = new enchant.animation.interval.Interval(node, "alpha", 0.7, 0.0, 1, true);
            
            var sequence = new enchant.animation.interval.Sequence([interval1, interval2, interval3]);
            var loop = new enchant.animation.interval.Loop(sequence, 2);
            loop.start();
            expect(node.alpha).toBe(0.1);
            expect(loop.isDone()).toBe(false);
            
            loop.update();
            expect(node.alpha).toBe(1.0);
            expect(loop.isDone()).toBe(false);
            
            loop.update();
            expect(node.alpha).toBe(1.0);
            expect(loop.isDone()).toBe(false);
            
            loop.update();
            expect(node.alpha).toBe(0.1);
            expect(loop.isDone()).toBe(false);
            
            loop.update();
            expect(node.alpha).toBe(1.0);
            expect(loop.isDone()).toBe(false);
            
            loop.update();
            expect(node.alpha).toBe(1.0);
            expect(loop.isDone()).toBe(false);
            
            loop.update();
            expect(node.alpha).toBe(0.0);
            expect(loop.isDone()).toBe(true);
        });
        it("Loop count infinite", function() {
            var node = new enchant.canvas.Node();
            var interval1 = new enchant.animation.interval.Interval(node, "alpha", 0.1, 1.0, 1, true);
            var interval2 = new enchant.animation.interval.Wait(1);
            var interval3 = new enchant.animation.interval.Interval(node, "alpha", 0.7, 0.0, 1, true);
            
            var sequence = new enchant.animation.interval.Sequence([interval1, interval2, interval3]);
            var loop = new enchant.animation.interval.Loop(sequence, 0);
            loop.start();
            expect(node.alpha).toBe(0.1);
            expect(loop.isDone()).toBe(false);
            
            for (var i = 0; i < 1; i++) {
                loop.update();
                expect(node.alpha).toBe(1.0);
                expect(loop.isDone()).toBe(false);
                
                loop.update();
                expect(node.alpha).toBe(1.0);
                expect(loop.isDone()).toBe(false);
                    
                loop.update();
                expect(node.alpha).toBe(0.1);
                expect(loop.isDone()).toBe(false);
            }
            
        });
        var animationTestResult = {
            "Smoke01" : [{
                
                    "position":[[0, 0], [0, 0], [0, 0], [0, 0], [0, 0], [0, 0], [0, 0], [0, 0], [0, 0], [0, 0], [0, 0], [0, 0], [0, 0], [0, 0], [0, 0], [0, 0]],
                    "scale":[[0.5, 0.5], [0.62, 0.62], [0.74, 0.74], [0.86, 0.86], [0.98, 0.98], [1.1, 1.1], [1.14, 1.14], [1.19, 1.19], [1.23, 1.23], [1.28, 1.28], [1.32, 1.32], [1.37, 1.37], [1.41, 1.41], [1.46, 1.46],[1.5, 1.5],[1.5, 1.5]],
                    "alpha":[1, 0.93, 0.86, 0.79, 0.71, 0.64, 0.57, 0.5, 0.43, 0.36, 0.29, 0.21, 0.14, 0.07, 0.0, 0.0],
                    "hue":[[0,0,0], [0,0,0], [0,0,0],[0,0,0],[0,0,0],[0,0,0],[0,0,0],[0,0,0],[0,0,0],[0,0,0],[0,0,0],[0,0,0],[0,0,0],[0,0,0],[0,0,0],[0,0,0]],
                    "rotation":[0, 2.14,4.29,6.43,8.57, 10.71, 12.86, 15, 17.14, 19.29, 21.43, 23.57, 25.71, 27.86, 30,  30],
                    "srcRect":[[0, 0, 32, 32], [0, 0, 32, 32], [0, 0, 32, 32],[0, 0, 32, 32],[0, 0, 32, 32],[0, 0, 32, 32],[0, 0, 32, 32],[0, 0, 32, 32],[0, 0, 32, 32],[0, 0, 32, 32],[0, 0, 32, 32],[0, 0, 32, 32],[0, 0, 32, 32],[0, 0, 32, 32],[0, 0, 32, 32],[0, 0, 32, 32]],
                    "srcPath":["test.png","test.png","test.png","test.png","test.png","test.png","test.png","test.png","test.png","test.png","test.png","test.png","test.png","test.png","test.png","test.png"],
                    "isDone": [false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true],
                    "center":[[0, 0], [0, 0], [0, 0], [0, 0], [0, 0], [0, 0], [0, 0], [0, 0], [0, 0], [0, 0], [0, 0], [0, 0], [0, 0], [0, 0], [0, 0], [0, 0]]
            }]
        }
        it("AnimationTest1", function() {
        	var root = new enchant.canvas.Node();

            var interval = enchant.animation.CreateAnimation(root, enchant.loader.getAnimation("Smoke01"), false);
            expect(root._children.length).toBe(1);

			interval.start();
			
            for (var i = 0; i < 16; i++) {
                if (i == 0) {
                    interval.start();
                } else {
                    interval.update();
                }

                testFrame(root, interval, animationTestResult["Smoke01"], i, 0);
            }
        }); 

        it("AnimationTest2", function() {
        	var root = new enchant.canvas.Node();
            var interval = enchant.animation.CreateAnimation(root, enchant.loader.getAnimation("SmokeRing"), false);
            expect(root._children.length).toBe(8);

			var frameCount = 16;
			var timelineCount = 8;
            var temp = [[22, 22], [-22, 22], [0, 30], [0, -30], [22, -22], [30, 0], [-30, 0], [-22, -22]];
 			var testData = [];
 			for (var j = 0; j < timelineCount; j++) {
                var t = {
                    "position":[],
                    "scale":[],
                    "alpha":[],
                    "hue":[],
                    "rotation":[],
                    "srcRect":[],
                    "srcPath":[],
                    "center":[],
                    "isDone": []
                }
                testData.push(t);
			}

			for (var i = 0; i < frameCount; i++) {
                for (var j = 0; j < timelineCount; j++) {
                    if (i < 4) {
                        testData[j]["position"].push([temp[j][0] * i / 4, temp[j][1] * i / 4]);
                    } else {
                        testData[j]["position"].push([temp[j][0], temp[j][1]]);
                    }

                    testData[j]["scale"].push([1, 1]);
                    testData[j]["alpha"].push(1);
                    testData[j]["hue"].push([0, 0, 0]);
                    testData[j]["center"].push([0, 0]);
                    testData[j]["rotation"].push(0);
                    testData[j]["srcPath"].push(null);
                    testData[j]["srcRect"].push(null);
                    testData[j]["isDone"].push(i == frameCount - 1);
                }
            }
            
            
			for (var i = 0; i < frameCount; i++) {
                if (i == 0) {
                    interval.start();
                } else {
                    interval.update();
                }
                for (var j = 0; j < timelineCount; j++) {
                    testFrame(root, interval, testData, i, j);
                }
            }

            for (var j = 0; j < timelineCount; j++) {
               expect(interval._intervals[j]._intervals[1]._interval.isDone()).toBe(false);
            }
            
        }); 
        
    });
});
var root = new enchant.canvas.Node();
var interval = enchant.animation.CreateAnimation(root, enchant.loader.getAnimation("Smoke01"), false);