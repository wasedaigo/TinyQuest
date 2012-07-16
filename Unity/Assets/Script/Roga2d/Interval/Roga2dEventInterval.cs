using UnityEngine;
using System.Collections.Generic;

public class Roga2dEventInterval : Roga2dBaseInterval {
	protected float duration;
	protected float elapsed;
	private int frameNo;
	private Dictionary<int, string[]> events;
	private Roga2dAnimationSettings settings;

	public Roga2dEventInterval(Dictionary<int, string[]> events, Roga2dAnimationSettings settings)
	{
		this.elapsed = 0;
		this.frameNo = -1;
		// Calculate the length
		int max = 0;
		foreach (KeyValuePair<int, string[]> entry in events) {
			if (entry.Key > max) {
				max = entry.Key;
			}
		}
		this.duration = max + 1;
		this.events = events;
		this.settings = settings;
	}
	
	public override bool IsDone() {
        return this.frameNo >= this.duration;
    }
	
	public override sealed float ExcessTime() {
		return 0;
	}
	
	public override void Start() {
		this.Reset();
		this.callCommand(0);
	}
	
    public override void Reset() {
        this.frameNo = 0;
    }
	
	public override void Finish() {
		this.frameNo = Mathf.FloorToInt(this.duration);
	}
	
	private void callCommand(int index) {
		if (this.events.ContainsKey(index)) {
			string[] commands = this.events[index];
			foreach (string command in commands) {
				if(settings.CommandCallBack != null) {
					settings.CommandCallBack(settings, command);
				}
			}
		}
	}
	
    public override void Update(float delta) {
		if (!this.IsDone()) {
			this.elapsed += delta;
			int temp = Mathf.FloorToInt(this.elapsed * Roga2dConst.AnimationFPS);
			if (this.frameNo != temp) {
				
				if (this.frameNo > 0) {
					for (int i = this.frameNo; i < temp; i++) {
						this.callCommand(i);
					}
				}
				
				this.frameNo = temp;
		    }
		}
    }
}