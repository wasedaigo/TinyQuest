using JsonFx.Json;
using UnityEngine;
using System.Collections.Generic;
using TinyQuest.Data.Cache;
using TinyQuest.Data;
using TinyQuest.Core;
using TinyQuest.Entity;
using TinyQuest.Factory.Entity;
using TinyQuest.Object;

public class CombatPanelController : MonoBehaviour {
	public GameObject CommandButtonPrefab;
	public ZoneStageController ZoneStageController;
	public GameObject[] Buttons = new GameObject[3];
	public GameObject CompositeCommandButton;
	public GameObject CompositeToggleButton;

	private BoxCollider boxCollider;
	private ZoneEntity zoneEntity;
	
	public void Start() {
		this.boxCollider = this.GetComponent<BoxCollider>();
	}
	
	public void SkillDrawn(SkillEntity[] skillEntities) {
		for (int i = 0; i < skillEntities.Length; i++) {
			this.SetSkillAtSlot(i, skillEntities[i]);	
		}
	}

	public void SetCompositeSkill(int[] commandIndexes, SkillEntity skillEntity) {
		// Setup Label
		UILabel label = this.CompositeCommandButton.transform.FindChild("Label").GetComponent<UILabel>();
		label.text = skillEntity.MasterSkill.GetName();
	}

	public void SetSkillAtSlot(int commandIndex, SkillEntity skillEntity) {
		if (skillEntity == null) {return;}
		// Setup button
		GameObject button = this.Buttons[commandIndex];
		
		// Setup Label
		UILabel label = button.transform.FindChild("Label").GetComponent<UILabel>();
		label.text = skillEntity.MasterSkill.GetName();
		
		// Setup weapon icon
		Transform icon = button.transform.FindChild("Icon");
		if (icon.GetChildCount() > 0) {
			Destroy(icon.GetChild(0).gameObject);
		}
		icon.DetachChildren();
		string textureId = skillEntity.OwnerWeapon.GetMasterWeapon().GetUIImagePath();
		UITexture ut = NGUITools.AddWidget<UITexture>(icon.gameObject);
		Material material = Roga2dResourceManager.getSharedMaterial(textureId, Roga2dBlendType.Unlit);
        ut.material = material;
		ut.MarkAsChanged();
		ut.MakePixelPerfect();
		float scale = 0.5f;
		ut.transform.localScale = new Vector3(ut.transform.localScale.x * scale, ut.transform.localScale.y * scale, ut.transform.localScale.z);
		ut.transform.localPosition = Vector3.zero;
		
		Vector3 pos = button.transform.localPosition;
		button.transform.localPosition = new Vector3(400, pos.y, pos.z);
		iTween.MoveTo(button, iTween.Hash("x", 0, "time", 0.5, "easetype",iTween.EaseType.linear, "oncompletetarget", this.gameObject, "oncomplete", "onCompleteDraw"));
		
		this.boxCollider.enabled = true;
	}
	
	private void onCompleteDraw() {
		this.boxCollider.enabled = false;
	}
	
	private void onCompleteDiscard() {
	}
	
	public void Slot0Clicked() {
		this.click(0);	
	}

	public void Slot1Clicked() {
		this.click(1);	
	}
	
	public void Slot2Clicked() {
		this.click(2);	
	}
	
	private void click(int commandIndex) {
		GameObject button = this.Buttons[commandIndex];
		this.boxCollider.enabled = true;
		iTween.MoveTo(button, iTween.Hash("x", -2, "time", 0.5, "easetype",iTween.EaseType.linear, "oncompletetarget", this.gameObject, "oncomplete", "onCompleteDiscard"));
		
		CombatController controller = this.ZoneStageController.GetComponent<CombatController>();
		if (controller != null) {
			controller.InvokeCommand(commandIndex);
		}
	}
}