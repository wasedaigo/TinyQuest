using UnityEngine;

public class Roga2dAnimation {
	public Roga2dBaseInterval Interval;
	public Roga2dNode Node;
	public Roga2dAnimationSettings settings;
	public System.Action<Roga2dAnimation> finishCallback;
	public bool IsStarted;
	public GameObject Root;
	public Object Data;
	
	public static Roga2dAnimation Build(Roga2dNode node,  Roga2dBaseInterval interval) {
		return Build(node, interval, null);
	}
	
	public static Roga2dAnimation Build(Roga2dNode node, Roga2dBaseInterval interval, Roga2dAnimationSettings settings) {
		Roga2dAnimation animation = new Roga2dAnimation();
		animation.Interval = interval;
		animation.Node = node;
		animation.IsStarted = false;
		animation.finishCallback = null;
		animation.settings = settings;
		return animation;
	}
}