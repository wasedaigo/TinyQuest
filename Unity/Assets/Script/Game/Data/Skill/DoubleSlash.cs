using UnityEngine;
using System.Collections;

namespace TinyQuest.Data.Skills {
	public class DoubleSlash : BaseSkill {
		public DoubleSlash(int id) : base(id) {}
		public override int GetChance() {
			return 40;
		}
		
		public override SkillResult Calculate(CombatUnit combatUnit) {
			SkillResult result;
			result.damage = combatUnit.GetUserUnit().Power * 2;
			result.animation = "Combat/Sword/CrossSlash";
			result.shout = this.GetName() + "!!";
			return result;
		}
	}
}