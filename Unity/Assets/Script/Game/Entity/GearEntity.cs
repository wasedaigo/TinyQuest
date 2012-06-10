using UnityEngine;
using System.Collections;
using TinyQuest.Data;
using TinyQuest.Data.Cache;
using TinyQuest.Data.Request;

namespace TinyQuest.Entity {

	public class GearEntity {
		public static readonly int SkillCount = 3;
		
		private MasterSkill[] masterSkills = new MasterSkill[SkillCount];
		private MasterGear masterGear;
		private UserGear userGear;
		private MasterGearParameter parameter;
		
		public GearEntity(MasterGear masterGear, int level) {
			this.masterGear = masterGear;
		}

		public GearEntity(MasterGear masterGear, UserGear userGear) {
			this.masterGear = masterGear;
			this.userGear = userGear;
		}
		
		public MasterGear GetMasterGear() {
			return this.masterGear;
		}
		
		public UserGear GetUserGear() {
			return this.userGear;
		}
		
		public MasterSkill[] GetSkills() {
			return this.masterSkills;
		}
		
		public void SetSkill(int index, MasterSkill masterSkill) {
			this.masterSkills[index] = masterSkill;	
		}
		
		public int GetLevel() {
			return this.masterGear.GetLevel(this.userGear.exp);
		}

		public void AddExp(int exp) {
			int level = this.masterGear.GetLevel(this.userGear.exp);
			
			int maxExp = this.masterGear.GetMaxExp();
			if (this.userGear.exp < maxExp) {
				// Exp Gain Event
				this.userGear.exp += exp;
				if (this.userGear.exp > maxExp) {
					this.userGear.exp = maxExp;
				}

				int updatedLevel = this.masterGear.GetLevel(this.userGear.exp);
				if (updatedLevel > level) {
					// Level Up Event	
				}
			}
		}
	}
}