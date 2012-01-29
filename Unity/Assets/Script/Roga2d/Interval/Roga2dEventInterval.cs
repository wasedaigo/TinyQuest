using UnityEngine;
using System.Collections.Generic;

public class Roga2dEventInterval : Roga2dBaseInterval {
	protected float duration;
	private int frameNo;
	private Dictionary<int, string[]> events;
	private Roga2dCommandCallback callback;

	public Roga2dEventInterval(Dictionary<int, string[]> events, Roga2dCommandCallback callback)
	{
		// Calculate the length
		int max = 0;
		foreach (KeyValuePair<int, string[]> entry in events) {
			if (entry.Key > max) {
				max = entry.Key;
			}
		}
		this.duration = max + 1;
		this.events = events;
		this.callback = callback;
	}
	
	public override bool IsDone() {
        return this.frameNo >= this.duration;
    }
	
	public override void Start() {
		this.Reset();
	}
	
    public override void Reset() {
        this.frameNo = 0;
    }
	
	public override void Finish() {
		this.frameNo = Mathf.FloorToInt(this.duration);
	}
	
    public override void Update() {
		this.frameNo += 1;
        if (!this.IsDone()) {
			if (this.events.ContainsKey(this.frameNo)) {
				string[] commands = this.events[this.frameNo];
				foreach (string command in commands) {
					if(this.callback != null) {
						this.callback(command);
					}
				}
			}
        }
    }
}