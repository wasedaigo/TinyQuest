using UnityEngine;
using System.Collections.Generic;

namespace TinyQuest.Object {
	public class Ally : AdventureObject {
		private Roga2dBaseInterval interval;
		private State state;
		public enum State {
			Stand,
			Walk,
			Sit
		};

		public Ally(string name, State state) 
		: base("Characters/" + name, new Vector2(32, 32), new Vector2(0, 0), new Rect(32, 0, 32, 32))
		{
			this.state = state;
			
			switch (this.state) {
				case State.Stand:
					this.SrcRect = new Rect(32, 0, 32, 32);
				break;
				case State.Sit:
					this.SrcRect = new Rect(224, 0, 32, 32);
				break;
			}
		}

		private Roga2dBaseInterval buildWalkInterval() {
			List<Roga2dAnimationKeyFrame> keyFrames = new List<Roga2dAnimationKeyFrame>();
			int[] frames = new int[4]{1, 0, 1, 2};
			for (int i = 0; i < 4; i++) {
				Roga2dAnimationKeyFrame keyFrame = new Roga2dAnimationKeyFrame();
				keyFrame.Rect = new Rect(96 + frames[i] * 32, 0, 32, 32);
				keyFrame.Id = this.TextureID;
				keyFrame.Duration = 0.15f;
				keyFrame.Type = Roga2dAnimationKeyFrameType.Image;
				keyFrame.PixelCenter = new Vector2(0, 0);
				keyFrame.PixelSize = new Vector2(32, 32);
				keyFrames.Add(keyFrame);
			}

			return new Roga2dLoop(new Roga2dSourceInterval(this.Sprite, keyFrames, null, null), 0);
		}
		
		public void startWalkingAnimation() {
			this.interval = this.buildWalkInterval();
			Roga2dIntervalPlayer.GetInstance().Play(this.interval);
		}
		
		public void stopWalkingAnimation() {
			
			if (this.interval != null) {
				Roga2dIntervalPlayer.GetInstance().Stop(this.interval);
				this.interval = null;
				this.sprite.RenderObject.SetSrcRect(new Rect(128, 0, 32, 32));
			}
		}
	}
}