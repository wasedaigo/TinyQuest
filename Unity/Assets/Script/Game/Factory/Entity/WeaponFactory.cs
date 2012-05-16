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
		
		public WeaponEntity Build(UserWeapon userWeapon) {
			MasterWeapon masterWeapon = CacheFactory.Instance.GetMasterDataCache().GetWeaponByID(userWeapon.weaponId);
			WeaponEntity weapon = new WeaponEntity(masterWeapon, userWeapon, 1);


			weapon.SetSkill(0, CacheFactory.Instance.GetMasterDataCache().GetSkillByID(masterWeapon.skill1));
			weapon.SetSkill(1, CacheFactory.Instance.GetMasterDataCache().GetSkillByID(masterWeapon.skill2));
			weapon.SetSkill(2, CacheFactory.Instance.GetMasterDataCache().GetSkillByID(masterWeapon.skill3));
			
			return weapon;
		}
		
	}
}