using UnityEngine;
using System.Collections.Generic;

/*
 * 	[Optimization idea]
 *  1. Setting LocalRot/Position/Scale costs a bit, so keep the value to local valuable. Set it to the node in update call
 *  2. Use GameObject pool
 */
public class Roga2dNode {
	public object Tag;
	public Vector2 Velocity;
	public float LocalAlpha;
	public Roga2dHue LocalHue;
	public float LocalPriority;
	
	public Roga2dBlendType BlendType;
	public Roga2dNode Parent;
	public List<Roga2dNode> children;
	
	protected GameObject GameObject;
	
	private static int nodeCount = 0;
	private Transform transform;
	private float priority;
	private float alpha;
	private Roga2dHue hue;
	private bool isHidden;

	
	public Vector2 LocalPixelPosition {
		get {
			return Roga2dUtils.localToPixel(this.transform.localPosition);
		}
		set {
			this.LocalPosition = Roga2dUtils.pixelToLocal(value);
		}
	}
	
	public float LocalRotation {
		get {
			return this.transform.localEulerAngles.z;
		}
		set {
			value = (value + 360.0f) % 360.0f;
			this.transform.localEulerAngles = new Vector3(0, 0, value);
		}
	}
	
	public Vector2 LocalPosition {
		get {
			return this.transform.localPosition;
		}
		set {
			this.transform.localPosition = new Vector3(value.x, value.y, this.transform.localPosition.z);
		}
	}
	
	public Vector2 LocalScale {
		get {
			return this.transform.localScale ;
		}
		set {
			this.transform.localScale = new Vector3(value.x, value.y, 1.0f);
		}
	}
	
	public Transform Transform {
		get {
			return this.transform;	
		}
	}
	
	public bool IsVisible {
		get {
			return !this.isHidden && this.alpha != 0.0f;
		}
	}
	
	public virtual void Hide() {
		this.isHidden = true;
		foreach(Roga2dNode node in this.children) {
			node.Hide();
		}
	}
	
	public virtual void Show() {
		this.isHidden = false;
		foreach(Roga2dNode node in this.children) {
			node.Show();
		}
	}
	
	public Roga2dNode(string name) {
		Initialize(name);
	}
	
	public Roga2dNode() {
		Initialize("Node");
	}
	
	private void Initialize(string name) {
		
		this.children = new List<Roga2dNode>();
		
		if (this.GameObject == null) {
			this.GameObject = new GameObject(name);
			this.transform = this.GameObject.transform;
		} else {
			this.LocalPosition = this.transform.position;
		}

		this.LocalHue.SetRGB(0, 0, 0);
		this.LocalAlpha = 1.0f;
		this.LocalPriority = 0.0f;
		this.alpha = 1.0f;
		this.hue = new Roga2dHue(0, 0, 0); 
		this.priority = -999.0f;
	}
	
	public virtual void Destroy() {
		if (this.GameObject != null) {
			this.transform.parent = null;
			Object.Destroy(this.GameObject);
			this.GameObject = null;
		}

		foreach (Roga2dNode node in this.children) {
			node.Destroy();	
		}
	}

	public int ChildrenCount {
		get {
			return this.children.Count;
		}
	}
	
	public Vector2 Position {
		get {
			return (Vector2)this.transform.position;
		}
	}
	
	public float Alpha {
		get {
			return this.alpha;	
		}
	}
	
	public Roga2dHue Hue {
		get {
			return this.hue;	
		}
	}
	
	public float Priority {
		get {
			return this.priority;	
		}
	}

	public virtual void UpdateTransparency() {
        if (this.Parent == null) {
			this.alpha = this.LocalAlpha;
        } else {
            this.alpha = this.LocalAlpha * this.Parent.Alpha;
        }
	}
	
	public virtual void UpdateHue() {
        if (this.Parent == null) {
			this.hue = this.LocalHue;
        } else {
            this.hue = this.LocalHue + this.Parent.Hue;
        }
	}
	
	public virtual void UpdateMovement() {
		// Add velocity
		// TODO: Should be remove in future. Use interval for this kind of move!
		if (this.Velocity.x != 0 && this.Velocity.y != 0) {
			Vector2 velocity = this.Velocity;
			Vector2 position = this.LocalPosition;

			this.LocalPosition = new Vector2(Roga2dUtils.RoundPrecision(position.x + velocity.x *  Roga2dConst.AnimationFPS * Time.deltaTime), Roga2dUtils.RoundPrecision(position.y + velocity.y * Roga2dConst.AnimationFPS * Time.deltaTime));
		}
	}
	
	public virtual void UpdatePriority() {
		float lastPriority = this.priority;
        if (this.Parent == null) {
            this.priority = this.LocalPriority;
        } else {
			this.priority = this.LocalPriority + this.Parent.Priority;
        }
		// Move z position of the node, so that it reflects its render-priority
		if (lastPriority != this.priority) {
			Transform transform = this.transform;
			transform.position = new Vector3(
				transform.position.x, 
				transform.position.y, 
				this.priority
			);
		}
	}

	public virtual void Update() {
		UpdateTransparency();
		UpdateHue();
		UpdateMovement();
		UpdatePriority();

		foreach(Roga2dNode node in this.children) {
			node.Update();
		}
	}
	
	public void AddChild(Roga2dNode node) {
		if (node.Parent != null) {
			Debug.LogError("Node cannot have multiple parent");
		}

		this.children.Add(node);
		
		if (this.isHidden) {
			node.Hide();
		}
		Roga2dGameObjectState state = Roga2dUtils.stashState(node.Transform);
		node.Transform.parent = this.transform;
		node.Parent = this;
		Roga2dUtils.applyState(node.Transform, state);
		nodeCount += 1;
	}
	
    public void RemoveChild(Roga2dNode node) {
		if (node != null) {
	        this.children.Remove(node);
			node.Destroy();
			nodeCount -= 1;
		}
    }
	
	public void RemoveAllChildren() {
		foreach(Roga2dNode node in this.children) {
			node.Destroy();
			nodeCount -= 1;
		}
		this.children.Clear();
	}

	public Vector2 InverseTransformPoint(Vector2 point) {
			return this.transform.InverseTransformPoint(point);
	}
	
	public virtual Vector2 GetOffsetByPositionAnchor(float positionAnchorX, float positionAnchorY) {return new Vector2(0, 0);}
}
