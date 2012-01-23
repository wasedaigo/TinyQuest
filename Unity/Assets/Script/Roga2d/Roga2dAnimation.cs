using UnityEngine;

public class Roga2dAnimation {
	public Roga2dBaseInterval Interval;
	public Roga2dNode Node;
	public Roga2dNode Target;
	public Roga2dAnimationFinishCallback finishCallback;
	public bool IsStarted;
	public GameObject Root;
	
	public static Roga2dAnimation Build(Roga2dNode node, Roga2dBaseInterval interval) {
		Roga2dAnimation animation = new Roga2dAnimation();
		animation.Interval = interval;
		animation.Node = node;
		animation.IsStarted = false;
		animation.finishCallback = null;
		return animation;
	}
}
public delegate void Roga2dAnimationFinishCallback(Roga2dAnimation animation);