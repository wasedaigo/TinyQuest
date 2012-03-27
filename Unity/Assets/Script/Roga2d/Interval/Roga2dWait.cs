using UnityEngine;

public class Roga2dWait : Roga2dBaseInterval {
	protected float duration;
	protected float elapsed;

	public Roga2dWait (float duration) {
		this.duration = duration;
		this.elapsed = 0;
	}
	
	public override sealed float ExcessTime() {
		float excessTime = this.elapsed - this.duration;
		return excessTime;
	}

	public override sealed bool IsDone() {
		return this.elapsed >= this.duration;
	}
	
	public override sealed void Reset() {
		this.elapsed = 0;
		this.Start();
	}
	
	public override sealed void Start() {}
	
	public override sealed void Finish() {
		this.elapsed = this.duration;
	}

	public override sealed void Update(float delta) {
		if (!this.IsDone()) {
			this.elapsed += delta;
		}
	}
}