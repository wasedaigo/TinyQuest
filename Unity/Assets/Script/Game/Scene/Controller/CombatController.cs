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
	
	private CombatControlPanelController combatControlPanelController;
	private CombatModel combatModel;
	
	protected void CombatFinished() {
		this.SendMessage("OnCombatFinished");
	}

	private void Start () {
		Application.targetFrameRate = 60;
		this.combatModel = new CombatModel();

		// Get reference
		this.combatControlPanelController = this.UICombatPanel.GetComponent<CombatControlPanelController>();
		this.combatControlPanelController.SetModels(this.combatModel);
		// Set Models
		this.combatControlPanelController.SetModels(this.combatModel);
		this.SetModels(this.combatModel);
		
		// Delegate
		this.combatControlPanelController.InvokeSkill += this.InvokeSkill;

		this.StartBattle();
	}
	
	public void ChangeActorStatus(CombatActionResult result){
		this.combatControlPanelController.SendMessage("ChangeActorStatus", result);
	}
	
	public void StartBattle() {
		LocalUserDataRequest req = RequestFactory.Instance.GetLocalUserRequest();
		req.StartBattle(this.OnLoaded);	
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

		combatControlPanelController.SendMessage("UpdateStatus");
	}
	
	public void SetModels(CombatModel combatModel) {
		this.combatModel = combatModel;
		this.combatModel.ExecuteAction += this.ActionExecuted;
		this.combatModel.SelectUnit += this.UnitSelected;
		this.combatModel.FinishBattle += this.BattleFinished;
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
		SendMessage("SelectCombatActors", new CombatUnit[]{caster, target});
	}

	public void InvokeSkill(int slotNo) {
		this.combatModel.ProgressTurn(slotNo);
	}
}