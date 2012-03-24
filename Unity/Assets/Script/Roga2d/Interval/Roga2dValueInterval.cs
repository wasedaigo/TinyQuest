using UnityEngine;

public abstract class Roga2dValueInterval<T> : Roga2dBaseInterval {
	protected T start;
	protected T end;
	protected bool tween;
	protected float duration;
	protected float elapsed;
	
	public Roga2dValueInterval (T start, T end, float duration, bool tween) {
		this.start = start;
		this.end = end;
		this.duration = duration;
		this.elapsed = 0;
		this.tween = tween;
	}
	
	public override sealed bool IsDone() {
		return this.elapsed >= this.duration;
	}
	
	public override sealed void Reset() {
		this.elapsed = 0;
		this.Start();
	}
	
	public override sealed void Start() {
		T[] values = this.TweenBeforeFilter(this.start, this.end);
		this.SetValue(values[0]);
	}
	
	public override sealed void Finish() {
		this.elapsed = this.duration;
	}

	public override sealed void Update(float delta) {
		if (!this.IsDone()) {
			this.elapsed += delta;
			T[] values = this.TweenBeforeFilter(this.start, this.end);
			
			if (this.tween) {
				float t = this.elapsed;
				if (this.elapsed > this.duration) {
					t = this.duration;
				}
				this.SetValue(Roga2dUtils<T>.Completement(values[0], values[1], t / this.duration));
			} else {
				// Single frame get its ending value immediately
				if (this.duration <= 1 && this.IsDone()) {
					this.SetValue(values[1]);
				} else {
					this.SetValue(values[0]);
				}
				
			}
		}
	}
	
	protected virtual T[] TweenBeforeFilter(T start, T end) {return new T[2] {start, end};}
	protected abstract void SetValue(T value);
}