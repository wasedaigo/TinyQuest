using UnityEngine;

public class Roga2dWait : Roga2dBaseInterval {
	protected float duration;
	protected int frameNo;

	public Roga2dWait (int duration) {
		this.duration = duration;
		this.frameNo = 0;
	}
	
	public override sealed bool IsDone() {
		return this.frameNo >= this.duration;
	}
	
	public override sealed void Reset() {
		this.frameNo = 0;
		this.Start();
	}
	
	public override sealed void Start() {}
	
	public override sealed void Finish() {
		this.frameNo = (int)this.duration;
	}

	public override sealed void Update() {
		if (!this.IsDone()) {
			this.frameNo += 1;
		}
	}
}