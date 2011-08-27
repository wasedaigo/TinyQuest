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

function testFrame(root, interval, testData, i) {
    expect(root._children[0].position).toEqual(testData.position[i]);
    expect(testRound(root._children[0].scale)).toEqual(testData.scale[i]);
    expect(testRound(root._children[0].alpha)).toEqual(testData.alpha[i]);
    expect(root._children[0].hue).toEqual(testData.hue[i]);
    expect(testRound(root._children[0].rotation)).toEqual(testData.rotation[i]);
    expect(testRound(root._children[0].srcRect)).toEqual(testData.srcRect[i]);
    expect(root._children[0].srcPath).toEqual(testData.srcPath[i]);
    expect(interval.isDone()).toBe(testData.isDone[i]);
}

describe("Animation", function() {
    var animationTestData = {
        "Smoke01" : {"timelines":[{"position":[],"scale":[{"duration":5,"endValue":[1.10000002384186,1.10000002384186],"tween":true,"startValue":[0.5,0.5],"frameNo":0},{"duration":9,"endValue":[1.5,1.5],"tween":true,"startValue":[1.10000002384186,1.10000002384186],"frameNo":5},{"duration":1,"tween":false,"endValue":[1.5,1.5],"startValue":[1.5,1.5],"frameNo":14}],"hue":[],"source":[{"duration":15,"anchor":[0,0],"id":"test","type":"image","frameNo":0,"rect":[0,0,32,32],"relative":false}],"rotation":[{"duration":14,"endValue":30,"tween":true,"startValue":0,"frameNo":0},{"duration":1,"tween":false,"endValue":30,"startValue":30,"frameNo":14}],"alpha":[{"duration":14,"endValue":0.0,"tween":true,"startValue":1.0,"frameNo":0},{"duration":1,"tween":false,"endValue":0.0,"startValue":0.0,"frameNo":14}]}]},
        "SmokeRing" : {"timelines":[{"position":[{"duration":4,"endValue":[22,22],"tween":true,"startValue":[0,0],"frameNo":0},{"duration":11,"tween":false,"endValue":[22,22],"startValue":[22,22],"frameNo":4}],"scale":[],"hue":[],"source":[{"duration":4,"emitter":false,"id":"GameData/Animations/Battle/Skills/Effect/Smoke01","type":"animation","frameNo":0,"relative":false},{"duration":11,"emitter":false,"id":"GameData/Animations/Battle/Skills/Effect/Smoke01","type":"animation","frameNo":4,"relative":false}],"rotation":[],"alpha":[]},{"position":[{"duration":4,"endValue":[-22,22],"tween":true,"startValue":[0,0],"frameNo":0},{"duration":11,"tween":false,"endValue":[-22,22],"startValue":[-22,22],"frameNo":4}],"scale":[],"hue":[],"source":[{"duration":4,"emitter":false,"id":"GameData/Animations/Battle/Skills/Effect/Smoke01","type":"animation","frameNo":0,"relative":false},{"duration":11,"emitter":false,"id":"GameData/Animations/Battle/Skills/Effect/Smoke01","type":"animation","frameNo":4,"relative":false}],"rotation":[],"alpha":[]},{"position":[{"duration":4,"endValue":[0,30],"tween":true,"startValue":[0,0],"frameNo":0},{"duration":11,"tween":false,"endValue":[0,30],"startValue":[0,30],"frameNo":4}],"scale":[{"duration":11,"tween":false,"endValue":[1,1],"startValue":[1,1],"frameNo":4}],"hue":[],"source":[{"duration":4,"emitter":false,"id":"GameData/Animations/Battle/Skills/Effect/Smoke01","type":"animation","frameNo":0,"relative":false},{"duration":11,"emitter":false,"id":"GameData/Animations/Battle/Skills/Effect/Smoke01","type":"animation","frameNo":4,"relative":false}],"rotation":[],"alpha":[]},{"position":[{"duration":4,"endValue":[0,-30],"tween":true,"startValue":[0,0],"frameNo":0},{"duration":11,"tween":false,"endValue":[0,-30],"startValue":[0,-30],"frameNo":4}],"scale":[{"duration":11,"tween":false,"endValue":[1,1],"startValue":[1,1],"frameNo":4}],"hue":[],"source":[{"duration":4,"emitter":false,"id":"GameData/Animations/Battle/Skills/Effect/Smoke01","type":"animation","frameNo":0,"relative":false},{"duration":11,"emitter":false,"id":"GameData/Animations/Battle/Skills/Effect/Smoke01","type":"animation","frameNo":4,"relative":false}],"rotation":[],"alpha":[]},{"position":[{"duration":4,"endValue":[22,-22],"tween":true,"startValue":[0,0],"frameNo":0},{"duration":11,"tween":false,"endValue":[22,-22],"startValue":[22,-22],"frameNo":4}],"scale":[{"duration":11,"tween":false,"endValue":[1,1],"startValue":[1,1],"frameNo":4}],"hue":[],"source":[{"duration":4,"emitter":false,"id":"GameData/Animations/Battle/Skills/Effect/Smoke01","type":"animation","frameNo":0,"relative":false},{"duration":11,"emitter":false,"id":"GameData/Animations/Battle/Skills/Effect/Smoke01","type":"animation","frameNo":4,"relative":false}],"rotation":[],"alpha":[]},{"position":[{"duration":4,"endValue":[30,0],"tween":true,"startValue":[0,0],"frameNo":0},{"duration":11,"tween":false,"endValue":[30,0],"startValue":[30,0],"frameNo":4}],"scale":[],"hue":[],"source":[{"duration":4,"emitter":false,"id":"GameData/Animations/Battle/Skills/Effect/Smoke01","type":"animation","frameNo":0,"relative":false},{"duration":11,"emitter":false,"id":"GameData/Animations/Battle/Skills/Effect/Smoke01","type":"animation","frameNo":4,"relative":false}],"rotation":[],"alpha":[]},{"position":[{"duration":4,"endValue":[-30,0],"tween":true,"startValue":[0,0],"frameNo":0},{"duration":11,"tween":false,"endValue":[-30,0],"startValue":[-30,0],"frameNo":4}],"scale":[],"hue":[],"source":[{"duration":4,"emitter":false,"id":"GameData/Animations/Battle/Skills/Effect/Smoke01","type":"animation","frameNo":0,"relative":false},{"duration":11,"emitter":false,"id":"GameData/Animations/Battle/Skills/Effect/Smoke01","type":"animation","frameNo":4,"relative":false}],"rotation":[],"alpha":[]},{"position":[{"duration":4,"endValue":[-22,-22],"tween":true,"startValue":[0,0],"frameNo":0},{"duration":11,"tween":false,"endValue":[-22,-22],"startValue":[-22,-22],"frameNo":4}],"scale":[],"hue":[],"source":[{"duration":4,"emitter":false,"id":"GameData/Animations/Battle/Skills/Effect/Smoke01","type":"animation","frameNo":0,"relative":false},{"duration":11,"emitter":false,"id":"GameData/Animations/Battle/Skills/Effect/Smoke01","type":"animation","frameNo":4,"relative":false}],"rotation":[],"alpha":[]}]}
    };
    describe("TestInterval", function() {
        it("AlphaInterval", function() {
            var node = new enchant.canvas.Node();
            var interval = new enchant.animation.interval.Interval(node, "alpha", 0.0, 1.0, 5);

            expect(node.alpha).toBe(0);
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
        });
        it("PositionInterval", function() {
            var node = new enchant.canvas.Node();
            var interval = new enchant.animation.interval.Interval(node, "position", [10, 10], [2, 6], 4);

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
        }); 
        it("ScaleInterval", function() {
            var node = new enchant.canvas.Node();
            var interval = new enchant.animation.interval.Interval(node, "scale", [10, 10], [2, 6], 4);

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
        });   

        it("HueInterval", function() {
            var node = new enchant.canvas.Node();
            var interval = new enchant.animation.interval.Interval(node, "hue", [10, 10, 20], [2, 6, 40], 4);

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
        });  
        
        it("RotationInterval", function() {
            var node = new enchant.canvas.Node();
            var interval = new enchant.animation.interval.Interval(node, "rotation", 360, 0, 3);

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
        });
        
        it("SourceInterval", function() {
            
            var sprite = new enchant.canvas.Sprite("", [10, 20], [5, 14, 25, 30]);
            var keyframes = [
                {
                    "frameNo" : 0,
                    "rect" : [10, 10, 32, 48],
                    "id" : "test",
                    "duration" : 3
                },
                {
                    "frameNo" : 3,
                    "rect" : [20, 10, 22, 48],
                    "id" : "test2",
                    "duration" : 2
                },
                {
                    "frameNo" : 5,
                    "rect" : [20, 20, 22, 48],
                    "id" : "test3",
                    "duration" : 1
                },
                {
                    "frameNo" : 6,
                    "rect" : [20, 20, 22, 48],
                    "id" : "test4",
                    "duration" : 2
                }
            ];

            var interval = new enchant.animation.interval.SourceInterval(sprite, keyframes);

            expect(sprite.srcPath).toEqual("");
            expect(interval.isDone()).toBe(false);

            interval.start();
            expect(sprite.srcPath).toEqual("test.png");
            expect(sprite.srcRect).toEqual([10, 10, 32, 48]);
            expect(interval.isDone()).toBe(false);
            
            interval.update();
            expect(sprite.srcPath).toEqual("test.png");
            expect(sprite.srcRect).toEqual([10, 10, 32, 48]);
            expect(interval.isDone()).toBe(false);
            
            interval.update();
            expect(sprite.srcPath).toEqual("test.png");
            expect(sprite.srcRect).toEqual([10, 10, 32, 48]);
            expect(interval.isDone()).toBe(false);
            
            interval.update();
            expect(sprite.srcPath).toEqual("test.png");
            expect(sprite.srcRect).toEqual([10, 10, 32, 48]);
            expect(interval.isDone()).toBe(false);

            interval.update();
            expect(sprite.srcPath).toEqual("test2.png");
            expect(sprite.srcRect).toEqual([20, 10, 22, 48]);
            expect(interval.isDone()).toBe(false);
            
            interval.update();
            expect(sprite.srcPath).toEqual("test2.png");
            expect(sprite.srcRect).toEqual([20, 10, 22, 48]);
            expect(interval.isDone()).toBe(false);
            
            interval.update();
            expect(sprite.srcPath).toEqual("test3.png");
            expect(sprite.srcRect).toEqual([20, 20, 22, 48]);
            expect(interval.isDone()).toBe(false);
            
            interval.update();
            expect(sprite.srcPath).toEqual("test4.png");
            expect(sprite.srcRect).toEqual([20, 20, 22, 48]);
            expect(interval.isDone()).toBe(false);
               
            interval.update();
            expect(sprite.srcPath).toEqual("test4.png");
            expect(sprite.srcRect).toEqual([20, 20, 22, 48]);
            expect(interval.isDone()).toBe(true);
        });
        
        it("Sequence", function() {
            var node = new enchant.canvas.Node();
            var interval1 = new enchant.animation.interval.Interval(node, "alpha", 0.1, 1.0, 3);
            var interval2 = new enchant.animation.interval.Wait(2);
            var interval3 = new enchant.animation.interval.Interval(node, "alpha", 0.7, 0.0, 2);
            
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
        }); 
         
        it("Parallel", function() {
            var node = new enchant.canvas.Node();
            var interval1 = new enchant.animation.interval.Interval(node, "alpha", 0.1, 1.0, 3);
            var interval2 = new enchant.animation.interval.Interval(node, "rotation", 0, 180, 5);
            
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
        });  
        
         
        it("AnimationTest1", function() {
        	var root = new enchant.canvas.Node();

            var interval = enchant.animation.loader.CreateAnimation(root, animationTestData["Smoke01"]);
            expect(root._children.length).toBe(1);

			var testData = {
				"position":[[0, 0], [0, 0], [0, 0], [0, 0], [0, 0], [0, 0], [0, 0], [0, 0], [0, 0], [0, 0], [0, 0], [0, 0], [0, 0], [0, 0], [0, 0], [0, 0]],
				"scale":[[0.5, 0.5], [0.62, 0.62], [0.74, 0.74], [0.86, 0.86], [0.98, 0.98], [1.1, 1.1], [1.14, 1.14], [1.19, 1.19], [1.23, 1.23], [1.28, 1.28], [1.32, 1.32], [1.37, 1.37], [1.41, 1.41], [1.46, 1.46],[1.5, 1.5],[1.5, 1.5]],
				"alpha":[1, 0.93, 0.86, 0.79, 0.71, 0.64, 0.57, 0.5, 0.43, 0.36, 0.29, 0.21, 0.14, 0.07, 0.0, 0.0],
				"hue":[[0,0,0], [0,0,0], [0,0,0],[0,0,0],[0,0,0],[0,0,0],[0,0,0],[0,0,0],[0,0,0],[0,0,0],[0,0,0],[0,0,0],[0,0,0],[0,0,0],[0,0,0],[0,0,0]],
				"rotation":[0, 2.14,4.29,6.43,8.57, 10.71, 12.86, 15, 17.14, 19.29, 21.43, 23.57, 25.71, 27.86, 30,  30],
				"srcRect":[[0, 0, 32, 32], [0, 0, 32, 32], [0, 0, 32, 32],[0, 0, 32, 32],[0, 0, 32, 32],[0, 0, 32, 32],[0, 0, 32, 32],[0, 0, 32, 32],[0, 0, 32, 32],[0, 0, 32, 32],[0, 0, 32, 32],[0, 0, 32, 32],[0, 0, 32, 32],[0, 0, 32, 32],[0, 0, 32, 32],[0, 0, 32, 32]],
				"srcPath":["test.png","test.png","test.png","test.png","test.png","test.png","test.png","test.png","test.png","test.png","test.png","test.png","test.png","test.png","test.png","test.png"],
				"isDone": [false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true]
			}

			interval.start();
			
            for (var i = 0; i <= 15; i++) {
                if (i == 0) {
                    interval.start();
                } else {
                    interval.update();
                }
                testFrame(root, interval, testData, i);
            }
        }); 

        it("AnimationTest2", function() {
        	var root = new enchant.canvas.Node();
            var interval = enchant.animation.loader.CreateAnimation(root, animationTestData["Smoke01"]);
            expect(root._children.length).toBe(1);

			var testData = {
				"position":[[0, 0], [0, 0], [0, 0], [0, 0], [0, 0], [0, 0], [0, 0], [0, 0], [0, 0], [0, 0], [0, 0], [0, 0], [0, 0], [0, 0], [0, 0], [0, 0]],
				"scale":[[0.5, 0.5], [0.62, 0.62], [0.74, 0.74], [0.86, 0.86], [0.98, 0.98], [1.1, 1.1], [1.14, 1.14], [1.19, 1.19], [1.23, 1.23], [1.28, 1.28], [1.32, 1.32], [1.37, 1.37], [1.41, 1.41], [1.46, 1.46],[1.5, 1.5],[1.5, 1.5]],
				"alpha":[1, 0.93, 0.86, 0.79, 0.71, 0.64, 0.57, 0.5, 0.43, 0.36, 0.29, 0.21, 0.14, 0.07, 0.0, 0.0],
				"hue":[[0,0,0], [0,0,0], [0,0,0],[0,0,0],[0,0,0],[0,0,0],[0,0,0],[0,0,0],[0,0,0],[0,0,0],[0,0,0],[0,0,0],[0,0,0],[0,0,0],[0,0,0],[0,0,0]],
				"rotation":[0, 2.14,4.29,6.43,8.57, 10.71, 12.86, 15, 17.14, 19.29, 21.43, 23.57, 25.71, 27.86, 30,  30],
				"srcRect":[[0, 0, 32, 32], [0, 0, 32, 32], [0, 0, 32, 32],[0, 0, 32, 32],[0, 0, 32, 32],[0, 0, 32, 32],[0, 0, 32, 32],[0, 0, 32, 32],[0, 0, 32, 32],[0, 0, 32, 32],[0, 0, 32, 32],[0, 0, 32, 32],[0, 0, 32, 32],[0, 0, 32, 32],[0, 0, 32, 32],[0, 0, 32, 32]],
				"srcPath":["test.png","test.png","test.png","test.png","test.png","test.png","test.png","test.png","test.png","test.png","test.png","test.png","test.png","test.png","test.png","test.png"],
				"isDone": [false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, true]
			}

			interval.start();
            for (var i = 0; i <= 15; i++) {
                if (i == 0) {
                    interval.start();
                } else {
                    interval.update();
                }
                testFrame(root, interval, testData, i);
            }
        }); 
    });
});