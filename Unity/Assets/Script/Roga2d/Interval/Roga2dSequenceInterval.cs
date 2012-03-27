using System.Collections.Generic;
public class Roga2dSequence : Roga2dBaseInterval {
	
	private List<Roga2dBaseInterval> intervals;
	private int index;
	private Roga2dBaseInterval lastInterval;
	private float excessTime;
	public Roga2dSequence(List<Roga2dBaseInterval> intervals) {
		this.intervals = intervals;
		this.index = 0;
		this.excessTime = -1;
		this.lastInterval = intervals[intervals.Count - 1];
	}
	public override bool IsDone() {
		return this.lastInterval.IsDone();
	}
	
	public override sealed float ExcessTime() {
		return this.excessTime;
	}
	
	public override void Reset() {
        this.index = 0;
        foreach (Roga2dBaseInterval interval in this.intervals) {
            interval.Reset();
        }
        this.Start();
	}
	public override void Start() {
		this.intervals[0].Start();
	}
	public override void Finish() {
		this.intervals[this.index].Finish();
	}
	public override void Update(float delta) {
        if (this.IsDone()) {
			this.excessTime = 0;
		} else {
			while (true) {
	            Roga2dBaseInterval currentInterval = this.intervals[this.index];
	            currentInterval.Update(delta);
				delta = currentInterval.ExcessTime();
	            if (this.IsDone()) {
	                this.Finish();
					this.excessTime = delta;
					break;
	            } else if (currentInterval.IsDone()) {
	                this.index += 1;
	            }
				
				if (delta <= 0) { break; }
			}
        }
	}
}