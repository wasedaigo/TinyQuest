using UnityEngine;
using System.Collections.Generic;

namespace TinyQuest.Component {
	public class Player : Entity {
		public Player(string name) 
		: base("Characters/" + name, new Vector2(32, 32), new Vector2(0, 0), new Rect(128, 0, 32, 32))
		{
		}

		private Roga2dBaseInterval buildWalkInterval() {
			List<Roga2dAnimationKeyFrame> keyFrames = new List<Roga2dAnimationKeyFrame>();
			int[] frames = new int[4]{1, 0, 1, 2};
			for (int i = 0; i < 4; i++) {
				Roga2dAnimationKeyFrame keyFrame = Roga2dAnimationKeyFrame.Build();
				keyFrame.Rect = new Rect(96 + frames[i] * 32, 0, 32, 32);
				keyFrame.Id = this.TextureID;
				keyFrame.Duration = 10;
				keyFrame.Type = Roga2dAnimationKeyFrameType.Image;
				keyFrame.PixelCenter = new Vector2(0, 0);
				keyFrame.PixelSize = new Vector2(32, 32);
				keyFrames.Add(keyFrame);
			}

			return new Roga2dLoop(new Roga2dSourceInterval(this.Sprite, keyFrames, null, null), 0);
		}
	}
}