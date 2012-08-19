using UnityEngine;
using System.Collections;

namespace TinyQuest.Data.Skills {
	public class PoisonousBlow : BaseSkill {
		public PoisonousBlow(int id) : base(id) {}
		public override int GetChance() {
			return 30;
		}
		public override SkillResult Calculate(CombatUnit combatUnit) {
			SkillResult result;
			result.damage = combatUnit.GetUserUnit().Power * 2;
			result.animation = "Combat/Sword/Slash";
			result.shout = this.GetName() + "!!";
			return result;
		}
	}
}