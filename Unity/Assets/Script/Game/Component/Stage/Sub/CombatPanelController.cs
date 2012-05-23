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
	public GameObject CommandButtonPrefab;
	public ZoneStageController ZoneStageController;

	private BattlerEntity userBattlerEntity;
	private ZoneEntity zoneEntity;

	// Use this for initialization
	void Start() {
		this.zoneEntity = ZoneFactory.Instance.Build();
		this.userBattlerEntity = this.zoneEntity.GetPlayerBattler();
		
		SkillEntity[] handSkills = this.userBattlerEntity.GetHand();
		for (int i = 0; i < handSkills.Length; i++) {
			SkillEntity skill = handSkills[i];
			this.SetSkillAtSlot(0, i, skill);
		}
		
		for (int i = 0; i < handSkills.Length; i++) {
			SkillEntity skill = handSkills[i];
			this.SetSkillAtSlot(320, i, skill);
		}
		
		for (int i = 0; i < handSkills.Length; i++) {
			SkillEntity skill = handSkills[i];
			this.SetSkillAtSlot(640, i, skill);
		}
	}

	public void SetSkillAtSlot(int x, int slotNo, SkillEntity skillEntity) {
		//float x = 0;
		float y = 70 - slotNo * 60;
		
		// Setup button
		GameObject button = Object.Instantiate(CommandButtonPrefab) as GameObject;
		button.transform.parent = this.transform;
		button.transform.localPosition = new Vector3(x, y, 0);
		button.transform.localScale = new Vector3(3, 3, 1);
		
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
		
		
		ObjectClickHandler clickHandler = button.GetComponent<ObjectClickHandler>();
		clickHandler.Callback = () => {this.click(slotNo);};
	}
	
	private void click(int slot) {
		CombatController controller = this.ZoneStageController.GetComponent<CombatController>();
		if (controller != null) {
			controller.InvokeCommand(slot);
		}
	}
}