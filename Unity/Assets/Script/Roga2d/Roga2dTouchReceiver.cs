using UnityEngine;
using System.Collections;

public class Roga2dTouchReceiver : MonoBehaviour {
	public delegate void TouchDown();
	public TouchDown OnTouchDown;
	public delegate void TouchUp();
	public TouchUp OnTouchUp;
	
	public void ReceiveTouchDown() {
		if (this.OnTouchDown != null) {
			this.OnTouchDown();	
		}
	}
	
	public void ReceiveTouchUp() {
		if (this.OnTouchUp != null) {
			this.OnTouchUp();	
		}
	}
}
