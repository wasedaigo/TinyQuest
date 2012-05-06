using UnityEngine;
using System.Collections;

public class ActionWheelBattleController : MonoBehaviour {
	private bool isPressed;
	private int lastSlot;
	
	private Vector3 lastTouchPoint;
	private Vector2 startTouchPoint;
	private float angularVelocity;
	private float targetRotation;
	private float effectMagnitude;
	private bool isWheelMoved;
	private ActionWheel wheel;
	
	public void Start() {
		this.lastTouchPoint = ActionWheel.INVALID_TOUCH_POINT;
	}
	
	private ActionWheel GetWheel() {
		if (this.wheel == null) {
			this.wheel = this.gameObject.GetComponent("ActionWheel") as ActionWheel;	
		}
		return this.wheel;
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
			Vector3 point = hitInfo.point - this.transform.position;
			point.z = 0;
			float magnitude = point.magnitude;
			if (1.2 < magnitude && magnitude < 2.5) {
				return point;
			} else {
				return ActionWheel.INVALID_TOUCH_POINT;
			}
		}
		return ActionWheel.INVALID_TOUCH_POINT;
	}
	
	public void OnWheelRotationComplete(){
      this.GetWheel().audio.PlayOneShot(this.GetWheel().matchSound);
	}
	
	// Update is called once per frame
	public void Update () {
		/*
		float currentAngle = this.GetWheel().getCurrentAngle();
		float singleAngle = this.GetWheel().SingleAngle;
		// Setup slots
		for (int i = 0; i < this.GetWheel().slotCount; i++) {
			float angle = (currentAngle + (i + 1) * singleAngle) % 360;
			GameObject slot = this.GetWheel().GetSlot(i);
			float scale = Mathf.Sin(angle * Mathf.PI / 180) * 21;
			slot.transform.localScale = new Vector3(scale, scale, scale);
		}*/

		if (Input.GetMouseButtonDown(0)) {
			Vector3 pos = GetTouchPosition(Input.mousePosition);
			if (this.lastTouchPoint != pos) {
				this.isPressed = true;
				iTween.Stop();
				this.lastSlot = this.GetWheel().getSlotAt(0);
			}
		}
		
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
				float angularVelocity = magnitude * 50;
				this.effectMagnitude += magnitude * Time.deltaTime;
				
				if (Mathf.Abs(this.effectMagnitude) > 0.00075f) {
					this.GetWheel().rotationNode.transform.Rotate(new Vector3(0, 0, angularVelocity));
					this.GetWheel().OnRotate();
					this.isWheelMoved = true;
				}
				Clip(ref this.effectMagnitude, -0.01f, 0.01f);
				
				int slot = this.GetWheel().getSlotAt(0);
				if (slot != this.lastSlot) {
					this.audio.PlayOneShot(this.GetWheel().matchSound);
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

		if (wheelRotationFinished && this.isPressed) {
			this.isPressed = false;
			this.lastTouchPoint = ActionWheel.INVALID_TOUCH_POINT;
			
			if (this.isWheelMoved) {
				int skipNum = Mathf.FloorToInt(this.effectMagnitude * 200);
	
				skipNum += 1;
				int num = Mathf.FloorToInt(this.GetWheel().getCurrentAngle() /  this.GetWheel().SingleAngle);
				float targetAngle = (num + skipNum) * this.GetWheel().SingleAngle;
				
				iTween.RotateTo(
					this.GetWheel().rotationNode,
					iTween.Hash(
						"z", targetAngle, 
						"easeType", iTween.EaseType.linear, 
						"time", 0.3f, 
						"onupdate", "OnRotate",
						"onupdatetarget", this.GetWheel().gameObject,
						"oncomplete", "OnWheelRotationComplete",
						"oncompletetarget", this.gameObject
					)
				);
			}
			this.effectMagnitude = 0;
			this.isWheelMoved = false;
		}
	}
}
