using UnityEngine;
using System.Collections;

public class ObjectClickHandler : MonoBehaviour {
	public string FuncName;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void OnDelayClick () {
		this.SendMessageUpwards(FuncName);
	}
}
