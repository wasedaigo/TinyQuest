using UnityEngine;
using System.Collections;

public class ObjectClickHandler : MonoBehaviour {
	public System.Action Callback;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void OnClick () {
		if (Callback != null) {
			Callback();	
		}
	}
}
