using UnityEngine;
using System.Collections.Generic;

namespace TinyQuest.Object {
	public class Actor : Roga2dNode {
		protected Roga2dSprite sprite;
		private string textureId;
		
		public Actor(string textureId, Vector2 pixelSize, Vector2 pixelCenter, Rect srcRect)
		:base("Actor")
		{
			this.textureId = textureId;
			this.sprite = new Roga2dSprite(this.textureId, pixelSize, pixelCenter, srcRect);
			this.SetPixelSize(pixelSize);
			this.SetPixelCenter(pixelCenter);
			this.AddChild(this.sprite);
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
	}
}