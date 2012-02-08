using UnityEngine;
using System.Collections;

public class Roga2dTouchReceiver : MonoBehaviour {
	public delegate void TouchDown(Vector2 pos);
	public TouchDown OnTouchDown;
	public delegate void TouchUp();
	public TouchUp OnTouchUp;
	
	public void ReceiveTouchDown(object data) {
		Vector2 pos = this.transform.InverseTransformPoint((Vector3)data);
		if (this.OnTouchDown != null) {
			this.OnTouchDown(pos);	
		}
	}
	
	public void ReceiveTouchUp() {
		if (this.OnTouchUp != null) {
			this.OnTouchUp();	
		}
	}
}
