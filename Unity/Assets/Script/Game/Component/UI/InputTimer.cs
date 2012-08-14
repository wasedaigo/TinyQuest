using UnityEngine;
using System.Collections;


public class InputTimer : MonoBehaviour {
	public GameObject Timer;
	public int TimerDuration = 5;
	
	private int timeLeft;
	private UILabel label;
	
	// Use this for initialization
	void Start () {
		this.label = Timer.transform.FindChild("Label").GetComponent<UILabel>();
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	void StartTimer() {
		this.StartCoroutine("CountDown");
	}
	
	void CancelTimer() {
		this.StopCoroutine("CountDown");
		this.label.text = "";
	}
	
	IEnumerator CountDown() {
		for (int i = this.TimerDuration; i >= 1; i--) {
			this.label.text = i.ToString();
			yield return new WaitForSeconds(1);
		}
		this.label.text = "";
		this.SendMessage("InputTimerFinished");
	}
}
