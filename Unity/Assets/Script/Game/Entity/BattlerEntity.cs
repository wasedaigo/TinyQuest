using UnityEngine;
using System.Collections;

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
		
		public const int WeaponSlotNum = 6;
		public const int MaxAP = 6;
		public const int HealAP = 3;
		
		public System.Action<WeaponEntity, SkillEntity> WeaponUse;
		public System.Action<int> UpdateAP;
		
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
		
		public void SetWeapon(int slotIndex, WeaponEntity weapon) {
			if (this.weapons[slotIndex] != null) {
				Debug.LogError("Weapon is already set at slot "+ slotIndex);
			}
			this.weapons[slotIndex] = weapon;
			this.weapons[slotIndex].WeaponUse += this.WeaponUsed;
		}
		
		public void UseWeapon(int slotIndex) {
			this.weapons[slotIndex].Use();
		}
		
		private void WeaponUsed(WeaponEntity weaponEntity, SkillEntity skillEntity, int newAP) {
			if (this.WeaponUse != null) {
				this.WeaponUse(weaponEntity, skillEntity);
			}
			
			if (this.UpdateAP != null) {
				this.UpdateAP(newAP);
			}
			
		}
	}
}
