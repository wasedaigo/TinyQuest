using UnityEngine;
using TinyQuest.Data;
using TinyQuest.Entity;
using TinyQuest.Data.Cache;

namespace TinyQuest.Factory.Entity {
	public class WeaponFactory {
		
		public static readonly WeaponFactory Instance = new WeaponFactory();
		private WeaponFactory(){}
	
		public WeaponEntity Build(int id) {
			MasterWeapon masterWeapon = MasterDataCache<MasterWeapon>.Instance.Get(id);
			
			WeaponEntity weapon = new WeaponEntity(masterWeapon, 1);
			return weapon;
		}
		
		public WeaponEntity Build(int weaponId, int userWeaponId) {
			MasterWeapon masterWeapon = MasterDataCache<MasterWeapon>.Instance.Get(weaponId);
			UserWeapon userWeapon = LocalUserDataCache<UserWeapon>.Instance.Get(userWeaponId);
			WeaponEntity weapon = new WeaponEntity(masterWeapon, userWeapon, 1);
			return weapon;
		}
		
	}
}