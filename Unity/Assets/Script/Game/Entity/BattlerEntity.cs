using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TinyQuest.Data;
using TinyQuest.Factory.Entity;

namespace TinyQuest.Entity {
	public class BattlerEntity {
		
		public enum GroupType {
			Player = 0,
			Enemy = 1,
			Count
		};
		
		public const int MaxHands = 3;
		public const int WeaponSlotNum = 6;
		public const int MaxAP = 6;
		public const int HealAP = 3;
		
		public System.Action<SkillEntity[]> SkillDraw;
		public System.Action<WeaponEntity, SkillEntity> SkillUse;
		
		private int no;
		public int No {
			get {return no;}
		}
		
		private GroupType group;
		public GroupType Group {
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
		
		private List<SkillEntity> librarySkills = new List<SkillEntity>();
		private SkillEntity[] handSkills = new SkillEntity[MaxHands];
		private WeaponEntity[] weapons = new WeaponEntity[WeaponSlotNum];

		public BattlerEntity(int hp, int maxHP, int no, GroupType group) {
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
			for (int i = 0; i < weapons.Length; i++) {
				WeaponEntity weapon = weapons[i];
				MasterSkill[] skills = weapon.GetSkills();
				for (int j = 0; j < skills.Length; j++) {
					MasterSkill skill = skills[j];
					if (skill != null) {
						this.librarySkills.Add(SkillFactory.Instance.Build(skill.id, weapon, MaxHands/ weapon.GetSkillCount()));
					}
				}
			}
		}

		public void UseSkill(int slotIndex) {
			if (this.handSkills[slotIndex] == null) {
				Debug.LogError("No skill exists for slotIndex = " + slotIndex);
			}
			this.SkillUsed(this.weapons[slotIndex], this.handSkills[slotIndex]);
			this.librarySkills.Add(this.handSkills[slotIndex]);
			this.handSkills[slotIndex] = null;
		}

		public void DrawSkills() {
			SkillEntity[] drawnSkills = new SkillEntity[MaxHands];
			for (int i = 0; i < MaxHands; i++) {
				if (this.handSkills[i] == null) {
					int index = Random.Range(0, this.librarySkills.Count - 1);
					this.handSkills[i] = this.librarySkills[index];
					this.librarySkills.RemoveAt(index);
					drawnSkills[i] = this.handSkills[i];
				}
			}

			if (this.SkillDraw != null) {
				this.SkillDraw(drawnSkills);
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
