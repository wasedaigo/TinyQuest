using UnityEngine;
using System.Collections;
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
		public const int WeaponSlotNum = 3;
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
		
		private SkillEntity[] allSkills = new SkillEntity[WeaponEntity.SkillCount * WeaponSlotNum];
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
				MasterSkill[] skills = weapons[i].GetSkills();
				for (int j = 0; j < skills.Length; j++) {
					MasterSkill skill = skills[i];
					if (skill != null) {
						this.allSkills[index] = SkillFactory.Instance.Build(skill.id);
					}
					index++;
				}
			}
			
			this.DrawSkills();
		}

		public void UseSkill(int slotIndex) {
			this.SkillUsed(this.weapons[slotIndex], this.handSkills[slotIndex]);
		}
		
		public SkillEntity[] GetAllSkills() {
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
