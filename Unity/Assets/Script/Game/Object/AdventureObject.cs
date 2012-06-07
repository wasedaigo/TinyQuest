using UnityEngine;
using System.Collections.Generic;

namespace TinyQuest.Object {
	public class AdventureObject : Roga2dNode {
		protected Roga2dSprite sprite;
		private string textureId;
		public int HP;
		public int TP;
		
		public AdventureObject(string textureId, Vector2 pixelSize, Vector2 pixelCenter, Rect srcRect)
		:base("AdventureObject")
		{
			this.textureId = textureId;
			this.sprite = new Roga2dSprite(this.textureId, pixelSize, pixelCenter, srcRect);
			this.SetPixelSize(pixelSize);
			this.SetPixelCenter(pixelCenter);
			this.AddChild(this.sprite);
			this.HP = 10000;
			this.TP = 3;
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
	
		public bool IsDead() {
			return this.HP < 0;
		}
		
		public void ApplyDamage(uint value) {
			//this.hp -= (int)value;
		}
	}
}