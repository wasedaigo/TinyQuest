public class Roga2dFunc : Roga2dBaseInterval {
	public delegate void Callback();
	private Callback callback;
	private bool isDone;
	public Roga2dFunc (Callback callback) {
		this.isDone = false;
		this.skippable = true;
		this.callback = callback;
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

	public override sealed void Update() {
		if (!this.IsDone()) {
			this.callback();
			this.isDone = true;
		}
	}
}