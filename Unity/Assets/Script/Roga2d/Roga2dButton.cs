using UnityEngine;
public class Roga2dButton : Roga2dNode {
	
	public enum State{
		Up,
		Down
	};
	
	public delegate void OnTouchDelegate(Roga2dButton button);
	private OnTouchDelegate onTouched;
	private Roga2dRenderObject upRenderObject;
	private Roga2dRenderObject downRenderObject;
	private Roga2dTouchReceiver touchReceiver;
	private bool isPressed;
	private State state;
	
	public Roga2dButton() 
	: base("Button")
	{
		this.GameObject.AddComponent("BoxCollider");
		this.touchReceiver = this.GameObject.AddComponent("Roga2dTouchReceiver") as Roga2dTouchReceiver;
		this.touchReceiver.OnTouchDown = this.OnTouchDown;
		this.touchReceiver.OnTouchUp = this.OnTouchUp;
		this.state = State.Up;
	}
	
	public void SetUpSprite(string textureId, Vector2 pixelSize, Vector2 pixelCenter, Rect srcRect) {
		if (this.upRenderObject == null) {
			this.upRenderObject = new Roga2dRenderObject(textureId, pixelSize, pixelCenter, srcRect);
			this.upRenderObject.Pop();
			this.upRenderObject.Transform.parent = this.Transform;
			this.upRenderObject.Hide();
			UpdateState();
		}
	}
	
	public void SetDownSprite(string textureId, Vector2 pixelSize, Vector2 pixelCenter, Rect srcRect) {
		if (this.downRenderObject == null) {
			this.downRenderObject = new Roga2dRenderObject(textureId, pixelSize, pixelCenter, srcRect);
			this.downRenderObject.Pop();
			this.downRenderObject.Transform.parent = this.Transform;
			this.downRenderObject.Hide();
			UpdateState();
		}
	}
	
	public OnTouchDelegate OnTouched {
		set {
			this.onTouched = value;
		}
	}
	
	private void OnTouchDown(Vector2 pos) {
		this.isPressed = true;
		this.state = State.Down;
		UpdateState();
	}
	
	private void OnTouchUp() {
		if (this.isPressed) {
			if (this.onTouched != null) {
				this.onTouched(this);	
			}
		}
		this.isPressed = false;
		this.state = State.Up;
		UpdateState();
	}
	
	private void UpdateState() {
		
		if (this.upRenderObject != null) {
			if (this.state == State.Up) {
				this.upRenderObject.Show();
			} else {
				this.upRenderObject.Hide();
			}
		}
		
		if (this.downRenderObject != null) {
			if (this.state == State.Down) {
				this.downRenderObject.Show();
			} else {
				this.downRenderObject.Hide();
			}
		}
	}
	
	public override void Destroy() {
		if (this.upRenderObject != null) {
			this.upRenderObject.Destroy();	
		}
		if (this.downRenderObject != null) {
			this.downRenderObject.Destroy();	
		}
		base.Destroy();
	}
}