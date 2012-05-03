using UnityEngine;
using TinyQuest.Data;
using TinyQuest.Entity;
using TinyQuest.Factory.Data;

namespace TinyQuest.Factory.Entity {
	public class WeaponFactory {
		
		public static readonly WeaponFactory Instance = new WeaponFactory();
		private WeaponFactory(){}
	
		public WeaponEntity Build(int id) {
			MasterWeapon masterWeapon = MasterDataFactory<MasterWeapon>.Instance.Get(id);
			
			WeaponEntity weapon = new WeaponEntity(masterWeapon, 1);
			return weapon;
		}
		
		public WeaponEntity Build(int weaponId, int userWeaponId) {
			MasterWeapon masterWeapon = MasterDataFactory<MasterWeapon>.Instance.Get(weaponId);

			UserWeapons userWeapons = UserDataFactory<UserWeapons>.Instance.Build();
			UserWeapon userWeapon = userWeapons.data[userWeaponId];

			WeaponEntity weapon = new WeaponEntity(masterWeapon, userWeapon, 1);
			return weapon;
		}
	}
}