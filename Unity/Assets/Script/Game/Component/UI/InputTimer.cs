using UnityEngine;
using System.Collections;


public class InputTimer : MonoBehaviour {
	public GameObject Beacon;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void StartTimer() {
		Vector3 pos = Beacon.transform.localPosition;
		
		Beacon.transform.localPosition = new Vector3(-163, pos.y, pos.z);
		iTween.MoveTo(Beacon, iTween.Hash("time", 5, "x", 0.7f,  "easeType", "linear", "oncomplete", "OnTimerFinished", "oncompletetarget", this.gameObject));
	}

	void OnTimerFinished() {
		this.SendMessage("InputTimerFinished");
	}
}
