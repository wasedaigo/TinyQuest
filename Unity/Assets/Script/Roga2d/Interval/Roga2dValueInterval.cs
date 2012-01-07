using UnityEngine;

public abstract class Roga2dValueInterval<T> : Roga2dBaseInterval {
	protected T start;
	protected T end;
	protected bool tween;
	protected float duration;
	protected int frameNo;

	public Roga2dValueInterval (T start, T end, int duration, bool tween) {
		this.start = start;
		this.end = end;
		this.duration = duration;
		this.frameNo = 0;
		this.tween = tween;
	}
	
	public override sealed bool IsDone() {
		return this.frameNo >= this.duration;
	}
	
	public override sealed void Reset() {
		this.frameNo = 0;
		this.Start();
	}
	
	public override sealed void Start() {
		this.SetValue(this.start);
	}
	
	public override sealed void Finish() {
		this.frameNo = (int)this.duration;
	}

	public override sealed void Update() {
		if (!this.IsDone()) {
			this.frameNo += 1;

			T[] values = this.TweenBeforeFilter(this.start, this.end);
			
			if (this.tween) {
				this.SetValue(Roga2dUtils<T>.Completement(values[0], values[1], this.frameNo / this.duration));
			} else {
				this.SetValue(values[0]);
			}
		}
	}
	
	protected virtual T[] TweenBeforeFilter(T start, T end) {return new T[2] {start, end};}
	protected abstract void SetValue(T value);
}