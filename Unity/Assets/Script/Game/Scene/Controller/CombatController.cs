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
		this.combatModel.SkillUse += this.SkillUsed;
	}
	
	public void StartBattle() {
		this.combatModel.Start();
	}
	
	public void BattleStarted() {
		List<CombatUnit> combatUnits = this.combatModel.GetCombatUnits();
		foreach (CombatUnit combatUnit in combatUnits) {
			this.SendMessage("SpawnActor", combatUnit.GetUserUnit());
		}
		
		//this.monster = spawnBattler("fighter", Ally.State.Stand, -40, 0);
		this.SendMessage("UpdateStatus");
	}
	
	protected void AnimationFinished() {
		this.combatModel.ProgressTurn();
	}
	
	public void SkillUsed(MasterSkill masterSkill, CombatUnit casterUnit, CombatUnit targetUnit) {
		SendMessage(
			"PlaySkillAnimation", 
			new SkillAnimationParams(casterUnit.GetUserUnit(), targetUnit.GetUserUnit(), masterSkill)
		);
	}

	public void InvokeSkill(int slotNo) {
		this.combatModel.UseSkill(slotNo);
	}
	
}
