using System.Collections.Generic;
using TinyQuest.Data;
using TinyQuest.Data.Skills;

public class CombatActionResult {
	public CombatUnit combatUnit;
	public CombatUnit swapUnit;
	public int[] activatedBuffs;
	public int[] expiredBuffs;
	public int effect;
	public int life;
	public int maxLife;
}

public class CombatAction {
	public readonly CombatUnit caster;
	public readonly CombatUnit target;
	public readonly BaseSkill.SkillResult skillResult;
	public CombatActionResult casterResult;
	public CombatActionResult targetResult;
	
	public CombatAction(CombatUnit caster, CombatUnit target, BaseSkill.SkillResult skillResult, CombatActionResult casterResult, CombatActionResult targetResult) {
		this.caster = caster;
		this.target = target;
		this.skillResult = skillResult;
		this.casterResult = casterResult;
		this.targetResult = targetResult;
	}
}