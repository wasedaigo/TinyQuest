using System.Collections.Generic;
public class Roga2dParallel : Roga2dBaseInterval {
	
	private List<Roga2dBaseInterval> intervals;
	private float excessTime;
	public Roga2dParallel(List<Roga2dBaseInterval> intervals) {
		this.intervals = intervals;
		this.excessTime = -1;
	}
	
	public override sealed float ExcessTime() {
		return this.excessTime;
	}

	public override bool IsDone() {
        bool isDone = true;
        foreach (Roga2dBaseInterval interval in this.intervals) {
            if (!interval.IsDone()) {
                isDone = false;
                break;
            }
        }
        return isDone;
	}
	
	public override void Reset() {
        foreach (Roga2dBaseInterval interval in this.intervals) {
        	interval.Reset();
        }
        this.Start();
	}
	
	public override void Start() {
        foreach (Roga2dBaseInterval interval in this.intervals) {
        	interval.Start();
        }
	}
	
	public override void Finish() {
        foreach (Roga2dBaseInterval interval in this.intervals) {
        	interval.Finish();
        }
	}
	
	public override void Update(float delta) {
        if (this.IsDone()) {
			this.excessTime = 0;
		} else {
			float excessTime = 0.0f;
	        foreach (Roga2dBaseInterval interval in this.intervals) {
	        	interval.Update(delta);
				if (excessTime < interval.ExcessTime()) {
					excessTime = interval.ExcessTime();
				}
	        }
			if (this.IsDone()) {
				this.Finish();
				this.excessTime = excessTime;
			}
        }
	}
}