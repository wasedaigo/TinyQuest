using System.Collections.Generic;
public class Roga2dSequence : Roga2dBaseInterval {
	
	private List<Roga2dBaseInterval> intervals;
	private int index;
	private Roga2dBaseInterval lastInterval;
	public Roga2dSequence(List<Roga2dBaseInterval> intervals) {
		this.intervals = intervals;
		this.index = 0;
		this.lastInterval = intervals[intervals.Count - 1];
	}
	public override bool IsDone() {
		return this.lastInterval.IsDone();
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
	public override void Update() {
        if (!this.IsDone()) {
            Roga2dBaseInterval currentInterval = this.intervals[this.index];
            currentInterval.Update();
            if (this.IsDone()) {
                this.Finish();
            } else if (currentInterval.IsDone()) {
                this.index += 1;
            }
        }
	}
}