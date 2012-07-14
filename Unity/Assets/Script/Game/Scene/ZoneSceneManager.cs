using UnityEngine;
using System.Collections;
using TinyQuest.Data;
using TinyQuest.Data.Request;
using TinyQuest.Scene.Model;

public class ZoneSceneManager : MonoBehaviour {
	enum ZoneScene {
		Adventure,
		Combat
	};
	
	public GameObject UICombatPanel;
	public GameObject UIProgressPanel;
	private ZoneEventController zoneEventController;
	private CombatController combatController;
	private CombatControlPanelController combatControlPanelController;
	private CombatModel combatModel;

	// Use this for initialization
	private void Start () {
		ZoneModel zoneModel = new ZoneModel();
		this.combatModel = new CombatModel();

		// Get reference
		this.zoneEventController = this.GetComponent<ZoneEventController>();
		this.combatController = this.GetComponent<CombatController>();
		this.combatControlPanelController = this.UICombatPanel.GetComponent<CombatControlPanelController>();
		
		// Set Models
		this.zoneEventController.SetModels(zoneModel);
		this.combatControlPanelController.SetModels(this.combatModel);
		this.combatController.SetModels(this.combatModel);
		
		// Delegate
		this.combatControlPanelController.InvokeSkill += combatController.InvokeSkill;

		LocalUserDataRequest req = RequestFactory.Instance.GetLocalUserRequest();
		req.LoadZone(this.OnLoaded);
	}

	private void SetScene(ZoneScene zoneScene) {
		this.UIProgressPanel.SetActiveRecursively(false);
		this.UICombatPanel.SetActiveRecursively(false);
		this.zoneEventController.enabled = false;
		this.combatController.enabled = false;

		switch(zoneScene) {
			case ZoneScene.Adventure:
				this.zoneEventController.enabled = true;
				this.UIProgressPanel.SetActiveRecursively(true);
				this.zoneEventController.ResumeAdventure();
			break;
			case ZoneScene.Combat:
				this.combatController.enabled = true;

				this.UICombatPanel.SetActiveRecursively(true);
				combatControlPanelController.SendMessage("UpdateStatus");

				CombatUnitGroup[] combatUnitGroups = this.combatModel.GetCombatUnits();

				CombatUnit[] activeUnits = new CombatUnit[Constant.GroupTypeCount];
				for (int i = 0; i < Constant.GroupTypeCount; i++) {
					CombatUnitGroup combatUnitGroup = combatUnitGroups[i];
					foreach (CombatUnit combatUnit in combatUnitGroup.combatUnits) {
					   this.SendMessage("SpawnCombatActor", combatUnit);
					}
					if (combatUnitGroup.combatUnits.Count > 0) {
						activeUnits[i] = combatUnitGroup.combatUnits[combatUnitGroup.activeIndex];
					};
				}

				this.SendMessage("SelectCombatActors", activeUnits);
			break;
		}
	}

	private void OnLoaded(CombatUnitGroup[] combatUnitGroups) {
		CombatUnit[] activeUnits = new CombatUnit[Constant.GroupTypeCount];
		for (int i = 0; i < Constant.GroupTypeCount; i++) {
			CombatUnitGroup combatUnitGroup = combatUnitGroups[i];
			foreach (CombatUnit combatUnit in combatUnitGroup.combatUnits) {
			   this.SendMessage("SpawnCombatActor", combatUnit);
			}
			if (combatUnitGroup.combatUnits.Count > 0) {
				activeUnits[i] = combatUnitGroup.combatUnits[combatUnitGroup.activeIndex];
			}
		}
		
		this.SendMessage("ShowCombatActors", activeUnits);
		this.SetScene(ZoneScene.Adventure);
	}

	private void ChangeActorStatus(CombatActionResult result) {
		CombatControlPanelController combatControlPanelController = this.UICombatPanel.GetComponent<CombatControlPanelController>();
		combatControlPanelController.SendMessage("ChangeActorStatus", result);
	}
	
	private void StartBattle(int enemyGroupId) {
		this.SetScene(ZoneScene.Combat);
	}

	private void OnFinishBattle() {
		this.SetScene(ZoneScene.Adventure);
	}
}
