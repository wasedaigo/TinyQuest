using UnityEngine;
using System.Collections;
using TinyQuest.Data;
using TinyQuest.Data.Request;
using TinyQuest.Scene.Model;

public class ZoneSceneManager : MonoBehaviour {
	public enum ZoneSceneType {
		Adventure,
		Combat
	};
	
	public enum ZonePanelType {
		None,
		Combat,
		Progress,
		Next
	};
	
	public GameObject UICombatPanel;
	public GameObject UIProgressPanel;
	public GameObject UINextPanel;

	private ZoneEventController zoneEventController;
	private CombatController combatController;
	private CombatControlPanelController combatControlPanelController;
	private ZoneModel zoneModel;
	private CombatModel combatModel;
	private ZoneSceneType currentZoneSceneType;
	private ZonePanelType currentZonePanelType;
	
	// Use this for initialization
	private void Start () {
		Application.targetFrameRate = 60;
		this.zoneModel = new ZoneModel();
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

	private void SetScene(ZoneSceneType zoneScene) {

		this.zoneEventController.enabled = false;
		this.combatController.enabled = false;
		
		this.currentZoneSceneType = zoneScene;
		switch(zoneScene) {
			case ZoneSceneType.Adventure:
				this.zoneEventController.enabled = true;
				this.zoneEventController.ResumeAdventure();
			break;
			case ZoneSceneType.Combat:
				this.combatController.enabled = true;

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

	private void ShowPanel(ZonePanelType type) {
		if (currentZonePanelType == type) {return;}
		
		this.currentZonePanelType = type;
		this.UINextPanel.SetActiveRecursively(false);
		this.UIProgressPanel.SetActiveRecursively(false);
		this.UICombatPanel.SetActiveRecursively(false);
		
		switch (type) {
		case ZonePanelType.Combat:
			this.UICombatPanel.SetActiveRecursively(true);
			break;
		case ZonePanelType.Next:
			this.UINextPanel.SetActiveRecursively(true);
			break;
		case ZonePanelType.Progress:
			this.UIProgressPanel.SetActiveRecursively(true);
			break;
		case ZonePanelType.None:
			break;
		}
	}
	
	private ZonePanelType GetPanelTypeBySceneType(ZoneSceneType type) {
		ZonePanelType panelType = ZonePanelType.None;
		switch (type) {
			case ZoneSceneType.Adventure:
				panelType = ZonePanelType.Progress;
			break;
			case ZoneSceneType.Combat:
				panelType = ZonePanelType.Combat;
			break;
		}
		
		return panelType;
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
		this.SetScene(ZoneSceneType.Adventure);
		//this.ShowPanel(this.GetPanelTypeBySceneType(this.currentZoneSceneType));
	}

	private void ChangeActorStatus(CombatActionResult result) {
		CombatControlPanelController combatControlPanelController = this.UICombatPanel.GetComponent<CombatControlPanelController>();
		combatControlPanelController.SendMessage("ChangeActorStatus", result);
	}
	
	private void StartBattle(int enemyGroupId) {
		this.SetScene(ZoneSceneType.Combat);
	}

	private void OnFinishBattle() {
		this.SetScene(ZoneSceneType.Adventure);
	}

	private void OnCutSceneFinished() {
		if (this.currentZoneSceneType == ZoneSceneType.Combat) {
			this.ShowPanel(ZonePanelType.Combat);
			combatControlPanelController.SendMessage("UpdateStatus");
		} else {
			if (this.zoneModel.IsCommandExecuting()) {
				this.SendMessage("NextCommand");
			} else {
				this.ShowPanel(ZonePanelType.Progress);
			}
			
		}
	}
}
