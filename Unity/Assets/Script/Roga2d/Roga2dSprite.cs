using UnityEngine;
public class Roga2dSprite : Roga2dNode {
	private Roga2dRenderObject renderObject;
	
	public Roga2dSprite(string textureId, Vector2 pixelSize, Vector2 pixelCenter, Rect srcRect) 
	: base(textureId)
	{
		this.RenderObject = new Roga2dRenderObject(textureId, pixelSize, pixelCenter, srcRect);
		this.SetPixelSize(this.RenderObject.pixelSize);
		this.SetPixelCenter(this.RenderObject.pixelCenter);
	}
	
	public Roga2dSprite(Roga2dRenderObject renderObject) 
	: base("Sprite")
	{
		this.RenderObject = renderObject;
		if (this.RenderObject != null) {
			base.SetPixelSize(this.RenderObject.pixelSize);
			base.SetPixelCenter(this.RenderObject.pixelCenter);
		}
	}
	
	public override void Destroy() {
		base.Destroy();
		if (this.renderObject != null) {
			this.renderObject.Destroy();
		}
	}
	
	public Roga2dRenderObject RenderObject {
		get {
			return this.renderObject;
		}
		set {
			if (value != this.renderObject) {
				if (this.renderObject != null) {
					this.renderObject.Destroy();
					this.renderObject = null;
				}
				if (value != null) {
					this.renderObject = value;
					this.renderObject.Pop();
					this.renderObject.Transform.localPosition = renderObject.Anchor;
					
					Roga2dGameObjectState state = Roga2dUtils.stashState(this.renderObject.Transform);
					this.renderObject.Transform.parent = this.Transform;
					Roga2dUtils.applyState(this.renderObject.Transform, state);
				}
			}
		}
	}
	
	public override void Hide() {
		base.Hide();
		if (this.renderObject != null) {
			this.renderObject.Renderer.enabled = this.IsVisible;
		}
	}
	
	
	public override void Show() {
		base.Show();
		if (this.renderObject != null) {
			this.renderObject.Renderer.enabled = this.IsVisible;
		}
	}
	
	public override void Update() {
		base.Update();
		if (this.renderObject != null && this.renderObject.Renderer != null) {
			this.renderObject.Renderer.enabled = this.IsVisible;
			if (this.IsVisible) {
				this.renderObject.SetBlend(this.BlendType, this.Alpha, this.Hue);
			}
		}
	}
}