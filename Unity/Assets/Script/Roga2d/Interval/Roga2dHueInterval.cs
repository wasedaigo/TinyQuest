using UnityEngine;

public class Roga2dHueInterval : Roga2dValueInterval<Roga2dHue> {
	private Roga2dNode node;
	
	public Roga2dHueInterval(Roga2dNode node, Roga2dHue start, Roga2dHue end, int duration, bool tween) 
	: base(start, end, duration, tween) 
	{
		this.node = node;
	}

	protected override void SetValue(Roga2dHue value) {
		this.node.LocalHue = value;
	}
}