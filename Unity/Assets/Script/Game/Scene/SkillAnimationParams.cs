using TinyQuest.Data;

namespace TinyQuest.Scene {
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
}