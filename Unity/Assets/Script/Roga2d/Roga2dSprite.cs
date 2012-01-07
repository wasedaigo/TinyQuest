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
					Transform transform = this.GameObject.transform;
					this.renderObject.GameObject.transform.parent = transform;
					Vector2 anchor = renderObject.Anchor;
					this.renderObject.GameObject.transform.localPosition = new Vector3(anchor.y, anchor.x, 0.0f);
				}
			}
		}
	}

	public override void Update() {
		base.Update();
		if (this.renderObject != null) {
			this.renderObject.SetAlpha(this.Alpha);
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