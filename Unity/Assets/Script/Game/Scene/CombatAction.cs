using System.Collections.Generic;
using TinyQuest.Data;

public class CombatActionResult {
	public CombatUnit combatUnit;
	public int[] activatedBuffs;
	public int[] expiredBuffs;
	public int effect;
	public int life;
	public int maxLife;
}

public class CombatAction {
	public readonly CombatUnit caster;
	public readonly CombatUnit target;
	public readonly MasterSkill skill;
	public CombatActionResult casterResult;
	public CombatActionResult targetResult;
	
	public CombatAction(CombatUnit caster, CombatUnit target, MasterSkill skill, CombatActionResult casterResult, CombatActionResult targetResult) {
		this.caster = caster;
		this.target = target;
		this.skill = skill;
		this.casterResult = casterResult;
		this.targetResult = targetResult;
	}
}