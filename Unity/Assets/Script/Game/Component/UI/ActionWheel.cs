using UnityEngine;
using System.Collections;
using TinyQuest.Data;
using TinyQuest.Core;
using TinyQuest.Object;

public class ActionWheel : MonoBehaviour {
	
	public enum State {
		Combat,
		Progress
	};
	
	public delegate void SlotClickDelegate(int slotNo);
	public SlotClickDelegate onActionButtonClicked;
	public static Vector3 INVALID_TOUCH_POINT = new Vector3(0, 0, 0);
	public UIAtlas atlas;
	public AudioClip matchSound;
	public int slotCount;
	public int wheelRadius;
	
	private GameObject[] slots;
	private State state;
	private float singleAngle;
	private float savedRotation;
	private bool isPressed;
	private int lastSlot;
	
	private GameObject rotationNode;
	private Vector3 lastTouchPoint;
	private Vector2 startTouchPoint;
	private float angularVelocity;
	private float effectMagnitude;
	private bool isWheelMoved;
	
	// Use this for initialization
	void Start () {
		
		this.lastTouchPoint = ActionWheel.INVALID_TOUCH_POINT;
		
		this.rotationNode = NGUITools.AddChild(this.gameObject);
		this.rotationNode.name = "RotationNode";
		this.rotationNode.transform.parent = this.transform;
		this.rotationNode.transform.localScale = new Vector3(1, 1, 1);
		this.rotationNode.transform.localPosition = new Vector3(0, 0, 0);
		
		// Setup Collide
		//BoxCollider collider = this.transform.gameObject.AddComponent("BoxCollider") as BoxCollider;
		//collider.size = new Vector3(100, 100, 0.1f);
	
		this.slots = new GameObject[this.slotCount];
		

		this.singleAngle = 360 / this.slotCount;
		// Setup slots
		for (int i = 0; i < this.slotCount; i++) {
			GameObject go = NGUITools.AddChild(this.rotationNode);
			go.transform.parent = this.rotationNode.transform;
			go.name = "slot" + i;
			float angle = i * 2 * Mathf.PI / this.slotCount + (this.slotCount + 1) * Mathf.PI / this.slotCount;
			float x = this.wheelRadius * Mathf.Cos(-angle);
			float y = this.wheelRadius * Mathf.Sin(-angle);
			go.transform.localScale = Vector3.one;
			go.transform.localPosition = new Vector3(x, y, 0);
			this.slots[i] = go;
			this.SetPuppetAtSlot(i, i + 1);
		}

	}

	public void UpdateIconAngle() {
		float angle = this.rotationNode.transform.localEulerAngles.z;
		for (int i = 0; i < this.slotCount; i++) {
			this.slots[i].transform.localEulerAngles = new Vector3(0, 0, -angle);
		}
	}
	
	public GameObject GetSlot(int i) {
		return this.slots[i];
	}

	public void SetPuppetAtSlot(int i, int puppetId) {
		FaceIcon actor = new FaceIcon(puppetId);
		actor.Transform.parent = this.slots[i].transform;
		actor.LocalPosition = new Vector2(0, 0);
		actor.LocalRotation = 180.0f;
		actor.LocalScale = new Vector2(-24, 24);
		Utils.SetLayerRecursively(actor.Transform, 5);
	}
	
	
	public void Clip(ref float value, float start, float end) {
		if (value < start) {
			value = start;
		}
		if (value > end) {
			value = end;
		}
	}
	
	private Vector3 GetTouchPosition(Vector3 pos) {
		Ray ray = Camera.main.ScreenPointToRay(pos);
		RaycastHit hitInfo;
		if (Physics.Raycast( ray, out hitInfo ))
		{
			Vector3 point = new Vector3(hitInfo.point.x - this.transform.position.x, hitInfo.point.y + this.transform.position.y, 0);
			point.z = 0;
			float magnitude = point.magnitude;
			if (0.9f < magnitude && magnitude < 1.8f) {
				return point;
			} else {
				return ActionWheel.INVALID_TOUCH_POINT;
			}
		}
		return ActionWheel.INVALID_TOUCH_POINT;
	}
	
	public void OnWheelRotationComplete(){
      //this.audio.PlayOneShot(this.matchSound);
	}

	private void UpdateRotationStart() {
		if (Input.GetMouseButtonDown(0)) {
			Vector3 pos = GetTouchPosition(Input.mousePosition);
			if (this.lastTouchPoint != pos) {
				this.isPressed = true;
				iTween.Stop();
				this.lastSlot = this.getSlotAt(0);
			}
		}
	}

	private bool UpdatePressMove() {
		bool wheelRotationFinished = false;
		if (this.isPressed) {
			this.effectMagnitude *= 0.8f;
			Vector3 pos = GetTouchPosition(Input.mousePosition);
			if (this.lastTouchPoint != pos && pos != ActionWheel.INVALID_TOUCH_POINT) {
				Vector3 deltaVector = (pos - this.lastTouchPoint);
				Vector3 directionalVector = new Vector3(-pos.y, pos.x, 0);
				directionalVector.Normalize();
				float magnitude = Vector3.Dot(directionalVector, deltaVector);
				Clip(ref magnitude, -0.25f, 0.25f);
				float angularVelocity = -magnitude * 75;
				this.effectMagnitude -= magnitude * Time.deltaTime;
				if (Mathf.Abs(this.effectMagnitude) > 0.00075f) {
					this.rotationNode.transform.Rotate(new Vector3(0, 0, angularVelocity));
					this.UpdateIconAngle();
					this.isWheelMoved = true;
				}
				Clip(ref this.effectMagnitude, -0.01f, 0.01f);
				
				int slot = this.getSlotAt(0);
				if (slot != this.lastSlot) {
					//this.audio.PlayOneShot(this.matchSound);
				}
				this.lastSlot = slot;
			}

			if (pos == ActionWheel.INVALID_TOUCH_POINT){
				wheelRotationFinished = true;
			}
			this.lastTouchPoint = pos;
		}
		
		if (Input.GetMouseButtonUp(0)) {
			wheelRotationFinished = true;
		}

		return wheelRotationFinished;
	}
	
	private void UpdateRotationFinish() {
		this.isPressed = false;
		this.lastTouchPoint = ActionWheel.INVALID_TOUCH_POINT;
		
		if (this.isWheelMoved) {
			int skipNum = Mathf.FloorToInt(this.effectMagnitude * 200);

			skipNum += 1;
			int num = Mathf.FloorToInt(this.getCurrentAngle() /  this.singleAngle);
			float targetAngle = (num + skipNum) * this.singleAngle;
			
			float time = 0.1f + 0.2f * Mathf.Abs(targetAngle - this.getCurrentAngle()) / this.singleAngle;
			
			iTween.RotateTo(
				this.rotationNode,
				iTween.Hash(
					"z", targetAngle, 
					"easeType", iTween.EaseType.linear, 
					"time", time, 
					"oncomplete", "OnWheelRotationComplete",
					"oncompletetarget", this.gameObject
				)
			);
		}
		this.effectMagnitude = 0;
		this.isWheelMoved = false;
	}
	
	public void Update () {		
		this.UpdateRotationStart();
		bool wheelRotationFinished = this.UpdatePressMove();
		if (wheelRotationFinished && this.isPressed) {
			this.UpdateRotationFinish();
		}
		this.UpdateIconAngle();
	}
	
	public void SetState(State state) {
		if (this.state != state) {
			switch (state) {
				case State.Combat:
					this.gameObject.SetActiveRecursively(true);
				break;
				case State.Progress:
					this.gameObject.SetActiveRecursively(false);
				break;
			}
			this.state = state;
		}
	}
	
	public int getSlotAt(int no) {
		return Mathf.FloorToInt(this.getCurrentAngle() / this.singleAngle);
	}
	
	public float getCurrentAngle() {
		return this.rotationNode.transform.localEulerAngles.z + 0.001f; // HACK: Fix floating point error
	}
	
}
