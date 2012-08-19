using UnityEngine;
using System.Collections;


public class InputTimer : MonoBehaviour {
	public GameObject Timer;
	public int TimerDuration = 5;
	
	private bool active;
	private int timeLeft;
	private GameObject panel;
	private GameObject line;
	private float startTime;
	
	// Use this for initialization
	void Start () {
		this.panel = Timer.transform.FindChild("Panel").gameObject;
		this.line = this.panel.transform.FindChild("Cursor").gameObject;
		this.panel.SetActiveRecursively(false);
	}
	
	// Update is called once per frame
	void Update () {
		if (this.active) {
		}
	}
	
	void StartTimer() {
		this.active = true;
		this.startTime = Time.time;
		this.panel.SetActiveRecursively(true);

		Vector3 pos = this.line.transform.localPosition;
		this.line.transform.localPosition = new Vector3(0, pos.y, pos.z);
		iTween.MoveTo(this.line, iTween.Hash("name", "lineTween", "time", this.TimerDuration, "x", 0.65f,  "easeType", "linear", "oncomplete", "OnInputFinished", "oncompletetarget", this.gameObject));
	}
	
	void CancelTimer() {
		this.FinishTimer();
	}
	
	public void StopLine() {
		//this.SendMessage("InputTimerFinished");
		//this.FinishTimer();
	}
	
	private void OnInputFinished() {
		this.SendMessage("InputTimerFinished");
		this.FinishTimer();
	}
	
	private void FinishTimer() {
		iTween.StopByName("lineTween");
		this.active = false;
		this.active = false;
		this.panel.SetActiveRecursively(false);
	}
}
