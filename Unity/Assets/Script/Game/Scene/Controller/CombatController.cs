using UnityEngine;
using System.Collections;
using System.Collections.Generic;


using TinyQuest.Scene.Model;
using TinyQuest.Object;
using TinyQuest.Core;
using TinyQuest.Data;
using TinyQuest.Data.Request;
using TinyQuest.Scene;

public class CombatController : MonoBehaviour {
	
	public GameObject UICombatPanel;
	public GameObject ConnectingPop;
	public UICamera UICamera;
	
	private CombatControlPanelController combatControlPanelController;
	private CombatModel combatModel;
	private bool firstUnitSelected = false;
	private CombatUnitGroup[] combatUnitGroups;
	
	protected void CombatFinished() {
		this.SendMessage("OnCombatFinished");
	}
	private void Start () {
		Application.targetFrameRate = 60;
		this.combatModel = new CombatModel();

		// Get reference
		this.combatControlPanelController = this.UICombatPanel.GetComponent<CombatControlPanelController>();

		// Set Models
		this.combatControlPanelController.SetModels(this.combatModel);
		
		this.combatModel.ExecuteAction += this.ActionExecuted;
		this.combatModel.SelectUnit += this.UnitSelected;
		this.combatModel.FinishBattle += this.BattleFinished;
		this.combatModel.SelectStandByUnit += this.StandByUnitSelected;
		
		// Delegate
		this.combatControlPanelController.InvokeSkill += this.InvokeSkill;
		
		this.StartBattle();
		
	}

	public void ChangeActorStatus(CombatActionResult result){
		this.combatControlPanelController.SendMessage("ChangeActorStatus", result);
	}
	
	public void StartBattle() {		
		this.UICombatPanel.SetActiveRecursively(false);
		
		UICamera.enabled = false;
		this.ShowConnectingPop(true);
		LocalUserDataRequest req = RequestFactory.Instance.GetLocalUserRequest();
		req.StartBattle(this, this.OnLoaded);
    }
	
	private void OnLoaded(CombatUnitGroup[] combatUnitGroups) {
		this.ShowConnectingPop(false);
		this.combatUnitGroups = combatUnitGroups;
		this.StartCoroutine(this.ShowActors());
		this.combatModel.SetStandByUnitBySlot(0);
	}
	
	private void StandByUnitSelected(int unitId) {
		this.SendMessage("UpdateStandByUnit", unitId);
	}
	
	public IEnumerator ShowActors() {
		yield return new WaitForSeconds(0.1f);
	
		for (int i = 0; i < 6; i++) {
			for (int j = 0; j < CombatGroupInfo.Instance.GetGroupCount(); j++) {
				CombatUnitGroup combatUnitGroup = combatUnitGroups[j];
				this.SendMessage("SpawnCombatActor", combatUnitGroup.combatUnits[i]);
				this.SendMessage("MoveActorFront", combatUnitGroup.combatUnits[i]);
			}
			
			yield return new WaitForSeconds(0.1f);
		}
		
		yield return new WaitForSeconds(0.5f);
		iTween.MoveFrom(this.UICombatPanel, iTween.Hash("time", 0.5f, "y", -10,  "easeType", "easeOutCubic"));
		this.UICombatPanel.SetActiveRecursively(true);
		UICamera.enabled = true;
		combatControlPanelController.UpdateStatus();
	}
	
	public IEnumerator HideActors(System.Action callback) {
		for (int i = 5; i >= 0; i--) {
			for (int j = 0; j < CombatGroupInfo.Instance.GetGroupCount(); j++) {
				CombatUnitGroup combatUnitGroup = combatUnitGroups[j];
				this.SendMessage("MoveActorBack", combatUnitGroup.combatUnits[i]);
			}
			yield return new WaitForSeconds(0.05f);
		}
		yield return new WaitForSeconds(0.3f);
		callback();
	}
	
	private void ShowConnectingPop(bool value) {
		if (value) {
			this.ConnectingPop.SetActiveRecursively(true);
		} else {
			this.ConnectingPop.SetActiveRecursively(false);
		}
	}
	
	private void OnCombatActorSelected() {
		ExecuteNextAction();
	}
	
	private void ExecuteNextAction() {
		
		this.combatModel.ExecuteNextAction();
		
		if (!this.combatModel.IsExecutingAction()) {
			this.TurnFinished();
		}
	}
	
	public void TurnFinished() {
		this.combatModel.FinishTurn();
		UICamera.enabled = true;
	}
	
	public void BattleFinished() {
		this.SendMessage("ShowBattleWinPose");
		this.SendMessage("ShowZoneCutin", new ZoneCutinController.CutinParam("Battle Won!",
			() => {this.SendMessage("OnFinishBattle");}
		));
	}
	
	protected void OnFinishBattle() {
		Application.LoadLevel("Zone");	
	}
	
	public void ActionExecuted(CombatAction action) {
		SendMessage("CombatAction", action);	
	}
	
	public void UnitSelected(CombatUnit caster, CombatUnit target) {
		this.ShowConnectingPop(false);
		
		CombatUnit[] combatUnits = new CombatUnit[]{caster, target};
		System.Action callback = () => {
			SendMessage("SelectCombatActors", combatUnits);	
		};
		
		if (this.firstUnitSelected) {
			callback();
		} else {
			this.StartCoroutine(this.HideActors(callback));
			this.firstUnitSelected = true;
		}
	}

	public void InvokeSkill(int slotNo) {
		this.combatModel.SetStandByUnitBySlot(slotNo);
		
		//this.ShowConnectingPop(true);
		//UICamera.enabled = false;
		
		//this.combatModel.ProgressTurn(this, slotNo);
	}
}