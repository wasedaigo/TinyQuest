using UnityEngine;

public class Roga2dScaleInterval : Roga2dValueInterval<Vector2> {
	private Roga2dNode node;
	
	public Roga2dScaleInterval(Roga2dNode node, Vector2 start, Vector2 end, int duration, bool tween) 
	: base(start, end, duration, tween) 
	{
		this.node = node;
	}

	protected override void SetValue(Vector2 value) {
		this.node.LocalScale = value;
	}
}