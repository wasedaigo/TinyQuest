using UnityEngine;
using System.Collections;

public class ScrollView : MonoBehaviour {
	
	public GameObject ScrollTarget;
	public int PageCount;
	public float PageSize;
	
	private UIDragObject dragObject;
	private BoxCollider scrollCollider;
	private float touchStartTime;
	private GameObject pressedButton;
	private Vector3 lastMousePosition;
	private bool isMousePressed;
	private int pageNo;

	void Start() {
		this.touchStartTime = float.MaxValue;
		this.pressedButton = null;
		this.dragObject = this.GetComponent<UIDragObject>();
		this.scrollCollider = this.GetComponent<BoxCollider>();
		this.pageNo = 0;
	}
	

	private GameObject GetCollideButton(Ray ray) {
		RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity);
		foreach (RaycastHit hit in hits) {
			GameObject go = hit.collider.transform.gameObject;
			if (go.GetComponent<ObjectClickHandler>() != null) {
				return go;
			}
		}

		return null;
	}

	void Update() {
		float currentTime = Time.realtimeSinceStartup;

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (this.isMousePressed) {
			if (this.pressedButton == null) {
				if (currentTime - this.touchStartTime > 0.25f) {
					this.touchStartTime = float.MaxValue;
					this.pressedButton = this.GetCollideButton(ray);
					if (this.pressedButton != null) {
						this.pressedButton.SendMessage("OnPress", true);
					}
					
					this.dragObject.enabled = false;
				} else {
					Vector3 diff = Input.mousePosition - this.lastMousePosition;
					this.lastMousePosition = Input.mousePosition;
				
					if (diff.magnitude > 1) {
						this.touchStartTime = currentTime;
					}
				}
			}
		}

		if(Input.GetMouseButtonDown(0)) {
			RaycastHit hit;
			bool collide = this.scrollCollider.Raycast(ray, out hit, Mathf.Infinity);
			if (collide) {
				this.isMousePressed = true;
				this.touchStartTime = currentTime;
				this.lastMousePosition = Input.mousePosition;
			}
		}

		if (Input.GetMouseButtonUp(0)) {
			if (this.pressedButton != null) {
				GameObject currentPressedButton = this.GetCollideButton(ray);
				if (this.pressedButton == currentPressedButton) {
					ObjectClickHandler clickHandler = this.pressedButton.GetComponent<ObjectClickHandler>();
					clickHandler.OnClick();
					//this.click(this.pressedSlot);
				}

				this.touchStartTime = float.MaxValue;
				this.pressedButton.SendMessage("OnPress", false);
				
				this.dragObject.enabled = true;
				this.pressedButton = null;
			}
			this.isMousePressed = false;
			
			
			float movedDistance = this.ScrollTarget.transform.localPosition.x + this.pageNo * this.PageSize;
			Debug.Log(movedDistance);
			if (movedDistance < -this.PageSize / 4) {
				this.pageNo += 1;
			}
			if (movedDistance > this.PageSize / 4) {
				this.pageNo -= 1;
			}
			if (this.pageNo < 0) {
				this.pageNo = 0;	
			}
			if (this.pageNo > this.PageCount - 1) {
				this.pageNo = this.PageCount - 1;
			}
			
			Debug.Log(this.pageNo);
			SpringPosition.Begin(this.ScrollTarget, new Vector3(-this.pageNo * this.PageSize, 0, 0), 10.0f);
		}
	}
}
