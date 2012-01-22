using UnityEngine;
using System.Collections.Generic;

class TestRoga2dSourceInterval {
	
	public static void Test() {
		TestUpdate();
	}
	
	public static void TestUpdate() {
		Roga2dRenderObject renderObject = new Roga2dRenderObject(null, new Vector2(32, 48), new Vector2(1, 1), new Rect(10, 10, 32, 48));
        Roga2dSprite sprite = new Roga2dSprite(renderObject);
		Roga2dRoot root = new Roga2dRoot(null);
		
		List<Roga2dAnimationKeyFrame> keyFrames = new List<Roga2dAnimationKeyFrame>();
		
		Roga2dAnimationKeyFrame keyFrame = Roga2dAnimationKeyFrame.Build();
		keyFrame.FrameNo = 0;
		keyFrame.Rect = new Rect(10, 10, 32, 48);
		keyFrame.Id = "test";
		keyFrame.Duration = 3;
		keyFrame.Type = Roga2dAnimationKeyFrameType.Image;
		keyFrame.PixelCenter = new Vector2(1, 1);
		keyFrames.Add(keyFrame);
		
		keyFrame = Roga2dAnimationKeyFrame.Build();
		keyFrame.FrameNo = 3;
		keyFrame.Rect = new Rect(20, 10, 22, 48);
		keyFrame.Id = "test2";
		keyFrame.Duration = 2;
		keyFrame.Type = Roga2dAnimationKeyFrameType.Image;
		keyFrame.PixelCenter = new Vector2(2, 2);
		keyFrames.Add(keyFrame);
	
		keyFrame = Roga2dAnimationKeyFrame.Build();
		keyFrame.FrameNo = 5;
		keyFrame.Rect = new Rect(20, 10, 22, 48);
		keyFrame.Id = "test3";
		keyFrame.Duration = 1;
		keyFrame.Type = Roga2dAnimationKeyFrameType.Image;
		keyFrame.PixelCenter = new Vector2(3, 3);
		keyFrames.Add(keyFrame);
		
		keyFrame = Roga2dAnimationKeyFrame.Build();
		keyFrame.FrameNo = 6;
		keyFrame.Rect = new Rect(20, 10, 22, 48);
		keyFrame.Id = "test4";
		keyFrame.Duration = 2;
		keyFrame.Type = Roga2dAnimationKeyFrameType.Image;
		keyFrame.PixelCenter = new Vector2(4, 4);
		keyFrames.Add(keyFrame);

		Roga2dSourceInterval interval = new Roga2dSourceInterval(sprite, keyFrames, root, null);
	
		interval.Start();
		Tester.Match(sprite.RenderObject.SrcRect, new Rect(10, 10, 32, 48));
		Tester.Match(sprite.RenderObject.PixelCenter, new Vector2(1, 1));
		Tester.Ok(!interval.IsDone());
		
		interval.Update();
		Tester.Match(sprite.RenderObject.SrcRect, new Rect(10, 10, 32, 48));
		Tester.Match(sprite.RenderObject.PixelCenter, new Vector2(1, 1));
		Tester.Ok(!interval.IsDone());
		
		interval.Update();
		Tester.Match(sprite.RenderObject.SrcRect, new Rect(10, 10, 32, 48));
		Tester.Match(sprite.RenderObject.PixelCenter, new Vector2(1, 1));
		Tester.Ok(!interval.IsDone());
		
		interval.Update();
		Tester.Match(sprite.RenderObject.SrcRect, new Rect(10, 10, 32, 48));
		Tester.Match(sprite.RenderObject.PixelCenter, new Vector2(1, 1));
		Tester.Ok(!interval.IsDone());
		
		interval.Update();
		Tester.Match(sprite.RenderObject.SrcRect, new Rect(20, 10, 22, 48));
		Tester.Match(sprite.RenderObject.PixelCenter, new Vector2(2, 2));
		Tester.Ok(!interval.IsDone());
		
		interval.Update();
		Tester.Match(sprite.RenderObject.SrcRect, new Rect(20, 10, 22, 48));
		Tester.Match(sprite.RenderObject.PixelCenter, new Vector2(2, 2));
		Tester.Ok(!interval.IsDone());
		
		interval.Update();
		Tester.Match(sprite.RenderObject.SrcRect, new Rect(20, 10, 22, 48));
		Tester.Match(sprite.RenderObject.PixelCenter, new Vector2(3, 3));
		Tester.Ok(!interval.IsDone());
		
		interval.Update();
		Tester.Match(sprite.RenderObject.SrcRect, new Rect(20, 10, 22, 48));
		Tester.Match(sprite.RenderObject.PixelCenter, new Vector2(4, 4));
		Tester.Ok(!interval.IsDone());
		
		interval.Update();
		Tester.Match(sprite.RenderObject.SrcRect, new Rect(20, 10, 22, 48));
		Tester.Match(sprite.RenderObject.PixelCenter, new Vector2(4, 4));
		Tester.Ok(interval.IsDone());
		interval.Update();

		sprite.Destroy();
	}
}