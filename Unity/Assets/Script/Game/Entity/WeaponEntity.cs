using UnityEngine;
using System.Collections;
using TinyQuest.Data;
using TinyQuest.Data.Cache;
using TinyQuest.Data.Request;

namespace TinyQuest.Entity {

	public class WeaponEntity {
		public static readonly int SkillCount = 3;
		
		private MasterSkill[] masterSkills = new MasterSkill[SkillCount];
		private MasterWeapon masterWeapon;
		private UserWeapon userWeapon;
		private MasterWeaponParameter parameter;
		private UserZone userZone;
		
		public WeaponEntity(MasterWeapon masterWeapon, int level) {
			this.masterWeapon = masterWeapon;
		}

		public WeaponEntity(MasterWeapon masterWeapon, UserWeapon userWeapon, int level) {
			this.masterWeapon = masterWeapon;
			this.userWeapon = userWeapon;
			
			this.userZone = CacheFactory.Instance.GetLocalUserDataCache().GetUserZone();
		}
		
		public MasterWeapon GetMasterWeapon() {
			return this.masterWeapon;
		}
		
		public MasterSkill[] GetSkills() {
			return this.masterSkills;
		}
		
		public void SetSkill(int index, MasterSkill masterSkill) {
			this.masterSkills[index] = masterSkill;	
		}
		
		public int GetLevel() {
			return this.masterWeapon.GetLevel(this.userWeapon.exp);
		}
		
		public int GetSkillCount() {
			if (this.masterWeapon.skill1 == 0) {
				return 0;
			}
			if (this.masterWeapon.skill2 == 0) {
				return 1;
			}
			if (this.masterWeapon.skill3 == 0) {
				return 2;
			}
			
			return 3;
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