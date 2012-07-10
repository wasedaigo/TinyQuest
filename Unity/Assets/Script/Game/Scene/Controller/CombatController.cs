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

	void Awake() {
		CombatModel combatModel = new CombatModel();
		
		CombatControlPanelController combatControlPanelController = this.gameObject.GetComponent<CombatControlPanelController>();
		combatControlPanelController.SetModels(combatModel);

		this.SetModels(combatModel);
		Application.targetFrameRate = 60;
	}
	
	protected void CombatFinished() {
		this.SendMessage("OnCombatFinished");
	}

	public void SetModels(CombatModel combatModel) {
		this.combatModel = combatModel;
		this.combatModel.ExecuteAction += this.ActionExecuted;
		this.combatModel.SelectUnit += this.UnitSelected;
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