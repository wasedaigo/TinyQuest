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
		this.combatModel.FinishBattle += this.BattleFinished;
		this.combatModel.SelectStandbyUnit += this.StandbyUnitSelected;
		this.combatModel.UpdateStatus += this.StatusUpdated;
		this.combatModel.StartTurn 	+= this.TurnStarted;
		// Delegate
		this.combatControlPanelController.InvokeSkill += this.InvokeSkill;
		Vector3 pos = this.UICombatPanel.transform.position;
		this.UICombatPanel.transform.position = new Vector3(pos.x, -10, pos.y);
		
		this.StartBattle();
		
	}

	public void ChangeActorStatus(CombatActionResult result){
		this.combatControlPanelController.SendMessage("ChangeActorStatus", result);
		
		// Check if standby unit is killed
		if (result.combatUnit.IsDead) {
			CombatUnit nextUnit = this.combatModel.GetStandbyUnit();
			this.SendMessage("UpdateStandbyUnit", nextUnit);
		}
	}
	
	public void StartBattle() {		
		this.UICombatPanel.SetActiveRecursively(false);
		
		UICamera.enabled = false;
		this.ShowConnectingPop(true);
		this.combatModel.StartBattle(this);
    }
	
	private void StandbyUnitSelected() {
		CombatUnit combatUnit = this.combatModel.GetStandbyUnit();
		this.SendMessage("UpdateStandbyUnit", combatUnit);
	}
	
	public void StatusUpdated() {
		this.UICombatPanel.SetActiveRecursively(true);
		combatControlPanelController.UpdateStatus();	
	}
	
	public IEnumerator ShowActors(System.Action callback) {
		yield return new WaitForSeconds(0.1f);
	
		CombatUnitGroup[] combatUnitGroups = this.combatModel.GetCombatUnits();
		
		for (int i = 0; i < 6; i++) {
			for (int j = 0; j < CombatGroupInfo.Instance.GetGroupCount(); j++) {
				CombatUnitGroup combatUnitGroup = combatUnitGroups[j];
				this.SendMessage("SpawnCombatActor", combatUnitGroup.combatUnits[i]);
				this.SendMessage("MoveActorFront", combatUnitGroup.combatUnits[i]);
			}
			
			yield return new WaitForSeconds(0.1f);
		}
		
		yield return new WaitForSeconds(0.5f);
		iTween.MoveTo(this.UICombatPanel, iTween.Hash("time", 0.5f, "y", -0.57f,  "easeType", "easeOutCubic"));
		UICamera.enabled = true;
		
		for (int i = 5; i >= 0; i--) {
			for (int j = 0; j < CombatGroupInfo.Instance.GetGroupCount(); j++) {
				CombatUnitGroup combatUnitGroup = combatUnitGroups[j];
				this.SendMessage("MoveActorBack", combatUnitGroup.combatUnits[i]);
			}
			yield return new WaitForSeconds(0.05f);
		}
		
		yield return new WaitForSeconds(0.5f);
		callback();
	}
	
	private void ShowConnectingPop(bool value) {
		if (value) {
			this.ConnectingPop.SetActiveRecursively(true);
		} else {
			this.ConnectingPop.SetActiveRecursively(false);
		}
	}
	
	private void ExecuteNextAction() {
		
		this.combatModel.ExecuteNextAction();
		
		if (!this.combatModel.IsExecutingAction()) {
			this.TurnFinished();
		}
	}

	public void TurnStarted() {
		System.Action callback = () => {
			this.SendMessage("StartTimer");
			SendMessage("SelectCombatActor", this.combatModel.GetFightingUnit(0));	
			SendMessage("SelectCombatActor", this.combatModel.GetFightingUnit(1));
		};

		if (this.firstUnitSelected) {
			callback();
		} else {
			this.ShowConnectingPop(false);
			this.StartCoroutine(this.ShowActors(callback));
			this.firstUnitSelected = true;
		}
	}		
	
	public void InputTimerFinished() {
		UICamera.enabled = false;
		ExecuteNextAction();
		
		// Start asking the server for next turn info
		this.combatModel.LoadNextTurn(this);
	}

	public void TurnFinished() {
		this.combatModel.FinishTurn();
		UICamera.enabled = true;
		
		this.combatModel.NextTurn(this);
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

	public void InvokeSkill(int slotNo) {
		this.combatModel.SetStandbyUnitByIndex(slotNo);
		
		//this.ShowConnectingPop(true);
		//UICamera.enabled = false;
		
		//this.combatModel.ProgressTurn(this, slotNo);
	}
}