using UnityEngine;
using System.Collections.Generic;

namespace TinyQuest.Object {
	public class Ally : AdventureObject {
		private Roga2dIntervalPlayer intervalPlayer;
		private Roga2dBaseInterval interval;
		private State state;
		private bool isWalking;
		public enum State {
			Stand,
			Walk,
			Sit
		};

		public Ally(string name, State state) 
		: base("Characters/" + name, new Vector2(32, 32), new Vector2(0, 0), new Rect(32, 0, 32, 32))
		{
			this.intervalPlayer = new Roga2dIntervalPlayer();
			this.SetState(state);
		}
		
		public void SetState(State state) {
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
		
		public override void Update ()
		{
			base.Update ();
			this.intervalPlayer.Update();
		}
		
		private Roga2dBaseInterval buildWalkInterval() {
			List<Roga2dAnimationKeyFrame> keyFrames = new List<Roga2dAnimationKeyFrame>();
			int[] frames = new int[4]{0, 1, 2, 1};
			for (int i = 0; i < 4; i++) {
				Roga2dAnimationKeyFrame keyFrame = new Roga2dAnimationKeyFrame();
				keyFrame.Rect = new Rect(frames[i] * 32, 0, 32, 32);
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
			if (this.interval == null) {
				this.interval = this.buildWalkInterval();
				this.intervalPlayer.Play(this.interval);
				this.isWalking = true;
			}
		}
		
		public void stopWalkingAnimation() {
			
			if (this.interval != null) {
				this.intervalPlayer.Stop(this.interval);
				this.interval = null;
				this.isWalking = false;
				this.SetState(State.Stand);
			}
		}
		public bool IsWalking() {
			return this.isWalking;	
		}
	}
}