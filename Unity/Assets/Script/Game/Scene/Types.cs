using TinyQuest.Data;

class Constant {
	public const int GroupTypeCount = 2;
}

public class CombatAction {
	public readonly UserUnit caster;
	public readonly UserUnit target;
	public readonly MasterSkill skill;
	public int effect;
	
	public CombatAction(UserUnit caster, UserUnit target, MasterSkill skill, int effect) {
		this.caster = caster;
		this.target = target;
		this.skill = skill;
		this.effect = effect;
	}
}
