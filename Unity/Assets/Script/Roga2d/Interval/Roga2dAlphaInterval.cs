using UnityEngine;

public class Roga2dAlphaInterval : Roga2dValueInterval<float> {
	private Roga2dNode node;
	
	public Roga2dAlphaInterval(Roga2dNode node, float start, float end, float duration, bool tween) 
	: base(start, end, duration, tween) 
	{
		this.node = node;
	}

	protected override void SetValue(float value) {
		this.node.LocalAlpha = value;
	}
}