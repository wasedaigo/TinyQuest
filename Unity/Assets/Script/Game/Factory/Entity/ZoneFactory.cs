using UnityEngine;
using TinyQuest.Data;
using TinyQuest.Data.Cache;
using TinyQuest.Entity;

namespace TinyQuest.Factory.Entity {
	public class ZoneFactory {
		public static readonly ZoneFactory Instance = new ZoneFactory();
		private ZoneFactory(){}
		
		public ZoneEntity Build() {
			UserStatus userStatus = CacheFactory.Instance.GetLocalUserDataCache().GetUserStatus();
			UserZone userZone = CacheFactory.Instance.GetLocalUserDataCache().GetUserZone();
			ZoneEntity zoneEntity = new ZoneEntity(userZone);
			
			UserWeapon[] userWeapons = CacheFactory.Instance.GetLocalUserDataCache().GetEquipWeapons();
			BattlerEntity battler = new BattlerEntity(userStatus.maxHP, userStatus.maxHP, (int)BattlerEntity.NoType.Player, (int)BattlerEntity.GroupType.Player);
			
			WeaponEntity[] weaponEntities = new WeaponEntity[userWeapons.Length];
			for (int i = 0; i < userWeapons.Length; i++) {
				WeaponEntity weapon = WeaponFactory.Instance.Build(userWeapons[i]);
				weaponEntities[i] = weapon;
			}
			
			battler.SetWeapons(weaponEntities);
			
			zoneEntity.SetPlayerBattler(battler);
			
			
			return zoneEntity;
		}
	}
}