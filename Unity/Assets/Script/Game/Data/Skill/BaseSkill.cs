using UnityEngine;
using System.Collections;
using TinyQuest.Data.Cache;

namespace TinyQuest.Data.Skills {
	public abstract class BaseSkill {
		public struct SkillResult {
			public int damage;
			public string animation;
			public string shout;
		}
		
		public readonly int id;
		
		public BaseSkill(int id) {
			this.id = id;
		}
		
		public virtual string GetName() {
			return CacheFactory.Instance.GetLocalizedTextCache().Get("Skill", id.ToString(), "name") ;
		}
		
		public abstract int GetChance();
		public abstract SkillResult Calculate(CombatUnit combatUnit);
	}
}