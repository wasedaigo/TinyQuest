using UnityEngine;

public class Roga2dRoot : Roga2dNode {
	
	private Roga2dNode target;
	private Roga2dNode targetOrigin;
	private Roga2dAnimationPlayer player;

	public Roga2dAnimationPlayer Player {
		get {
			return player;	
		}
	}
	
	public Roga2dNode Target {
		get {
			return this.target;
		}
		set {
			if (this.target != value && this.target != null) {
				this.target.Destroy();	
				this.targetOrigin.Destroy();
				this.target = null;
				this.targetOrigin = null;
			} 

			this.target = value;
			if (this.target.Parent == null) {
				Debug.LogError("Target must be in scenegraph before set to the root");	
			}
			
			this.targetOrigin = new Roga2dNode("TargetOrigin");
			this.target.Parent.AddChild(this.targetOrigin);
			this.targetOrigin.LocalPosition = target.LocalPosition;
		}
	}
	
	public Roga2dNode TargetOrigin {
		get {
			return this.targetOrigin;
		}
	}
	
	public Roga2dRoot(Roga2dAnimationPlayer player) 
	: base("Root")
	{
		this.player = player;
	}
}