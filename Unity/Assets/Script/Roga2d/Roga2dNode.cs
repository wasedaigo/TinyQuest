using UnityEngine;
using System.Collections.Generic;

public class Roga2dNode {
	
	public Vector2 Origin;
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
	
	public float LocalRotation {
		get {
			return this.GameObject.transform.localEulerAngles.z;
		}
		set {
			//this.GameObject.transform.rotation = Quaternion.identity;
			//this.GameObject.transform.Rotate(new Vector3(0, 0, value));
			
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
	
	public Roga2dNode() {
		Initialize();
	}
	
	public Roga2dNode(GameObject go) {
		this.GameObject = go;
		Initialize();
	}
	
	private void Initialize() {
		this.children = new List<Roga2dNode>();
		
		if (this.GameObject == null) {
			this.GameObject = new GameObject("Node");
		} else {
			this.LocalPosition = this.GameObject.transform.position;
		}

		this.LocalAlpha = 1.0f;
		this.LocalPriority = 0.5f;
		this.alpha = 1.0f;
		this.priority = 0.5f;
		
	}
	
	public virtual void Destroy() {
		this.Parent = null;	
		if (this.GameObject) {
			this.GameObject.transform.parent = null;
			Object.Destroy(this.GameObject);
			this.GameObject = null;
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
	
	public virtual void Update() {
		this.UpdateAttributes();

		
		foreach(Roga2dNode node in this.children) {
			node.Update();
		}
	}

	private void UpdateAttributes() {
        if (this.Parent != null) {
            this.alpha = this.LocalAlpha * this.Parent.Alpha;
            this.priority = 2 * this.LocalPriority * this.Parent.Priority;
        } else {
            this.alpha = this.LocalAlpha;
            this.priority = this.LocalPriority;
        }
		
		// Add velocity
		Vector2 velocity = this.Velocity;
		if (this.Parent != null) {
			//velocity = this.Parent.InverseTransformPoint(this.Velocity);
			//Debug.Log(this.Velocity +" : "+velocity);
		}
		this.LocalPosition = new Vector2(this.LocalPosition.x + velocity.x, this.LocalPosition.y + velocity.y);
	
		// Move z position of the node, so that it reflects its render-priority
		this.GameObject.transform.position = new Vector3(this.GameObject.transform.position.x, this.GameObject.transform.position.y, this.priority);

		foreach(Roga2dNode node in this.children) {
			node.UpdateAttributes();
		}
    }
	
	public void AddChild(Roga2dNode node) {
		if (node.Parent != null) {
			Debug.LogError("Node cannot have multiple parent");
		}

		this.children.Add(node);
		
		Roga2dGameObjectState state = Roga2dUtils.stashState(node.GameObject);
		node.GameObject.transform.parent = this.GameObject.transform;
		node.Parent = this;
		Roga2dUtils.applyState(node.GameObject, state);
	}
	
    public void RemoveChild(Roga2dNode node) {
		if (node != null) {
	        this.children.Remove(node);
			node.Destroy();
		}
    }
	
	public void RemoveAllChildren() {
		foreach(Roga2dNode node in this.children) {
			node.Destroy();
		}
		this.children.Clear();
	}

	public Vector2 InverseTransformPoint(Vector2 point) {
			return this.GameObject.transform.InverseTransformPoint(point);
	}
	
	public virtual Vector2 GetOffsetByPositionAnchor(float positionAnchorX, float positionAnchorY) {return new Vector2(0, 0);}
}