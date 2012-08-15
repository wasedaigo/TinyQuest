using UnityEngine;
using System.Collections;

namespace TinyQuest.Data.Skills {
	public class MassiveAttack : BaseSkill {
		public MassiveAttack(int id) : base(id) {}
		public override int GetChance() {
			return 30;
		}
		public override SkillResult Calculate(CombatUnit combatUnit) {
			SkillResult result;
			result.damage = 4;
			result.animation = "Combat/Sword/MultiSlash";
			result.shout = this.GetName() + "!!";
			return result;
		}
	}
}