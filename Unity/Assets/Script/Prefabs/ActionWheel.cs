using UnityEngine;
using System.Collections;

public class ActionWheel : MonoBehaviour {
	public delegate void SlotClickDelegate(int slotNo);
	public SlotClickDelegate onSlotChanged;

	public static Vector3 INVALID_TOUCH_POINT = new Vector3(0, 0, 0);
	public GameObject parent;
	public UIAtlas atlas;
	public GameObject rotationNode;
	public GameObject[] slots;
	public AudioClip matchSound;
	public int slotCount;
	public int wheelRadius;
	private float singleAngle;
	private ActionWheelBattleController battleController;
	
	// Use this for initialization
	void Start () {
		this.rotationNode = new GameObject();
		this.rotationNode.name = "RotationNode";
		this.rotationNode.transform.parent = parent.transform;
		this.rotationNode.transform.localScale = new Vector3(1, 1, 1);
		this.rotationNode.transform.localPosition = new Vector3(0, 0, 0);
		
		// Setup Collide
		BoxCollider collider = this.transform.gameObject.AddComponent("BoxCollider") as BoxCollider;
		collider.size = new Vector3(100, 100, 0.1f);
		
		// Setup slots
		for (int i = 0; i < this.slotCount; i++) {
			float x = this.wheelRadius * Mathf.Cos(i * 2 * Mathf.PI / this.slotCount);
			float y = this.wheelRadius * Mathf.Sin(i * 2 * Mathf.PI / this.slotCount);
			UISprite sprite = NGUITools.AddSprite(this.rotationNode, atlas, "wheel_icon_1");
			sprite.name = "slot" + i;
			sprite.MakePixelPerfect();
			sprite.color = new Color(0.1f * i, 0.1f * i, 0.1f * i);
			//sprite.transform.localScale = new Vector3(1, 1, 1);
			sprite.transform.localPosition = new Vector3(x, y, 0);
		}

		this.singleAngle = 360 / this.slotCount;

		// Set up controller
		this.gameObject.AddComponent("ActionWheelBattleController");
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
	
	// Update is called once per frame
	void Update () {
	}

	public void OnActionButtonClick() {
		if (this.onSlotChanged != null) {
			this.onSlotChanged(this.getSlotAt(0));	
		}
	}
	
	public void OnMenuButtonClick() {
		
		if (this.onSlotChanged != null) {
			this.onSlotChanged(this.getSlotAt(0));	
		}
	}
}
