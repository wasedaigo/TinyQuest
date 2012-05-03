using UnityEngine;
using System.Collections;
using TinyQuest.Data;
using TinyQuest.Data.Cache;

namespace TinyQuest.Entity {

	public class WeaponEntity {
		public const int SkillCount = 3;
		private SkillEntity[] skills = new SkillEntity[SkillCount];
		private MasterWeapon masterWeapon;
		private UserWeapon userWeapon;
		public WeaponEntity(MasterWeapon masterWeapon, int level) {
			this.masterWeapon = masterWeapon;
		}
		
		public WeaponEntity(MasterWeapon masterWeapon, UserWeapon userWeapon, int level) {
			this.masterWeapon = masterWeapon;
			this.userWeapon = userWeapon;
			this.UpdateLevel();
		}
		
		public void SetSkill(int index, SkillEntity skill) {
			this.skills[index] = skill;	
		}
		
		public void UpdateLevel() {
			int level = this.masterWeapon.GetLevel(this.userWeapon.exp);
			this.masterWeapon.GetParam(level);
		}
	}
}