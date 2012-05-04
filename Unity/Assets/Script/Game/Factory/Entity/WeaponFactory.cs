using UnityEngine;
using TinyQuest.Data;
using TinyQuest.Entity;
using TinyQuest.Data.Cache;

namespace TinyQuest.Factory.Entity {
	public class WeaponFactory {
		
		public static readonly WeaponFactory Instance = new WeaponFactory();
		private WeaponFactory(){}
	
		public WeaponEntity Build(int id) {
			MasterWeapon masterWeapon = CacheFactory.Instance.GetMasterDataCache().GetWeaponByID(id);
			
			WeaponEntity weapon = new WeaponEntity(masterWeapon, 1);
			return weapon;
		}
		
		public WeaponEntity Build(int weaponId, int userWeaponId) {
			MasterWeapon masterWeapon = CacheFactory.Instance.GetMasterDataCache().GetWeaponByID(weaponId);
			UserWeapon userWeapon = CacheFactory.Instance.GetLocalUserDataCache().GetWeaponByID(userWeaponId);
			WeaponEntity weapon = new WeaponEntity(masterWeapon, userWeapon, 1);
			return weapon;
		}
		
	}
}