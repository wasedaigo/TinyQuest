public class Roga2dLoop : Roga2dBaseInterval {
	
	private Roga2dBaseInterval interval;
	private int loopCount;
	private int loopCounter;
	
	public Roga2dLoop(Roga2dBaseInterval interval, int loopCount) {
		this.interval = interval;
		this.loopCount = loopCount;
		this.loopCounter = 0;
	}
	
	public override bool IsDone() {
        bool isDone = false;
        if (this.loopCount == 0) {
            // Infinite loop never ends
            isDone = false;
		} else {
            // This is how to determine whether the interval is in the last frame or not
            isDone = this.interval.IsDone() && this.loopCounter >= this.loopCount - 1;
        }
        return isDone;
	}
	
	public override void Reset() {
        this.loopCounter = 0;
        this.interval.Reset();
	}
	
	public override void Start() {
		this.interval.Start();
	}
	
	public override void Finish() {
		this.interval.Finish();
	}
	
	public override void Update() {
        if (!this.IsDone()) {
            this.interval.Update();
            if (this.interval.IsDone()) {
                this.loopCounter++;
                if (this.loopCount == 0 || this.loopCounter < this.loopCount) {
                    // Repeat this interval again, since this is a subanimation
                    this.interval.Reset();
                }
            }
        }
	}
}