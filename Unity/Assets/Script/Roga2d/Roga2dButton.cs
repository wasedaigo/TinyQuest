using UnityEngine;
public class Roga2dButton : Roga2dNode {
	
	Roga2dRenderObject upRenderObject;
	Roga2dRenderObject downRenderObject;
	public Roga2dButton(Roga2dRenderObject upRenderObject) 
	: base("Button")
	{
		this.upRenderObject = upRenderObject;
		this.upRenderObject.Pop();

		this.GameObject.AddComponent("BoxCollider");
		Roga2dTouchReceiver touchReceiver = this.GameObject.AddComponent("Roga2dTouchReceiver") as Roga2dTouchReceiver;
		touchReceiver.Callback = this.OnTouched;
		this.upRenderObject.Transform.parent = this.Transform;
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
	
	public void OnTouched() {
		Debug.Log("ON TOUCHED");
	}
}