using JsonFx.Json;
using UnityEngine;
using System.Collections.Generic;
using TinyQuest.Data.Cache;
using TinyQuest.Data;
using TinyQuest.Core;
using TinyQuest.Entity;
using TinyQuest.Factory.Entity;
using TinyQuest.Model;
using TinyQuest.Object;

public class CombatPanelController : MonoBehaviour {
	
	public UIAtlas ZoneAtlas;
	public GameObject CommandButtonPrefab;
	public ZoneStageController ZoneStageController;
	public GameObject ScrollTarget;
	
	
	private BattlerEntity userBattlerEntity;
	private ZoneEntity zoneEntity;
	private float touchStartTime;
	private int pressedSlot;
	private Vector3 lastMousePosition;
	
	private List<GameObject> panels = new List<GameObject>(); 
	private UIDragObject dragObject;
	private BoxCollider scrollCollider;
	private GameObject[] buttons = new GameObject[3];
	private Dictionary<GameObject, int> buttonDictionary = new Dictionary<GameObject, int>();
	private bool isMousePressed;

	// Use this for initialization
	void Start() {
		this.zoneEntity = ZoneFactory.Instance.Build();
		this.userBattlerEntity = this.zoneEntity.GetPlayerBattler();
		
		SkillEntity[] handSkills = this.userBattlerEntity.GetHand();
		for (int i = 0; i < handSkills.Length; i++) {
			SkillEntity skill = handSkills[i];
			this.SetSkillAtSlot(i, skill);
		}
		

		this.touchStartTime = float.MaxValue;
		this.pressedSlot = -1;
		
		this.dragObject = this.GetComponent<UIDragObject>();
		this.scrollCollider = this.GetComponent<BoxCollider>();
	}

	private int GetCollideSlot(Ray ray) {
		RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity);
		foreach (RaycastHit hit in hits) {
			GameObject go = hit.collider.transform.gameObject;
			if (go.GetComponent<UIImageButton>() != null) {
				if (buttonDictionary.ContainsKey(go)) {
					return buttonDictionary[go];	
				}
			}
		}
		
		return -1;
	}
	
	void Update() {
		SpringPosition springPosition = this.ScrollTarget.GetComponent<SpringPosition>();
		if (springPosition != null) {
			springPosition.enabled = true;	
		}
		
		float currentTime = Time.realtimeSinceStartup;

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (this.isMousePressed) {
			if (this.pressedSlot == -1) {
				if (currentTime - this.touchStartTime > 0.25f) {
					this.touchStartTime = float.MaxValue;
					this.pressedSlot = this.GetCollideSlot(ray);
					if (this.pressedSlot >= 0) {
						this.buttons[this.pressedSlot].SendMessage("OnPress", true);
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
			if (this.pressedSlot >= 0) {
				int currentPressedSlot = this.GetCollideSlot(ray);
				if (this.pressedSlot == currentPressedSlot) {
					this.click(this.pressedSlot);
				}

				this.touchStartTime = float.MaxValue;
				this.buttons[this.pressedSlot].SendMessage("OnPress", false);
				
				this.dragObject.enabled = true;
				this.pressedSlot = -1;
			}
			this.isMousePressed = false;
			SpringPosition.Begin(this.ScrollTarget, new Vector3(0, 0, 0), 10.0f);
		}
	}

	public void SetSkillAtSlot(int slotNo, SkillEntity skillEntity) {
		float x = 0;
		float y = 70 - slotNo * 60;
		
		// Setup button
		GameObject button = Object.Instantiate(CommandButtonPrefab) as GameObject;
		button.transform.parent = this.transform;
		button.transform.localPosition = new Vector3(x, y, 0);
		button.transform.localScale = new Vector3(3, 3, 1);
		
		buttonDictionary.Add(button, slotNo);
		
		// Setup button states
		UIImageButton imageButton = button.GetComponent<UIImageButton>();
		string prefix = "single";
		imageButton.target.spriteName = prefix + "_command_bar";
		imageButton.normalSprite = prefix + "_command_bar";
		imageButton.hoverSprite = prefix + "_command_bar";
		imageButton.pressedSprite = prefix + "_command_bar_on";
		
		// Setup Label
		UILabel label = button.transform.FindChild("Label").GetComponent<UILabel>();
		label.text = skillEntity.MasterSkill.GetName();
		
		// Setup weapon icon
		Transform icon = button.transform.FindChild("Icon");
		string textureId = skillEntity.OwnerWeapon.GetMasterWeapon().GetUIImagePath();
		UITexture ut = NGUITools.AddWidget<UITexture>(icon.gameObject);
		Material material = Roga2dResourceManager.getSharedMaterial(textureId, Roga2dBlendType.Unlit);
        ut.material = material;
		ut.MarkAsChanged();
		ut.MakePixelPerfect();
		float scale = 0.5f;
		ut.transform.localScale = new Vector3(ut.transform.localScale.x * scale, ut.transform.localScale.y * scale, ut.transform.localScale.z);
		ut.transform.localPosition = Vector3.zero;
		
		this.buttons[slotNo] = button;
	}
	
	private void click(int slot) {
		CombatController controller = this.ZoneStageController.GetComponent<CombatController>();
		if (controller != null) {
			controller.InvokeCommand(slot);
		}
	}
}