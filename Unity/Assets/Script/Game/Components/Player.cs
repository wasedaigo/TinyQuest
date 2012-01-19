using UnityEngine;
using System.Collections.Generic;

namespace TinyQuest.Component {
	public class Player : Roga2dNode {
		private Roga2dRenderObject renderObject;
		private Roga2dLoop interval;
		public Player() 
		{
			this.renderObject = new Roga2dRenderObject(Roga2dResourceManager.getTexture("Battle/Skills/Battler_Base"), new Vector2(32, 32), new Vector2(0, 0), new Rect(128, 0, 32, 32));
			Roga2dSprite sprite = new Roga2dSprite(renderObject);
			this.AddChild(sprite);

			List<Roga2dAnimationKeyFrame> keyFrames = new List<Roga2dAnimationKeyFrame>();
			
			int[] frames = new int[4]{1, 0, 1, 2};
			for (int i = 0; i < 4; i++) {
				Roga2dAnimationKeyFrame keyFrame = Roga2dAnimationKeyFrame.Build();
				keyFrame.Rect = new Rect(96 + frames[i] * 32, 0, 32, 32);
				keyFrame.Id = "Battle/Skills/Battler_Base";
				keyFrame.Duration = 10;
				keyFrame.Type = Roga2dAnimationKeyFrameType.Image;
				keyFrame.PixelCenter = new Vector2(0, 0);
				keyFrame.PixelSize = new Vector2(32, 32);
				keyFrames.Add(keyFrame);
			}
				
			this.interval = new Roga2dLoop(new Roga2dSourceInterval(sprite, keyFrames, null), 0);
			
		}
		
		public override void Update() {
			base.Update();
			this.interval.Update();
		}
	}
}