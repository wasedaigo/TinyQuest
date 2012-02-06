using UnityEngine;
using System.Collections;

public class Roga2dTouchReceiver : MonoBehaviour {
	public delegate void OnTouched();
	public OnTouched Callback;
	public void ReceiveTouch() {
		if (Callback != null) {
			Callback();	
		}
	}
}
