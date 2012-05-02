using UnityEngine;
using System.Collections;
using TinyQuest.Data;

namespace TinyQuest.Entity {

	public class WeaponEntity {
		public const int SkillCount = 3;
		private SkillEntity[] skills = new SkillEntity[SkillCount];
		public WeaponEntity(MasterWeapon masterWeapon) {
		}
		
		public WeaponEntity(MasterWeapon masterWeapon, UserWeapon userWeapon) {
		}
		
		public void SetSkill(int index, SkillEntity skill) {
			this.skills[index] = skill;	
		}
	}
}
