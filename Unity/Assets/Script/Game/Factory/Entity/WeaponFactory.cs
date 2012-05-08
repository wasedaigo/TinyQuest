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
			for (int i = 0; i < masterWeapon.skills.Length; i++) {
				int skillId = masterWeapon.skills[i];
				SkillEntity skillEntity = SkillFactory.Instance.Build(skillId);
				weapon.SetSkill(i, skillEntity);
			}
			
			return weapon;
		}
		
	}
}