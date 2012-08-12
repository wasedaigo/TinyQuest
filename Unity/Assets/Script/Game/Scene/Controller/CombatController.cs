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
	
	public GameObject UIAllyCombatPanel;
	public GameObject UIEnemyCombatPanel;
	
	public GameObject ConnectingPop;
	public UICamera UICamera;
	
	private CombatControlPanelController allyCombatControlPanelController;
	private CombatControlPanelController enemyCombatControlPanelController;
	private Vector3 allyCombatControlPanelOrigin;
	private Vector3 enemyCombatControlPanelOrigin;
	
	private CombatModel combatModel;
	private bool firstUnitSelected = false;
	
	protected void CombatFinished() {
		this.SendMessage("OnCombatFinished");
	}
	private void Start () {
		Application.targetFrameRate = 60;
		this.combatModel = new CombatModel();

		// Get reference
		this.allyCombatControlPanelController = this.UIAllyCombatPanel.GetComponent<CombatControlPanelController>();
		this.enemyCombatControlPanelController = this.UIEnemyCombatPanel.GetComponent<CombatControlPanelController>();
		
		// Set Models
		this.allyCombatControlPanelController.SetModels(this.combatModel);
		this.enemyCombatControlPanelController.SetModels(this.combatModel);
		
		this.combatModel.ExecuteAction += this.ActionExecuted;
		this.combatModel.FinishBattle += this.BattleFinished;
		this.combatModel.SelectStandbyUnit += this.StandbyUnitSelected;
		this.combatModel.UpdateStatus += this.StatusUpdated;
		this.combatModel.StartTurn 	+= this.TurnStarted;

		// Delegate
		this.allyCombatControlPanelController.CardClicked += this.CardClicked;
		this.allyCombatControlPanelOrigin = this.UIAllyCombatPanel.transform.position;
		this.UIAllyCombatPanel.transform.position = new Vector3(this.allyCombatControlPanelOrigin.x, -10, this.allyCombatControlPanelOrigin.z);
		this.enemyCombatControlPanelOrigin = this.UIEnemyCombatPanel.transform.position;
		this.UIEnemyCombatPanel.transform.position = new Vector3(this.enemyCombatControlPanelOrigin.x, 10, this.enemyCombatControlPanelOrigin.z);
		
		this.StartBattle();
	}

	public void ChangeActorStatus(CombatActionResult result){
		CombatControlPanelController controlPanelController;
		if (result.combatUnit.groupType == CombatGroupInfo.Instance.GetPlayerGroupType(0)) {
			controlPanelController = this.allyCombatControlPanelController;
		} else {
			controlPanelController = this.enemyCombatControlPanelController;
		}
		controlPanelController.SendMessage("ChangeActorStatus", result);
		
		// Check if standby unit is killed
		if (result.combatUnit.IsDead) {
			CombatUnit nextUnit = this.combatModel.GetStandbyUnit();
		}
	}
	
	public void StartBattle() {		
		this.UIAllyCombatPanel.SetActiveRecursively(false);
		
		UICamera.enabled = false;
		this.ShowConnectingPop(true);
		this.combatModel.StartBattle(this);
    }
	
	private void StandbyUnitSelected() {
	}
	
	public void StatusUpdated() {
		this.UIAllyCombatPanel.SetActiveRecursively(true);
		
		this.allyCombatControlPanelController.UpdateStatus(CombatGroupInfo.Instance.GetPlayerGroupType(0));
		this.enemyCombatControlPanelController.UpdateStatus(CombatGroupInfo.Instance.GetPlayerGroupType(1));
	}
	
	public IEnumerator ShowActors(System.Action callback) {
		yield return new WaitForSeconds(0.1f);
	
		CombatUnitGroup[] combatUnitGroups = this.combatModel.GetCombatUnits();
		
		for (int i = 0; i < Constant.UnitCount; i++) {
			for (int j = 0; j < CombatGroupInfo.Instance.GetGroupCount(); j++) {
				CombatUnitGroup combatUnitGroup = combatUnitGroups[j];
				this.SendMessage("SpawnCombatActor", combatUnitGroup.combatUnits[i]);
				this.SendMessage("MoveActorFront", combatUnitGroup.combatUnits[i]);
			}
			
			yield return new WaitForSeconds(0.1f);
		}
		
		yield return new WaitForSeconds(0.5f);
		iTween.MoveTo(this.UIAllyCombatPanel, iTween.Hash("time", 0.5f, "y", this.allyCombatControlPanelOrigin.y,  "easeType", "easeOutCubic"));
		iTween.MoveTo(this.UIEnemyCombatPanel, iTween.Hash("time", 0.5f, "y", this.enemyCombatControlPanelOrigin.y,  "easeType", "easeOutCubic"));
		
		UICamera.enabled = true;
		
		for (int i = Constant.UnitCount - 1; i >= 0; i--) {
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
			this.allyCombatControlPanelController.ExecuteCard(this.combatModel.GetFightingUnit(0).index);
			this.enemyCombatControlPanelController.ExecuteCard(this.combatModel.GetFightingUnit(1).index);
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

	public void CardClicked(int slotNo) {
		this.combatModel.SetStandbyUnitByIndex(slotNo);
		
		//this.ShowConnectingPop(true);
		//UICamera.enabled = false;
		
		//this.combatModel.ProgressTurn(this, slotNo);
	}
}