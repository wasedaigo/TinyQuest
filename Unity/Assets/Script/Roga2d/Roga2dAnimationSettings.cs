using UnityEngine;

public class Roga2dAnimationSettings {
	private Roga2dNode root;
	private Roga2dNode origin;
	private Roga2dNode target;
	private Roga2dNode targetOrigin;
	private Roga2dAnimationPlayer player;
	private Roga2dCommandCallback commandCallBack;

	public Roga2dAnimationPlayer Player {
		get {
			return player;	
		}
	}
	
	public Roga2dCommandCallback CommandCallBack {
		get {
			return this.commandCallBack;
		}
	}
	
	public Roga2dNode Root {
		get {
			return this.root;
		}
	}
	
	public Roga2dNode Origin {
		get {
			return this.origin;
		}
	}
	
	public Roga2dNode Target {
		get {
			return this.target;
		}
		private set {
			if (this.target != value && this.target != null) {
				this.target.Destroy();	
				this.targetOrigin.Destroy();
				this.target = null;
				this.targetOrigin = null;
			} 
			
			if (value != null) {
				this.target = value;
				if (this.target.Parent == null) {
					Debug.LogError("Target must be in scenegraph before set to the root");	
				}
				
				this.targetOrigin = new Roga2dNode("TargetOrigin");
				this.target.Parent.AddChild(this.targetOrigin);
				this.targetOrigin.LocalPosition = target.LocalPosition;
			}
		}
	}
	
	public Roga2dNode TargetOrigin {
		get {
			return this.targetOrigin;
		}
	}
	
	public Roga2dAnimationSettings(Roga2dAnimationPlayer player, Roga2dNode root, Roga2dNode origin, Roga2dNode target, Roga2dCommandCallback commandCallBack) 
	{
		this.player = player;
		this.root = root;
		this.origin = origin;
		this.Target = target;
		this.commandCallBack = commandCallBack;
	}
	
	public void Destroy() {
		if (this.targetOrigin != null) {
			this.targetOrigin.Destroy();
			this.targetOrigin.Parent.RemoveChild(this.targetOrigin);
			this.targetOrigin = null;
		}
	}
}
public delegate void Roga2dCommandCallback(Roga2dAnimationSettings settings, string command);