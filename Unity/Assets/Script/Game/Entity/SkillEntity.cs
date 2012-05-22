using UnityEngine;
using System.Collections;
using TinyQuest.Data;

namespace TinyQuest.Entity {
	public class SkillEntity {
		private MasterSkill masterSkill;
		private WeaponEntity ownerWeapon;
	
		public WeaponEntity OwnerWeapon {
			get { return this.ownerWeapon; }
		}
		
		public MasterSkill MasterSkill {
			get { return this.masterSkill; }
		}
		
		private float weight;
		public float Weight {
			get { return this.weight; }
		}
		
		public SkillEntity(WeaponEntity ownerWeapon, MasterSkill masterSkill, float weight) {
			this.ownerWeapon = ownerWeapon;
			this.masterSkill = masterSkill;
			this.weight = weight;
		}
	}
}
