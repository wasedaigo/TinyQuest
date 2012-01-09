using UnityEngine;
public class Roga2dSprite : Roga2dNode {
	private Roga2dRenderObject renderObject;
	
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
					this.renderObject.GameObject.transform.localPosition = Roga2dUtils.FixCoordinate(renderObject.Anchor);
					
					Roga2dGameObjectState state = Roga2dUtils.stashState(this.renderObject.GameObject);
					this.renderObject.GameObject.transform.parent = this.GameObject.transform;
					Roga2dUtils.applyState(this.renderObject.GameObject, state);

				}
			}
		}
	}

	public override void Update() {
		base.Update();
		if (this.renderObject != null) {
			this.renderObject.SetAlpha(this.Alpha);
			this.renderObject.SetBlendType(this.BlendType);
		}
	}
	
	public override Vector2 GetOffsetByPositionAnchor(float positionAnchorX, float positionAnchorY) {
		Vector2 offset = new Vector2(0, 0);
		if (this.renderObject != null) {
	        float centerX = this.RenderObject.PixelSize.x / 2 + this.RenderObject.PixelCenter.x;
	        float centerY = this.RenderObject.PixelSize.y / 2 + this.RenderObject.PixelCenter.y;
			Vector2 pixelSize = this.RenderObject.PixelSize;
			offset.x = centerX + (positionAnchorX * (pixelSize.x / 2) - centerX) * this.LocalScale.x;
			offset.y = centerY + (positionAnchorY * (pixelSize.y / 2) - centerY) * this.LocalScale.y;
		}
        return offset;
    }
}