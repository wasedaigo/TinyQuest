using UnityEngine;
using TinyQuest.Data;
using TinyQuest.Entity;
using TinyQuest.Factory.Data;

namespace TinyQuest.Factory.Entity {
	public class WeaponFactory {
		
		public static readonly WeaponFactory Instance = new WeaponFactory();
		private WeaponFactory(){}
	
		public WeaponEntity Build(int id) {
			MasterWeapons masterWeapons = MasterDataFactory<MasterWeapons>.Instance.Get();
			MasterWeapon masterWeapon = masterWeapons.data[id.ToString()];
			
			WeaponEntity weapon = new WeaponEntity(masterWeapon);
			return weapon;
		}
		
		public WeaponEntity Build(int weaponId, int userWeaponId) {
			MasterWeapons masterWeapons = MasterDataFactory<MasterWeapons>.Instance.Get();
			MasterWeapon masterWeapon = masterWeapons.data[weaponId.ToString()];

			UserWeapons userWeapons = UserDataFactory<UserWeapons>.Instance.Build();
			UserWeapon userWeapon = userWeapons.data[userWeaponId.ToString()];

			WeaponEntity weapon = new WeaponEntity(masterWeapon, userWeapon);
			return weapon;
		}
	}
}