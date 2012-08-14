using UnityEngine;
using System.Collections.Generic;

namespace TinyQuest.Object {
	public class PuppetActor : Actor {
		private Roga2dIntervalPlayer intervalPlayer;
		private Roga2dBaseInterval interval;
		private PoseType poseType;
		private bool isWalking;
		private int hp;
		private int maxHp;

		public PuppetActor(string name, PoseType poseType) 
			:base(name)
		{
			this.SetSprite("Characters/" + name, new Vector2(32, 32), new Vector2(0, 0), new Rect(32, 0, 32, 32));
			this.intervalPlayer = new Roga2dIntervalPlayer();
			this.SetPoseType(poseType);
		}
		
		public override void SetPoseType(PoseType poseType) {
			this.poseType = poseType;
			switch (this.poseType) {
				case PoseType.Stand:
					this.SrcRect = new Rect(32, 0, 32, 32);
				break;
				case PoseType.Sit:
					this.SrcRect = new Rect(224, 0, 32, 32);
				break;
				case PoseType.Dying:
					this.SrcRect = new Rect(124, 0, 32, 32);
				break;
				case PoseType.Attack:
					this.SrcRect = new Rect(96, 0, 32, 32);
				break;
				case PoseType.Attacked:
					this.SrcRect = new Rect(156, 0, 32, 32);
				break;
				case PoseType.Dead:
					this.SrcRect = new Rect(188, 0, 32, 32);
				break;
			}
		}
		
		public override void ResetPose() {
			float ratio = this.hp / (float)this.maxHp;
			ActorHealthState state = Core.Utils.GetHealthState(ratio);
			switch (state) {
			case ActorHealthState.Full:
			case ActorHealthState.Ok:
				this.SetPoseType(PoseType.Stand);
				break;
			case ActorHealthState.Dying:
				this.SetPoseType(PoseType.Dying);
				break;
			case ActorHealthState.Dead:
				this.SetPoseType(PoseType.Dead);
				break;
			}
		}
		
		public override void SetStatus(int hp, int maxHp) {
			this.hp = hp;
			this.maxHp = maxHp;
			ResetPose();
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
		
		public override void startWalkingAnimation() {
			if (this.interval == null) {
				this.interval = this.buildWalkInterval();
				this.intervalPlayer.Play(this.interval);
				this.isWalking = true;
			}
		}
		
		public override void stopWalkingAnimation() {
			
			if (this.interval != null) {
				this.intervalPlayer.Stop(this.interval);
				this.interval = null;
				this.isWalking = false;
				this.SetPoseType(PoseType.Stand);
			}
		}
		public bool IsWalking() {
			return this.isWalking;	
		}
	}
}