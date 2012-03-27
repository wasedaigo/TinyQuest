public abstract class Roga2dBaseInterval {
	protected bool skippable;
	public bool isSkippable() {
		return this.skippable;	
	}
	
	public abstract bool IsDone();
	public abstract void Reset();
	public abstract void Start();
	public abstract void Finish();
	public abstract void Update(float delta);
	public abstract float ExcessTime();
}