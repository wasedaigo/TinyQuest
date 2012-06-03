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
	public float[] StartPositionY = new float[3];
	public GameObject CompositeCommandButton;
	public GameObject CompositeToggleButton;

	private BoxCollider boxCollider;
	private ZoneEntity zoneEntity;
	private const float MoveInStartPositionX = 400;
	public void Start() {
		this.boxCollider = this.GetComponent<BoxCollider>();
		
		for (int i = 0; i < this.StartPositionY.Length; i++) {
			this.StartPositionY[i] = this.Buttons[i].transform.localPosition.y;
		}
	}
	
	private CompositeData compositeData;
	public void SkillDrawn(SkillEntity[] skillEntities, CompositeData compositeData) {
		this.compositeData = compositeData;
		for (int i = 0; i < skillEntities.Length; i++) {
			this.SetSkillAtSlot(i, skillEntities[i]);
			if (skillEntities[i] != null) {
				GameObject button = this.Buttons[i];
				Vector3 pos = button.transform.localPosition;
				button.transform.localPosition = new Vector3(MoveInStartPositionX, pos.y, pos.z);
				iTween.MoveTo(button, iTween.Hash("x", 0, "time", 0.5, "easetype",iTween.EaseType.linear, "oncompletetarget", this.gameObject, "oncomplete", "onCompleteDraw"));
			}
		}
		
		this.boxCollider.enabled = true;
	}

	public void SetSkillAtSlot(int slotIndex, SkillEntity skillEntity) {
		if (skillEntity == null) {return;}
		// Setup button
		GameObject button = this.Buttons[slotIndex];
		UIImageButton imageButton = button.GetComponent<UIImageButton>();
		
		
		UISprite background = button.transform.FindChild("Background").GetComponent<UISprite>();
		switch (skillEntity.GetCompositeType()) {
			case(SkillCompositeType.Single):
				imageButton.normalSprite = "single_command_bar";
				imageButton.hoverSprite = "single_command_bar";
				imageButton.pressedSprite = "single_command_bar_on";
				background.spriteName = "single_command_bar";
			break;
			case(SkillCompositeType.Double):
				imageButton.normalSprite = "double_command_bar";
				imageButton.hoverSprite = "double_command_bar";
				imageButton.pressedSprite = "double_command_bar_on";
				background.spriteName = "double_command_bar";
			break;
			case(SkillCompositeType.Triple):
				imageButton.normalSprite = "triple_command_bar";
				imageButton.hoverSprite = "triple_command_bar";
				imageButton.pressedSprite = "triple_command_bar_on";
				background.spriteName = "triple_command_bar";
			break;
		}
		
		// Setup Label
		UILabel label = button.transform.FindChild("Label").GetComponent<UILabel>();
		label.text = skillEntity.MasterSkill.GetName();
		
		// Setup weapon icon
		Transform icon = button.transform.FindChild("Icon");
		if (icon.GetChildCount() > 0) {
			Destroy(icon.GetChild(0).gameObject);
		}
		icon.DetachChildren();
		
		/*
		string textureId = skillEntity.OwnerWeapon.GetMasterWeapon().GetUIImagePath();
		UITexture ut = NGUITools.AddWidget<UITexture>(icon.gameObject);
		Material material = Roga2dResourceManager.getSharedMaterial(textureId, Roga2dBlendType.Unlit);
        ut.material = material;
		ut.MarkAsChanged();
		ut.MakePixelPerfect();
		float scale = 0.5f;
		ut.transform.localScale = new Vector3(ut.transform.localScale.x * scale, ut.transform.localScale.y * scale, ut.transform.localScale.z);
		ut.transform.localPosition = Vector3.zero;*/
	}
	
	private void onCompleteDraw() {
		if (this.compositeData == null) {
			this.boxCollider.enabled = false;
		} else {
			this.boxCollider.enabled = true;
			int firstSkillSlot = -1;
			for (int i = 0; i < this.compositeData.BaseSkills.Length; i++) {
				int baseSkill = this.compositeData.BaseSkills[i];
				if (baseSkill > 0) {
					if (firstSkillSlot >= 0) {
						GameObject srcbutton = this.Buttons[i];
						GameObject dstButton = this.Buttons[firstSkillSlot];
						Vector3 pos = dstButton.transform.position;
						iTween.MoveTo(srcbutton, iTween.Hash("y", pos.y, "time", 0.5, "easetype",iTween.EaseType.linear, "oncompletetarget", this.gameObject, "oncomplete", "onCompleteComposition", "oncompleteparams", i));
						
					} else {
						this.SetSkillAtSlot(i, SkillFactory.Instance.Build(compositeData.Skill));
						firstSkillSlot = i;
					}
				}
			}
			
		}
		this.compositeData = null;
	}
	
	private void onCompleteComposition(int slotIndex) {
		this.boxCollider.enabled = false;
		GameObject button = this.Buttons[slotIndex];
		button.transform.localPosition = new Vector3(MoveInStartPositionX, this.StartPositionY[slotIndex], button.transform.localPosition.z);
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
	
	private void discard(int slotIndex) {
		GameObject button = this.Buttons[slotIndex];
		this.boxCollider.enabled = true;
		iTween.MoveTo(button, iTween.Hash("x", -2, "time", 0.5, "easetype",iTween.EaseType.linear, "oncompletetarget", this.gameObject, "oncomplete", "onCompleteDiscard"));	
	}
	
	private void click(int slotIndex) {
		this.discard(slotIndex);
		
		CombatController controller = this.ZoneStageController.GetComponent<CombatController>();
		if (controller != null) {
			controller.InvokeCommand(slotIndex);
		}
	}
}