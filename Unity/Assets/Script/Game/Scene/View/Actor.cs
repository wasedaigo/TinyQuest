using UnityEngine;
using System.Collections.Generic;

namespace TinyQuest.Object {
	public class Actor : Roga2dNode {
		protected Roga2dSprite sprite;
		private string textureId;
		
		public enum PoseType {
			Stand,
			Walk,
			Sit,
			Dying,
			Dead,
			Attacked,
			Attack
		};
		
		public Actor(string name) 
			:base(name)
		{
		}
		
		public virtual void SetPoseType(PoseType poseType) {
		}
		
		public virtual void SetSprite(string textureId, Vector2 pixelSize, Vector2 pixelCenter, Rect srcRect) {
			this.textureId = textureId;
			this.sprite = new Roga2dSprite(this.textureId, pixelSize, pixelCenter, srcRect);
			this.SetPixelSize(pixelSize);
			this.SetPixelCenter(pixelCenter);
			this.AddChild(this.sprite);
		}
		
		public virtual void SetStatus(int hp, int maxHp) {
			
		}
		
		public Roga2dSprite Sprite {
			get {
				return this.sprite;
			}
		}
		
		public string TextureID {
			get {
				return this.textureId;
			}
		}

		public Vector2 PixelSize {
			get {
				return this.sprite.RenderObject.PixelSize;
			}
		}
	
		public Vector2 PixelCenter {
			get {
				return this.sprite.RenderObject.PixelCenter;
			}
		}
	
		public Rect SrcRect {
			get {
				return this.sprite.RenderObject.SrcRect;
			}
			set {
				this.sprite.RenderObject.SetSrcRect(value);
			}
		}
		
		public virtual void startWalkingAnimation() {}
		public virtual void stopWalkingAnimation() {}
	}
}