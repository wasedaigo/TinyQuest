using UnityEngine;
public class Roga2dSprite : Roga2dNode {
	private Roga2dRenderObject renderObject;
	
	public Roga2dSprite(string name, Roga2dRenderObject renderObject) 
	: base(new GameObject(name))
	{
		this.RenderObject = renderObject;
	}
	
	public Roga2dSprite(Roga2dRenderObject renderObject) 
	: base(new GameObject("Sprite"))
	{
		this.RenderObject = renderObject;
	}

	public Roga2dSprite(GameObject go, Roga2dRenderObject renderObject) 
	: base(go)
	{
		this.RenderObject = renderObject;
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
					this.renderObject.Transform.localPosition = Roga2dUtils.FixCoordinate(renderObject.Anchor);
					
					Roga2dGameObjectState state = Roga2dUtils.stashState(this.renderObject.Transform);
					this.renderObject.Transform.parent = this.Transform;
					Roga2dUtils.applyState(this.renderObject.Transform, state);
				}
			}
		}
	}

	public override void Update() {
		base.Update();
		if (this.renderObject != null && this.renderObject.Renderer != null) {
			this.renderObject.Renderer.enabled = this.IsVisible;
			if (this.IsVisible) {
				this.renderObject.SetBlend(this.BlendType, this.Alpha, this.LocalHue);
			}
		}
	}
	
	public override Vector2 GetOffsetByPositionAnchor(float positionAnchorX, float positionAnchorY) {
		Vector2 offset = new Vector2(0, 0);
		if (this.renderObject != null) {
			Vector2 pixelSize = this.RenderObject.PixelSize;
	        float centerX = pixelSize.x / 2 + this.RenderObject.PixelCenter.x;
	        float centerY = pixelSize.y / 2 + this.RenderObject.PixelCenter.y;
			offset.x = centerX + (positionAnchorX * (pixelSize.x / 2) - centerX) * this.LocalScale.x;
			offset.y = centerY + (positionAnchorY * (pixelSize.y / 2) - centerY) * this.LocalScale.y;
		}
        return offset;
    }
}