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
	private ZoneScene zoneScene;
	private ZoneEventController zoneEventController;
	private CombatController combatController;
	private CombatControlPanelController combatControlPanelController;
	
	void Awake() {
		
	}
	
	// Use this for initialization
	void Start () {
		ZoneModel zoneModel = new ZoneModel();
		CombatModel combatModel = new CombatModel();
		
		// Get reference
		this.zoneEventController = this.GetComponent<ZoneEventController>();
		this.combatController = this.GetComponent<CombatController>();
		this.combatControlPanelController = this.UICombatPanel.GetComponent<CombatControlPanelController>();
		
		// Set Models
		this.zoneEventController.SetModels(zoneModel);
		this.combatControlPanelController.SetModels(combatModel);
		this.combatController.SetModels(combatModel);
		
		// Delegate
		this.combatControlPanelController.InvokeSkill += combatController.InvokeSkill;

		LocalUserDataRequest req = RequestFactory.Instance.GetLocalUserRequest();
		req.LoadZone(this.OnLoaded);
	}

	// Update is called once per frame
	void Update () {
	
	}
	
	void SetScene(ZoneScene zoneScene) {
		this.UIProgressPanel.SetActiveRecursively(false);
		this.UICombatPanel.SetActiveRecursively(false);
		this.zoneEventController.enabled = false;
		this.combatController.enabled = false;

		this.zoneScene = zoneScene;
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
			break;
		}
	}

	void OnLoaded(CombatUnitGroup[] combatUnitGroups) {
		CombatUnit[] activeUnits = new CombatUnit[Constant.GroupTypeCount];
		for (int i = 0; i < Constant.GroupTypeCount; i++) {
			CombatUnitGroup combatUnitGroup = combatUnitGroups[i];
			foreach (CombatUnit combatUnit in combatUnitGroup.combatUnits) {
			   this.SendMessage("SpawnActor", combatUnit);
			}
			activeUnits[i] = combatUnitGroup.combatUnits[combatUnitGroup.activeIndex];
		}
		
		this.SendMessage("ShowActors", activeUnits);
		this.SetScene(ZoneScene.Adventure);
	}
	
	void ChangeActorStatus(CombatActionResult result) {
		CombatControlPanelController combatControlPanelController = this.UICombatPanel.GetComponent<CombatControlPanelController>();
		combatControlPanelController.SendMessage("ChangeActorStatus", result);
	}
	
	void StartBattle() {
		this.SetScene(ZoneScene.Combat);
	}

	void FinishBattle() {
		this.SetScene(ZoneScene.Adventure);
	}
}
