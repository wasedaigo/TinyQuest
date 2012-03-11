using UnityEngine;
using System.Collections.Generic;

namespace TinyQuest.Component {
	public class AdventureObject : Roga2dNode {
		protected Roga2dSprite sprite;
		private string textureId;
		private int hp;
		
		public AdventureObject(string textureId, Vector2 pixelSize, Vector2 pixelCenter, Rect srcRect)
		:base("AdventureObject")
		{
			this.textureId = textureId;
			this.sprite = new Roga2dSprite(this.textureId, pixelSize, pixelCenter, srcRect);
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
	
		public bool IsDead() {
			return this.hp < 0;
		}
		
		public void ApplyDamage(uint value) {
			this.hp -= (int)value;
		}
	}
}