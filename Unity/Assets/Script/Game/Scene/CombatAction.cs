using TinyQuest.Data;

public struct CombatAction {
	public readonly UserUnit caster;
	public readonly UserUnit target;
	public readonly MasterSkill skill;	
	
	public CombatAction(UserUnit caster, UserUnit target, MasterSkill skill) {
		this.caster = caster;
		this.target = target;
		this.skill = skill;
	}
}