using UnityEngine;
using System.Collections;

namespace TinyQuest.Data.Skills {
	public class Fireball : BaseSkill {
		public Fireball(int id) : base(id) {}
		public override int GetChance() {
			return 100;
		}
		public override SkillResult Calculate(CombatUnit combatUnit) {
			SkillResult result;
			result.damage = combatUnit.GetUserUnit().Power * 2;
			result.animation = "Combat/Mage/Skill_Flare";
			result.shout = this.GetName();
			
			return result;
		}
	}
}