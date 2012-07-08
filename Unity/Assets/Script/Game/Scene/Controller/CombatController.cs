using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using TinyQuest.Scene.Model;
using TinyQuest.Object;
using TinyQuest.Core;
using TinyQuest.Data;
using TinyQuest.Scene;

public class CombatController : MonoBehaviour {
	
	private int targetId;
	private CombatModel combatModel;
	
	void Start () {
		CombatModel combatModel = new CombatModel();
		
		CombatControlPanelController controlPanelController = this.gameObject.GetComponent<CombatControlPanelController>();
		controlPanelController.SetModels(combatModel);

		this.SetModels(combatModel);
		this.StartBattle();
		Application.targetFrameRate = 60;
	}
	
	protected void CombatFinished() {
		this.SendMessage("OnCombatFinished");
	}

	public void SetModels(CombatModel combatModel) {
		this.combatModel = combatModel;
		this.combatModel.StartBattle += this.BattleStarted;
		this.combatModel.ExecuteAction += this.ActionExecuted;
		this.combatModel.SelectUnit += this.UnitSelected;
	}

	public void StartBattle() {
		this.combatModel.Start();
	}

	public void BattleStarted() {
		List<CombatUnit>[] combatUnits = this.combatModel.GetCombatUnits();
		foreach (List<CombatUnit> combatUnitGroup in combatUnits) {
			foreach (CombatUnit combatUnit in combatUnitGroup) {
				this.SendMessage("SpawnActor", combatUnit);
			}
		}
		
		//this.monster = spawnBattler("fighter", Ally.State.Stand, -40, 0);
		this.SendMessage("UpdateStatus");
	}
	
	protected void ExecuteNextAction() {
		this.combatModel.ExecuteNextAction();
		
		if (!this.combatModel.IsExecutingAction()) {
			this.TurnFinished();
		}
	}
	
	public void TurnFinished() {
		this.combatModel.FinishTurn();
	}
	
	public void ActionExecuted(CombatAction action) {
		SendMessage("CombatAction", action);	
	}
	
	public void UnitSelected(CombatUnit caster, CombatUnit target) {
		SendMessage("SelectActor", new CombatUnit[]{caster, target});
	}

	public void InvokeSkill(int slotNo) {
		this.combatModel.ProgressTurn(slotNo);
	}
}