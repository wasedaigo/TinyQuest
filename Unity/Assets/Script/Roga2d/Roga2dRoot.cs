using UnityEngine;

public class Roga2dRoot : Roga2dNode {
	
	public Roga2dNode Target;
	public Roga2dNode TargetOrigin;
	private Roga2dAnimationPlayer player;


	public Roga2dAnimationPlayer Player {
		get {
			return player;	
		}
	}
	
	public Roga2dRoot(Roga2dAnimationPlayer player) 
	: base("Root")
	{
		this.player = player;
	}
}