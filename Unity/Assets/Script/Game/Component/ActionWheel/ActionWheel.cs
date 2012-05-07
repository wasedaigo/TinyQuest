using UnityEngine;
using System.Collections;
using TinyQuest.Entity;
using TinyQuest.Factory.Entity;

public class ActionWheel : MonoBehaviour {
	
	public enum ActionWheelState {
		Battle,
		Menu
	};
	
	public delegate void SlotClickDelegate(int slotNo);
	public SlotClickDelegate onSlotChanged;

	public delegate void SubButtonClickDelegate();
	public SubButtonClickDelegate onSubButtonClicked;
	

	public static Vector3 INVALID_TOUCH_POINT = new Vector3(0, 0, 0);
	public UIPanel parent;
	public UIAtlas atlas;
	public GameObject rotationNode;
	public GameObject[] slots;
	public UITexture[] weaponTextures;
	public AudioClip matchSound;
	public int slotCount;
	public int wheelRadius;
	private ActionWheelState state;
	private float singleAngle;
	private Component currentController;
	private float savedRotation;
	
	// Use this for initialization
	void Start () {
		this.rotationNode = NGUITools.AddChild(this.gameObject);
		this.rotationNode.name = "RotationNode";
		this.rotationNode.transform.parent = this.transform;
		this.rotationNode.transform.localScale = new Vector3(1, 1, 1);
		this.rotationNode.transform.localPosition = new Vector3(0, 0, 0);
		
		// Setup Collide
		BoxCollider collider = this.transform.gameObject.AddComponent("BoxCollider") as BoxCollider;
		collider.size = new Vector3(100, 100, 0.1f);
	
		this.slots = new GameObject[this.slotCount];
		this.weaponTextures = new UITexture[this.slotCount];
		

		this.singleAngle = 360 / this.slotCount;
		// Setup slots
		for (int i = 0; i < this.slotCount; i++) {
			GameObject go = NGUITools.AddChild(this.rotationNode);
			go.transform.parent = this.rotationNode.transform;
			go.name = "slot" + i;
			float angle = i * 2 * Mathf.PI / this.slotCount + (this.slotCount + 2) * Mathf.PI / this.slotCount;
			float x = this.wheelRadius * Mathf.Cos(-angle);
			float y = this.wheelRadius * Mathf.Sin(-angle);
			UISprite sprite = NGUITools.AddSprite(go, atlas, "wheel_icon_1");
			sprite.name = "bg";
			sprite.MakePixelPerfect();
			sprite.color = new Color(0.1f * i, 0.1f * i, 0.1f * i);
			sprite.transform.localPosition = Vector3.one;
			go.transform.localScale = Vector3.one;
			go.transform.localPosition = new Vector3(x, y, 0);
			parent.AddWidget(sprite);
			this.slots[i] = go;
		}


		// Set up controller
		this.currentController = this.gameObject.AddComponent("ActionWheelBattleController");
		
		this.state = ActionWheelState.Battle;
	}
	
	public void OnRotate() {
		float angle = this.rotationNode.transform.localEulerAngles.z;
		for (int i = 0; i < this.slotCount; i++) {
			this.slots[i].transform.localEulerAngles = new Vector3(0, 0, -angle);
		}
	}
	
    void OnDisable()
    {
		for (int i = 0; i < this.weaponTextures.Length; i++) {
			if (this.weaponTextures[i] != null) {
				NGUITools.Destroy(this.weaponTextures[i].material);	
				NGUITools.Destroy(this.weaponTextures[i]);	
			}
		}
    }
	
	public GameObject GetSlot(int i) {
		return this.slots[i];
	}
	
	public void SetWeaponAtSlot(int i, string textureId) {
		if (this.weaponTextures[i] != null) {
			NGUITools.Destroy(this.weaponTextures[i]);	
			this.weaponTextures[i] = null;
		}

		GameObject slot = this.GetSlot(i);
		
		UITexture ut = NGUITools.AddWidget<UITexture>(slot);
		Material material = Roga2dResourceManager.getSharedMaterial(textureId, Roga2dBlendType.Unlit);
        ut.material = material;
		ut.MarkAsChanged();
		ut.MakePixelPerfect();
		ut.transform.localScale = new Vector3(ut.transform.localScale.x / 2, ut.transform.localScale.y / 2, ut.transform.localScale.z);
		ut.transform.localPosition = new Vector3(1, 1, -10);
		ut.transform.localEulerAngles = Vector3.one;
		
		this.weaponTextures[i] = ut;
	}

	private void transitionToMenu() {
		if (this.currentController != null) {
			Object.Destroy(this.currentController);
			this.currentController = null;
		}
		/*
		Transform button = this.transform.Find("ActionButton");
		button.gameObject.active = false;
		UISprite buttonImage = button.Find("Background").gameObject.GetComponent("UISprite") as UISprite;
		buttonImage.color = new Color(0,0,0,0);
		iTween.FadeTo(
			buttonImage,
			iTween.Hash("alpha", 0, "time", 0.5f)
		);*/
		
		this.savedRotation = this.rotationNode.transform.localEulerAngles.z;
		iTween.MoveTo(
			this.rotationNode,
			iTween.Hash("x", -37, "y", -44, "islocal", true, "easeType", iTween.EaseType.easeOutQuad, "time", 0.5f)
		);
		iTween.RotateTo(
			this.rotationNode,
			iTween.Hash( "z", 540, "easeType", iTween.EaseType.easeOutQuad, "time", 0.5f)
		);
		
		iTween.ScaleTo(
			this.rotationNode,
			iTween.Hash( "x", 0.666f, "y", 0.666f, "z", 0.75f, "easeType", iTween.EaseType.easeOutQuad, "time", 0.5f)
		);
	}
	
	private void transitionToBattle() {
		if (this.currentController != null) {
			Object.Destroy(this.currentController);
			this.currentController = null;
		}
		this.currentController = this.gameObject.AddComponent("ActionWheelBattleController");
		
		/*
		Transform button = this.transform.Find("ActionButton");
		button.gameObject.active = true;
		UISprite buttonImage = button.Find("Background").gameObject.GetComponent("UISprite") as UISprite;
		buttonImage.color = new Color(1,1,1,1);

		
		GameObject buttonImage = button.Find("Background").gameObject;
		
		iTween.FadeTo(
			buttonImage,
			iTween.Hash("alpha", 1.0, "time", 0.5f)
		);*/
		
		iTween.MoveTo(
			this.rotationNode,
			iTween.Hash("x", 0, "y", 0,  "islocal", true, "easeType", iTween.EaseType.easeOutQuad, "time", 0.5f)
		);
		
		iTween.RotateTo(
			this.rotationNode,
			iTween.Hash( "z", this.savedRotation, "easeType", iTween.EaseType.easeOutQuad, "time", 0.5f)
		);
		
		iTween.ScaleTo(
			this.rotationNode,
			iTween.Hash( "x", 1.0f, "y", 1.0f, "z", 1.0f, "easeType", iTween.EaseType.easeOutQuad, "time", 0.5f)
		);
	}
	
	public void setState(ActionWheelState state) {
		if (this.state != state) {
			switch (state) {
				case ActionWheelState.Menu:
					this.transitionToMenu();
				break;
				case ActionWheelState.Battle:
					this.transitionToBattle();
				break;
			}
			this.state = state;
		}
	}
	
	public float SingleAngle {
		get {
			return this.singleAngle;
		}
	}
	
	public int getSlotAt(int no) {
		return Mathf.FloorToInt(this.getCurrentAngle() / this.singleAngle);
	}
	
	public float getCurrentAngle() {
		return this.rotationNode.transform.localEulerAngles.z + 0.001f; // HACK: Fix floating point error
	}
	

	public void OnActionButtonClick() {
		if (this.onSlotChanged != null) {
			this.onSlotChanged(this.getSlotAt(0));	
		}
	}
	
	public void OnMenuButtonClick() {
		if (this.onSubButtonClicked != null) {
			this.onSubButtonClicked();
		}
		/*
		switch (this.state) {
			case ActionWheelState.Menu:
				this.setState(ActionWheelState.Battle);
			break;
			case ActionWheelState.Battle:
				this.setState(ActionWheelState.Menu);
			break;
		}
		*/
	}
}
