using UnityEngine;
using System.Collections.Generic;

namespace TinyQuest.Component {
	public class BaseObject : Roga2dNode {
		private Roga2dBaseInterval interval;
		private string textureId;
		private Roga2dSprite sprite;
		private int hp;
		
		public BaseObject(string textureId, Vector2 pixelSize, Vector2 pixelCenter, Rect srcRect)
		:base("BaseObject")
		{
			this.textureId = textureId;
			this.interval = null;
			Roga2dRenderObject renderObject = new Roga2dRenderObject(this.textureId, pixelSize, pixelCenter, srcRect);
			this.sprite = new Roga2dSprite(textureId, renderObject);
			this.AddChild(this.sprite);
			this.hp = 10000;
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
		}
		
		public override void Update() {
			base.Update();
			if (this.interval != null) {
				this.interval.Update();
			}
		}
	
		public bool IsDead() {
			return this.hp < 0;
		}
		
		public void ApplyDamage(uint value) {
			this.hp -= (int)value;
		}
	}
}