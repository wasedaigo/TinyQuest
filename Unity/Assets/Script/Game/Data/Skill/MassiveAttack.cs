using UnityEngine;
using System.Collections;

namespace TinyQuest.Data.Skills {
	public class MassiveAttack : BaseSkill {
		public MassiveAttack(int id) : base(id) {}
		public override int GetChance() {
			return 100;
		}
		public override SkillResult Calculate(CombatUnit combatUnit) {
			SkillResult result;
			result.damage = combatUnit.GetUserUnit().Power * 3;
			result.animation = "Combat/Sword/MeteorSmash";
			result.shout = this.GetName() + "!!";
			return result;
		}
	}
}