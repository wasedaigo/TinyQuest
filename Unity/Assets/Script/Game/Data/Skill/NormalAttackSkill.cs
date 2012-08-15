using UnityEngine;
using System.Collections;

namespace TinyQuest.Data.Skills {
	public class NormalAttackSkill : BaseSkill {
		private string animation;
		public NormalAttackSkill(string animation) : base(0) {
			this.animation = animation;
		}
		public override int GetChance() {
			return 100;
		}
		public override SkillResult Calculate(CombatUnit combatUnit) {
			SkillResult result;
			result.damage = combatUnit.GetUserUnit().Power;
			result.animation = this.animation;
			result.shout = "";
			return result;
		}
	}
}