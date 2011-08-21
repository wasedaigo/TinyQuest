describe("Animation", function() {

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
        
        it("SourceInterval", function() {
            
            var sprite = new enchant.canvas.Sprite("", [10, 20], [5, 14, 25, 30]);
            var keyframes = [
                {
                    "frameNo" : 0,
                    "rect" : [10, 10, 32, 48],
                    "path" : "test.png",
                    "duration" : 3
                },
                {
                    "frameNo" : 3,
                    "rect" : [20, 10, 22, 48],
                    "path" : "test2.png",
                    "duration" : 2
                },
                {
                    "frameNo" : 5,
                    "rect" : [20, 20, 22, 48],
                    "path" : "test3.png",
                    "duration" : 1
                },
                {
                    "frameNo" : 6,
                    "rect" : [20, 20, 22, 48],
                    "path" : "test4.png",
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
            var interval2 = new enchant.animation.interval.Interval(node, "alpha", 0.7, 0.0, 2);
            
            var sequence = new enchant.animation.interval.Sequence([interval1, interval2]);
            
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
    });
});