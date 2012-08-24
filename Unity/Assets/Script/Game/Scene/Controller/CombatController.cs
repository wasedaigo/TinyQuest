using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using TinyQuest.Scene.Model;
using TinyQuest.Object;
using TinyQuest.Core;
using TinyQuest.Data;
using TinyQuest.Data.Skills;
using TinyQuest.Data.Request;
using TinyQuest.Scene;

public class CombatController : MonoBehaviour {
	
	public GameObject UIAllyCombatPanel;
	public GameObject UIEnemyCombatPanel;
	public GameObject ConnectingPop;
	public GameObject StopButton;
	
	private CombatControlPanelController allyCombatControlPanelController;
	private CombatControlPanelController enemyCombatControlPanelController;
	private ZoneViewController zoneViewController;
	private Vector3 allyCombatControlPanelOrigin;
	private Vector3 enemyCombatControlPanelOrigin;
	private bool firstTurnFinished;
	
	private CombatModel combatModel;
	
	protected void CombatFinished() {
		this.SendMessage("OnCombatFinished");
	}
	private void Start () {
		Application.targetFrameRate = 60;
		this.combatModel = new CombatModel();
		
		this.SetControlVisible(false);

		// Get reference
		this.allyCombatControlPanelController = this.UIAllyCombatPanel.GetComponent<CombatControlPanelController>();
		this.enemyCombatControlPanelController = this.UIEnemyCombatPanel.GetComponent<CombatControlPanelController>();
		this.zoneViewController = this.GetComponent<ZoneViewController>();
		
		// Set Models
		this.allyCombatControlPanelController.SetModels(this.combatModel);
		this.enemyCombatControlPanelController.SetModels(this.combatModel);
		this.zoneViewController.SetModel(this.combatModel);
		
		this.combatModel.ExecuteAction += this.ActionExecuted;
		this.combatModel.BattleStarted += this.BattleStarted;
		this.combatModel.FinishBattle += this.BattleFinished;		
		this.combatModel.UpdateStatus += this.StatusUpdated;
		this.combatModel.CombatReady += this.CombatReady;

		// Delegate
		this.allyCombatControlPanelController.CardSelected += this.CardSelected;
		this.allyCombatControlPanelOrigin = this.UIAllyCombatPanel.transform.position;
		this.UIAllyCombatPanel.transform.position = new Vector3(this.allyCombatControlPanelOrigin.x, -10, this.allyCombatControlPanelOrigin.z);
		this.enemyCombatControlPanelOrigin = this.UIEnemyCombatPanel.transform.position;
		this.UIEnemyCombatPanel.transform.position = new Vector3(this.enemyCombatControlPanelOrigin.x, 10, this.enemyCombatControlPanelOrigin.z);
		
		this.StartBattle();
	}

	public void ChangeActorStatus(CombatActionResult result){
		CombatControlPanelController controlPanelController;
		if (result.combatUnit.groupNo == 0) {
			controlPanelController = this.allyCombatControlPanelController;
			this.allyCombatControlPanelController.SelectCard(this.combatModel.GetStandByUnitIndex(0));
		} else {
			controlPanelController = this.enemyCombatControlPanelController;
		}
		controlPanelController.SendMessage("ChangeActorStatus", result);
		
		
	}
	
	public void StartBattle() {		
		this.UIAllyCombatPanel.SetActiveRecursively(false);
		this.allyCombatControlPanelController.SetTouchEnabled(false);
		
		this.ShowConnectingPop(true);
		this.combatModel.StartBattle(this);

		this.ShowConnectingPop(false);
    }
	
	public void StatusUpdated() {
		this.UIAllyCombatPanel.SetActiveRecursively(true);
		
		this.allyCombatControlPanelController.UpdateStatus(0);
		this.enemyCombatControlPanelController.UpdateStatus(1);
	}
	
	public IEnumerator ShowActors(System.Action callback) {
		yield return new WaitForSeconds(0.1f);
	
		CombatUnitGroup[] combatUnitGroups = this.combatModel.GetCombatUnits();
		
		for (int i = 0; i < Constant.UnitCount; i++) {
			for (int j = 0; j < Constant.GroupCount; j++) {
				CombatUnitGroup combatUnitGroup = combatUnitGroups[j];
				this.SendMessage("SpawnCombatActor", combatUnitGroup.combatUnits[i]);
				this.SendMessage("MoveActorFront", combatUnitGroup.combatUnits[i]);
			}
			
			yield return new WaitForSeconds(0.1f);
		}
		
		yield return new WaitForSeconds(0.5f);
		iTween.MoveTo(this.UIAllyCombatPanel, iTween.Hash("time", 0.5f, "y", this.allyCombatControlPanelOrigin.y,  "easeType", "easeOutCubic"));
		iTween.MoveTo(this.UIEnemyCombatPanel, iTween.Hash("time", 0.5f, "y", this.enemyCombatControlPanelOrigin.y,  "easeType", "easeOutCubic"));
		
		for (int i = Constant.UnitCount - 1; i >= 0; i--) {
			for (int j = 0; j < Constant.GroupCount; j++) {
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
	
	public void BattleStarted() {
		this.StatusUpdated();
		this.StartCoroutine(this.ShowActors(this.StartSetUpPhase));
	}

	public void StartSetUpPhase() {
		this.StatusUpdated();
		CombatUnit unit1 = this.combatModel.GetFightingUnit(0);
		CombatUnit unit2 = this.combatModel.GetFightingUnit(1);

		List<System.Action<System.Action>> list = new List<System.Action<System.Action>>() {
			(next) => { this.zoneViewController.SelectCombatActor(unit1, next); },
			(next) => { this.zoneViewController.SelectCombatActor(unit2, next); } 
		};
		
		this.allyCombatControlPanelController.SelectCard(this.combatModel.GetStandByUnitIndex(0));
		Async.Async.Instance.Parallel(list, () => {
			
			if (this.firstTurnFinished) {
				this.StartInput();
			} else {
				this.allyCombatControlPanelController.ChooseAttackingCard(unit1.index);
				this.SendMessage("ShowZoneCutin", new ZoneCutinController.CutinParam("Ready...", this.StartInput));
				this.firstTurnFinished = true;
			}
		});
	}

	public void StartInput() {
		this.SetControlVisible(true);
		
		this.allyCombatControlPanelController.SetTouchEnabled(true);
		this.SendMessage("StartTimer");
	}

	public void InputTimerFinished() {
		this.SendTurnInput();
	}

	public void CombatReady(CombatUnit playerUnit, CombatUnit opponentUnit) {
		List<System.Action<System.Action>> list = new List<System.Action<System.Action>>() {
			(next) => { this.zoneViewController.SelectCombatActor(playerUnit, next); },
			(next) => { this.zoneViewController.SelectCombatActor(opponentUnit, next); } 
		};

		Async.Async.Instance.Parallel(list, () => {
			this.ExecuteNextAction();
		});
	}
	
	public void TurnFinished() {
		this.zoneViewController.ResetPose(0);
		this.zoneViewController.ResetPose(1);
		if (!this.combatModel.FinishTurn()) {
			this.StartSetUpPhase();
		}
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

	public void CardSelected(int index) {
		this.SetControlVisible(false);
		this.SendMessage("CancelTimer");
		this.combatModel.ForceSwap();
		this.SendTurnInput();
	}

	public void SendTurnInput() {
		this.SetControlVisible(false);
			
		this.allyCombatControlPanelController.SetTouchEnabled(false);
		this.combatModel.SendTurnInput(this, this.allyCombatControlPanelController.GetSelectingCardIndex());
	}
	
	public void CombatUnitMoveOut(CombatUnit unit) {
		if (unit.groupNo == 0) {
			this.allyCombatControlPanelController.BackAttackingCard(unit.index);
		} else {
			this.enemyCombatControlPanelController.BackAttackingCard(unit.index);	
		}
	}
	
	public void CombatUnitMoveIn(CombatUnit unit) {
		if (unit.groupNo == 0) {
			this.allyCombatControlPanelController.ChooseAttackingCard(unit.index);
		} else {
			this.enemyCombatControlPanelController.ChooseAttackingCard(unit.index);	
		}
	}
	public void StopLine() {
		this.SendTurnInput();
	}
	
	private void SetControlVisible(bool visible) {
		StopButton.SetActiveRecursively(visible);
	}
}