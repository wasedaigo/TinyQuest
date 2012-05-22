using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TinyQuest.Data;
using TinyQuest.Factory.Entity;

namespace TinyQuest.Entity {
	public class BattlerEntity {
		public enum NoType {
			Player = 1,
			Enemy = 2
		};
		
		public enum GroupType {
			Player = 1,
			Enemy = 2
		};
		
		public const int MaxHands = 3;
		public const int WeaponSlotNum = 6;
		public const int MaxAP = 6;
		public const int HealAP = 3;
		
		public System.Action<WeaponEntity, SkillEntity> SkillUse;
		
		private int no;
		public int No {
			get {return no;}
		}
		
		private int group;
		public int Group {
			get {return group;}
		}
		
		private int maxHP;
		public int MaxHP {
			get {return this.maxHP;}
		}

		private int hp;
		public int HP {
			get {return this.hp;}
		}
		
		private List<SkillEntity> allSkills = new List<SkillEntity>();
		private SkillEntity[] handSkills = new SkillEntity[MaxHands];
		private WeaponEntity[] weapons = new WeaponEntity[WeaponSlotNum];

		public BattlerEntity(int hp, int maxHP, int no, int group) {
			this.maxHP = maxHP;
			this.hp = maxHP;
			this.no = no;
			this.group = group;
		}

		public WeaponEntity GetWeapon(int slotIndex) {
			return this.weapons[slotIndex];
		}
		
		public void SetWeapons(WeaponEntity[] weapons) {
			this.weapons = weapons;
			int index = 0;
			for (int i = 0; i < weapons.Length; i++) {
				WeaponEntity weapon = weapons[i];
				MasterSkill[] skills = weapon.GetSkills();
				for (int j = 0; j < skills.Length; j++) {
					MasterSkill skill = skills[j];
					if (skill != null) {
						this.allSkills.Add(SkillFactory.Instance.Build(skill.id, weapon, MaxHands/ weapon.GetSkillCount()));
					}
				}
			}
			
			this.DrawSkills();
		}

		public void UseSkill(int slotIndex) {
			this.SkillUsed(this.weapons[slotIndex], this.handSkills[slotIndex]);
		}
		
		public List<SkillEntity> GetAllSkills() {
			return this.allSkills;
		}
		
		public void DrawSkills() {
			for (int i = 0; i < MaxHands; i++) {
				if (this.weapons[i] != null) {
					this.handSkills[i] = this.allSkills[i];
				}
			}
		}
		
		public SkillEntity[] GetHand() {
			return this.handSkills;
		}
		
		private void SkillUsed(WeaponEntity weaponEntity, SkillEntity skillEntity) {
			if (this.SkillUse != null) {
				this.SkillUse(weaponEntity, skillEntity);
			}
		}
	}
}
