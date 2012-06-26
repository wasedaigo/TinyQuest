using TinyQuest.Data;

class Constant {
	public static int GroupTypeCount = 2;
}

public class SkillAnimationParams {
	public UserUnit Caster;
	public UserUnit Target;
	public MasterSkill MasterSkill;
	
	public SkillAnimationParams(UserUnit caster, UserUnit target, MasterSkill masterSkill) {
		this.Caster = caster;
		this.Target = target;
		this.MasterSkill = masterSkill;
	}
}