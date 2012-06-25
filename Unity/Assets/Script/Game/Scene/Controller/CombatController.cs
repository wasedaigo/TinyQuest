using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using TinyQuest.Scene.Model;
using TinyQuest.Object;
using TinyQuest.Core;
using TinyQuest.Data;
using TinyQuest.Scene;

public class CombatController : MonoBehaviour {
	
	public UILabel AllyHP;
	public UILabel EnemyHP;
	
	private int targetId;
	private CombatModel combatModel;

	protected void CombatFinished() {
		this.SendMessage("OnCombatFinished");
	}

	public void SetModels(CombatModel combatModel) {
		this.combatModel = combatModel;
		this.combatModel.StartBattle += this.BattleStarted;
		this.combatModel.ExecuteAction += this.ActionExecuted;
	}
	
	public void StartBattle() {
		this.combatModel.Start();
	}
	
	public void BattleStarted() {
		List<CombatUnit>[] combatUnits = this.combatModel.GetCombatUnits();
		foreach (List<CombatUnit> combatUnitGroup in combatUnits) {
			foreach (CombatUnit combatUnit in combatUnitGroup) {
				this.SendMessage("SpawnActor", combatUnit.GetUserUnit());
			}
		}
		
		//this.monster = spawnBattler("fighter", Ally.State.Stand, -40, 0);
		this.SendMessage("UpdateStatus");
	}
	
	protected void AnimationFinished() {
		this.combatModel.ExecuteNextAction();
	}
	
	public void ActionExecuted(CombatAction action) {
		SendMessage(
			"PlaySkillAnimation", 
			new SkillAnimationParams(action.caster, action.target, action.skill)
		);	
	}

	public void InvokeSkill(int slotNo) {
		this.combatModel.ProgressTurn(slotNo);
	}
	
}
