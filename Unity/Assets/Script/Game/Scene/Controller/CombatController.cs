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
	public FighterStatusPanelController FightingStatusPanelAlly;
	public FighterStatusPanelController FightingStatusPanelEnemy;
	
	public GameObject ConnectingPop;
	public UICamera UICamera;
	
	private CombatControlPanelController allyCombatControlPanelController;
	private CombatControlPanelController enemyCombatControlPanelController;
	private ZoneViewController zoneViewController;
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
		this.zoneViewController = this.GetComponent<ZoneViewController>();
		
		// Set Models
		this.allyCombatControlPanelController.SetModels(this.combatModel);
		this.enemyCombatControlPanelController.SetModels(this.combatModel);
		this.zoneViewController.SetModel(this.combatModel);
		
		this.combatModel.ExecuteAction += this.ActionExecuted;
		this.combatModel.BattleStarted += this.BattleStarted;
		this.combatModel.FinishBattle += this.BattleFinished;		
		this.combatModel.UpdateStatus += this.StatusUpdated;

		// Delegate
		this.allyCombatControlPanelController.CardSelected += this.CardSelected;
		this.allyCombatControlPanelController.CardExecuted += this.CardExecuted;
		this.allyCombatControlPanelOrigin = this.UIAllyCombatPanel.transform.position;
		this.UIAllyCombatPanel.transform.position = new Vector3(this.allyCombatControlPanelOrigin.x, -10, this.allyCombatControlPanelOrigin.z);
		this.enemyCombatControlPanelOrigin = this.UIEnemyCombatPanel.transform.position;
		this.UIEnemyCombatPanel.transform.position = new Vector3(this.enemyCombatControlPanelOrigin.x, 10, this.enemyCombatControlPanelOrigin.z);
		
		this.StartBattle();
	}

	public void ChangeActorStatus(CombatActionResult result){
		CombatControlPanelController controlPanelController;
		if (result.combatUnit.groupType == 0) {
			controlPanelController = this.allyCombatControlPanelController;
			this.FightingStatusPanelAlly.SendMessage("ShowFighterStatus", result.combatUnit);
		} else {
			controlPanelController = this.enemyCombatControlPanelController;
			this.FightingStatusPanelEnemy.SendMessage("ShowFighterStatus", result.combatUnit);
		}
		controlPanelController.SendMessage("ChangeActorStatus", result);
	}
	
	public void StartBattle() {		
		this.UIAllyCombatPanel.SetActiveRecursively(false);
		this.allyCombatControlPanelController.SetTouchEnabled(false);
		
		UICamera.enabled = false;
		this.ShowConnectingPop(true);
		this.combatModel.StartBattle(this);

		this.ShowConnectingPop(false);
		
		this.firstUnitSelected = true;
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
		
		UICamera.enabled = true;
		
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
		this.StartCoroutine(this.ShowActors(this.ShowFirstActors));

	}
	
	public void ShowFirstActors() {
		if (this.combatModel.IsPlayerTurn()) {
			CombatUnit unit = this.combatModel.GetFightingUnit(1);
			this.zoneViewController.SelectCombatActor(
				unit,
				() => {
					this.FightingStatusPanelEnemy.SendMessage("ShowFighterStatus", unit);
					this.SendMessage("ShowZoneCutin", new ZoneCutinController.CutinParam("Choose the defender!", null));
					this.StartInput();
				}
			);
		} else {
			this.SendMessage("ShowZoneCutin", new ZoneCutinController.CutinParam("Choose the attacker!", null));
			this.StartInput();
		}
	}
	
	public void StartInput() {
		UICamera.enabled = true;
		this.allyCombatControlPanelController.SetTouchEnabled(true);
		this.SendMessage("StartTimer");
		
		CombatUnit combatUnit = this.combatModel.GetPlayerFightingUnit();
		if (combatUnit != null && !combatUnit.IsDead) {
			this.ShowPanel(combatUnit.index);
		}
	}

	public void InputTimerFinished() {
		CombatUnit combatUnit = this.combatModel.GetFirstAliveUnit(0);
		if (combatUnit != null) {
			this.allyCombatControlPanelController.SelectCard(combatUnit.index);
			this.CardExecuted(combatUnit.index);
		}
	}
	
	public void CombatReady() {
		this.StartCoroutine(this.CombatStarted());
	}
	
	public IEnumerator CombatStarted() {
		UICamera.enabled = false;
		this.zoneViewController.SetPose(1, Actor.PoseType.Attack);
		yield return new WaitForSeconds(0.2f);
		this.combatModel.ProcessActions();
		
		this.ExecuteNextAction();
	}
	
	public void TurnFinished() {
		this.zoneViewController.ResetPose(0);
		this.zoneViewController.ResetPose(1);
		if (!this.combatModel.FinishTurn()) {
			if (this.combatModel.IsPlayerTurn()) {
				this.StartInput();
			} else {
				this.ReceiveTurnInput();
			}
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
		this.ShowPanel(index);
	}
	
	private void ShowPanel(int index) {
		CombatUnit combatUnit = this.combatModel.GetCombatUnit(0, index);
		
		int[] skillIds = combatUnit.GetUserUnit().Unit.skills;
		BaseSkill[] skills = new BaseSkill[Constant.SkillSlotCount];
		for (int i = 0; i < skillIds.Length; i++) {
			skills[i] = SkillFactory.Instance.GetSkill(skillIds[i]);
		}
		
		CombatCardInformation.ShowPanelParams param = new CombatCardInformation.ShowPanelParams(skills[0], skills[1], 0);
		this.SendMessage("ShowCardInfoPanel", param);
	}

	public void CardExecuted(int index) {
		this.SendMessage("CancelTimer");
		this.SendMessage("HideCardInfoPanel");

		this.allyCombatControlPanelController.SetTouchEnabled(false);
		this.combatModel.SetPlayerFightingUnitIndex(index);
		
		this.SendTurnInput();
	}
	
	public void SendTurnInput() {
		this.FightingStatusPanelAlly.SendMessage("HideFighterStatus");
		List<System.Action<System.Action>> list = new List<System.Action<System.Action>>();
		list.Add( 
			(next) => { this.combatModel.SendTurnInput(this, next); } 
		);
		list.Add( 
			(next) => {
				CombatUnit unit = this.combatModel.GetFightingUnit(0);
				this.zoneViewController.SelectCombatActor(unit, () =>{
					this.FightingStatusPanelAlly.SendMessage("ShowFighterStatus", unit);
					this.zoneViewController.SetPose(0, Actor.PoseType.Attack);
					next();
				});
			} 
		);
		
		Async.Async.Instance.Parallel(list, () => {
			this.CombatReady();
		});		
	}
	
	public void ReceiveTurnInput() {
		List<System.Action<System.Action>> list = new List<System.Action<System.Action>>();
		list.Add( 
			(next) => { this.combatModel.ReceiveTurnInput(this, next); } 
		);
		list.Add( 
			(next) => {
				this.FightingStatusPanelEnemy.SendMessage("HideFighterStatus");
				CombatUnit unit = this.combatModel.GetFightingUnit(1);
				this.zoneViewController.SelectCombatActor(unit, () =>{
					this.FightingStatusPanelEnemy.SendMessage("ShowFighterStatus", unit);
					this.zoneViewController.SetPose(1, Actor.PoseType.Attack);
					next();
				});
			} 
		);
		
		Async.Async.Instance.Waterfall(list, () => {
			this.CombatReady();
		});		
	}
}