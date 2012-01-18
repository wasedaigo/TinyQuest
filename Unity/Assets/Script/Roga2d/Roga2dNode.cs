using UnityEngine;
using System.Collections.Generic;

/*
 * 	[Optimization idea]
 *  1. Setting LocalRot/Position/Scale costs a bit, so keep the value to local valuable. Set it to the node in update call
 *  2. Use GameObject pool
 */
public class Roga2dNode {
	
	private static int nodeCount = 0;
	public Vector2 Velocity;
	public float LocalAlpha;
	public Roga2dHue LocalHue;
	public float LocalPriority;
	
	public Roga2dBlendType BlendType;
	public Roga2dNode Parent;
	public GameObject GameObject;

	public List<Roga2dNode> children;
	private float priority;
	private float alpha;
	private bool isHidden;
	
	public Vector2 LocalPixelPosition {
		get {
			return Roga2dUtils.localToPixel(this.GameObject.transform.localPosition);
		}
		set {
			this.GameObject.transform.localPosition = Roga2dUtils.pixelToLocal(value);
		}
	}
	public float LocalRotation {
		get {
			return this.GameObject.transform.localEulerAngles.z;
		}
		set {
			value = (value + 360.0f) % 360.0f;
			this.GameObject.transform.localEulerAngles = new Vector3(0, 0, value);
		}
	}
	public Vector2 LocalPosition {
		get {
			return this.GameObject.transform.localPosition;
		}
		set {
			this.GameObject.transform.localPosition = value;
		}
	}
	public Vector2 LocalScale {
		get {
			return this.GameObject.transform.localScale ;
		}
		set {
			this.GameObject.transform.localScale = new Vector3(value.x, value.y, 1.0f);
		}
	}
	public bool IsVisible {
		get {
			return !this.isHidden;
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
	
	public Roga2dNode(GameObject go) {
		this.GameObject = go;
		Initialize("Node");
	}
	
	private void Initialize(string name) {
		this.children = new List<Roga2dNode>();
		
		if (this.GameObject == null) {
			this.GameObject = new GameObject(name);
		} else {
			this.LocalPosition = this.GameObject.transform.position;
		}

		this.LocalAlpha = 1.0f;
		this.LocalPriority = 0.5f;
		this.alpha = 1.0f;
		this.priority = 0.5f;
	}
	
	public virtual void Destroy() {
		if (this.GameObject != null) {
			this.GameObject.transform.parent = null;
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
			return (Vector2)this.GameObject.transform.position;
		}
	}
	
	public float Alpha {
		get {
			return this.alpha;	
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
	
	public virtual void UpdatePriority() {
        if (this.Parent == null) {
            this.priority = this.LocalPriority;
        } else {
			this.priority = 2 * this.LocalPriority * this.Parent.Priority;
        }
	}

	public virtual void UpdatePosition() {
		// Add velocity
		Vector2 velocity = this.Velocity;
		this.LocalPosition = new Vector2(this.LocalPosition.x + velocity.x, this.LocalPosition.y + velocity.y);
		
		// Move z position of the node, so that it reflects its render-priority
		this.GameObject.transform.position = new Vector3(
			this.GameObject.transform.position.x, 
			this.GameObject.transform.position.y, 
			this.priority
		);
	}
	public virtual void Update() {
		UpdateTransparency();
		UpdatePriority();
		UpdatePosition();

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
		Roga2dGameObjectState state = Roga2dUtils.stashState(node.GameObject);
		node.GameObject.transform.parent = this.GameObject.transform;
		node.Parent = this;
		Roga2dUtils.applyState(node.GameObject, state);
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
			return this.GameObject.transform.InverseTransformPoint(point);
	}
	
	public virtual Vector2 GetOffsetByPositionAnchor(float positionAnchorX, float positionAnchorY) {return new Vector2(0, 0);}
}