using UnityEngine;
using System.Collections.Generic;

namespace TinyQuest.Component {
	public class Player : Roga2dNode {
		private Roga2dRenderObject renderObject;
		private Roga2dBaseInterval interval;
		private string textureId;
		private Roga2dSprite sprite;
		public Player(string name) 
		{
			this.textureId = "Characters/" + name;
			
			this.renderObject = new Roga2dRenderObject(Roga2dResourceManager.getTexture(this.textureId), new Vector2(32, 32), new Vector2(0, 0), new Rect(128, 0, 32, 32));
			this.sprite = new Roga2dSprite(renderObject);
			this.AddChild(this.sprite);

			//this.interval = buildWalkInterval();
		}

		public string TextureID{
			get {
				return this.textureId;
			}
		}
		
		public Roga2dSprite Sprite{
			get {
				return this.sprite;
			}
		}

		private Roga2dBaseInterval buildWalkInterval() {
			List<Roga2dAnimationKeyFrame> keyFrames = new List<Roga2dAnimationKeyFrame>();
			int[] frames = new int[4]{1, 0, 1, 2};
			for (int i = 0; i < 4; i++) {
				Roga2dAnimationKeyFrame keyFrame = Roga2dAnimationKeyFrame.Build();
				keyFrame.Rect = new Rect(96 + frames[i] * 32, 0, 32, 32);
				keyFrame.Id = textureId;
				keyFrame.Duration = 10;
				keyFrame.Type = Roga2dAnimationKeyFrameType.Image;
				keyFrame.PixelCenter = new Vector2(0, 0);
				keyFrame.PixelSize = new Vector2(32, 32);
				keyFrames.Add(keyFrame);
			}

			Dictionary<string, string> options = new Dictionary<string, string>();
			options["Battle/Skills/Battler_Base"] = this.textureId;
			return new Roga2dLoop(new Roga2dSourceInterval(this.sprite, keyFrames, null, options), 0);
		}
		
		public override void Update() {
			base.Update();
			if (this.interval != null) {
				this.interval.Update();
			}
		}
	}
}