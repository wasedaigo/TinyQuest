using UnityEngine;
using System.Collections;

namespace TinyQuest.Entity {
	public class BattlerEntity {
		public const int WeaponSlotNum = 6;
		private int maxHP;
		public int MaxHP {
				get {return this.maxHP;}
		}

		private int hp;
		public int HP {
				get {return this.hp;}
		}
		
		private WeaponEntity[] weapons = new WeaponEntity[WeaponSlotNum];

		public BattlerEntity(int maxHP) {
			this.maxHP = maxHP;
			this.hp = maxHP;
		}

		public WeaponEntity GetWeapon(int slotIndex) {
			return this.weapons[slotIndex];
		}
		
		public void SetWeapon(int slotIndex, WeaponEntity weapon) {
			this.weapons[slotIndex] = weapon;
		}
	}
}
