public class Roga2dFunc : Roga2dBaseInterval {
	public delegate void Callback();
	private Callback callback;
	private bool isDone;
	private float excessTime;
	public Roga2dFunc (Callback callback) {
		this.isDone = false;
		this.skippable = true;
		this.callback = callback;
		this.excessTime = 0;
	}
	
	public override sealed float ExcessTime() {
		return this.excessTime;
	}
	
	public override sealed bool IsDone() {
		return this.isDone;
	}
	
	public override sealed void Reset() {
		this.isDone = false;
	}
	
	public override sealed void Start() {}
	
	public override sealed void Finish() {
		this.isDone = true;
	}

	public override sealed void Update(float delta) {
		this.excessTime = delta;
		if (!this.IsDone()) {
			this.callback();
			this.isDone = true;
		}
	}
}