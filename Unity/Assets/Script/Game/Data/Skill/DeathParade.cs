using UnityEngine;
using System.Collections;

namespace TinyQuest.Data.Skills {
	public class DeathParade : BaseSkill {
		public DeathParade(int id) : base(id) {}
		public override int GetChance() {
			return 30;
		}
		public override SkillResult Calculate(CombatUnit combatUnit) {
			SkillResult result;
			result.damage = 3;
			result.animation = "Combat/Sword/MultiSlash";
			result.shout = this.GetName() + "!!";
			return result;
		}
	}
}