using UnityEngine;
using System.Collections;
using TinyQuest.Data;
using TinyQuest.Data.Cache;

namespace TinyQuest.Entity {

	public class WeaponEntity {
		public static readonly int SkillCount = 3;
		private SkillEntity[] skills = new SkillEntity[SkillCount];
		private MasterWeapon masterWeapon;
		private UserWeapon userWeapon;
		private MasterWeaponParameter parameter;
		public WeaponEntity(MasterWeapon masterWeapon, int level) {
			this.masterWeapon = masterWeapon;
		}
		
		public WeaponEntity(MasterWeapon masterWeapon, UserWeapon userWeapon, int level) {
			this.masterWeapon = masterWeapon;
			this.userWeapon = userWeapon;
			this.UpdateLevel();
		}
	
		
		public SkillEntity[] GetSkills() {
			return this.skills;
		}
		
		public void SetSkill(int index, SkillEntity skill) {
			this.skills[index] = skill;	
		}
		
		public void GetLevel() {
			int level = this.masterWeapon.GetLevel(this.userWeapon.exp);
			this.parameter = this.masterWeapon.GetParam(level);
		}
		
		public void UpdateLevel() {
			int level = this.masterWeapon.GetLevel(this.userWeapon.exp);
			this.parameter = this.masterWeapon.GetParam(level);
		}

		public void AddExp(int exp) {
			int level = this.masterWeapon.GetLevel(this.userWeapon.exp);
			
			int maxExp = this.masterWeapon.GetMaxExp();
			if (this.userWeapon.exp < maxExp) {
				// Exp Gain Event
				this.userWeapon.exp += exp;
				if (this.userWeapon.exp > maxExp) {
					this.userWeapon.exp = maxExp;
				}

				int updatedLevel = this.masterWeapon.GetLevel(this.userWeapon.exp);
				if (updatedLevel > level) {
					// Level Up Event	
				}
			}
			
		}
	}
}