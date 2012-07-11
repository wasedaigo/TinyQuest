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
	
	void Awake() {
		
	}
	
	// Use this for initialization
	void Start () {
		LocalUserDataRequest req = RequestFactory.Instance.GetLocalUserRequest();
		req.LoadZone(this.OnLoaded);
	}

	// Update is called once per frame
	void Update () {
	
	}
	
	void SetScene(ZoneScene zoneScene) {
		this.zoneScene = zoneScene;
		switch(zoneScene) {
			case ZoneScene.Adventure:
				ZoneEventController zoneEventController = this.gameObject.AddComponent<ZoneEventController>();
				this.UIProgressPanel.SetActiveRecursively(true);
			
				ZoneModel zoneModel = new ZoneModel();
				zoneEventController.SetModels(zoneModel);
				zoneEventController.StartAdventure();
			break;
			case ZoneScene.Combat:
				CombatController combatController = this.gameObject.AddComponent<CombatController>();

				this.UICombatPanel.SetActiveRecursively(true);
				CombatControlPanelController combatControlPanelController = this.UICombatPanel.GetComponent<CombatControlPanelController>();

				CombatModel combatModel = new CombatModel();
				combatControlPanelController.SetModels(combatModel);
				combatController.SetModels(combatModel);
				
				combatControlPanelController.InvokeSkill += combatController.InvokeSkill;
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
}
