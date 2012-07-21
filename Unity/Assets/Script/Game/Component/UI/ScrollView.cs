using UnityEngine;
using System.Collections;
using TinyQuest;

public class ScrollView : MonoBehaviour {
	
	public GameObject ScrollTarget;
	public int PageCount;
	public float PageSize;
	public BoxCollider scrollCollider;
	
	private float touchStartTime;
	private GameObject pressedButton;
	private Vector3 touchStartPosition;
	private Vector3 lastMousePosition;
	private bool isMousePressed;
	private int pageNo;
	private const int LimitSize = 320;
	private const int InputThreshold = 16;

	void Start() {
		this.touchStartTime = float.MaxValue;
		this.pressedButton = null;
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
					this.pressedButton = this.GetCollideButton(ray);
					if (this.pressedButton != null) {
						this.pressedButton.SendMessage("OnPress", true);
					}
			}
			
			Vector3 pos = this.ScrollTarget.transform.localPosition;
			float changeAmount = (Input.mousePosition.x - this.touchStartPosition.x) * Config.ActualLogicalRatio;
			if (changeAmount < -LimitSize) {changeAmount = -LimitSize;}
			if (changeAmount > LimitSize) {changeAmount = LimitSize;}
			
			if (Mathf.Abs(changeAmount) > InputThreshold) {
				float newX = changeAmount - pageNo * PageSize;
				this.ScrollTarget.transform.localPosition = new Vector3(newX, pos.y, pos.z);
			}
		}

		if(Input.GetMouseButtonDown(0)) {
			RaycastHit hit;
			bool collide = this.scrollCollider.Raycast(ray, out hit, Mathf.Infinity);
			if (collide) {
				this.isMousePressed = true;
				this.touchStartTime = currentTime;
				this.lastMousePosition = Input.mousePosition;
				this.touchStartPosition = Input.mousePosition;
			}
		}

		if (Input.GetMouseButtonUp(0)) {
			float tmp = this.ScrollTarget.transform.localPosition.x % PageSize;
			if (tmp != 0){
				this.pressedButton = null;
			}
			if (this.pressedButton != null) {
				GameObject currentPressedButton = this.GetCollideButton(ray);
				if (this.pressedButton != null && this.pressedButton == currentPressedButton) {
					ObjectClickHandler clickHandler = this.pressedButton.GetComponent<ObjectClickHandler>();
					clickHandler.OnDelayClick();
				}

				this.touchStartTime = float.MaxValue;
				this.pressedButton.SendMessage("OnPress", false);
				
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
			
			SpringPosition.Begin(this.ScrollTarget, new Vector3(-this.pageNo * this.PageSize, 0, 0), 10.0f);
		}
	}
}
