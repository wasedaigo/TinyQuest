using UnityEngine;
using TinyQuest.Data.Cache;
using TinyQuest.Data;
using TinyQuest.Entity;

namespace TinyQuest.Factory.Entity {
	public class BattlerFactory  {
		public static readonly BattlerFactory Instance = new BattlerFactory();
		private BattlerFactory(){}

		public BattlerEntity BuildEnemy(int enemyId) {
			MasterMonster masterEnemy = CacheFactory.Instance.GetMasterDataCache().GetMonsterByID(enemyId);
			CombatProgress combatProgress = CacheFactory.Instance.GetLocalUserDataCache().GetCombatProgress();
			BattlerEntity battler = null;
			if (combatProgress != null) {
				CombatBattler combatBattler = combatProgress.GetCombatBattler(0, (int)BattlerEntity.GroupType.Enemy);
				battler = new BattlerEntity(combatBattler.hp, masterEnemy.hp, 0, BattlerEntity.GroupType.Enemy);
			} else {
				battler = new BattlerEntity(masterEnemy.hp, masterEnemy.hp, 0, BattlerEntity.GroupType.Enemy);
			}
			return battler;
		}
		
		public BattlerEntity BuildPlayer() {
			UserStatus userStatus = CacheFactory.Instance.GetLocalUserDataCache().GetUserStatus();
			
			UserWeapon[] userWeapons = CacheFactory.Instance.GetLocalUserDataCache().GetEquipWeapons();
			BattlerEntity battler = new BattlerEntity(userStatus.maxHP, userStatus.maxHP, 0, BattlerEntity.GroupType.Player);

			WeaponEntity[] weaponEntities = new WeaponEntity[userWeapons.Length];
			for (int i = 0; i < userWeapons.Length; i++) {
				WeaponEntity weapon = WeaponFactory.Instance.Build(userWeapons[i]);
				weaponEntities[i] = weapon;
			}
			
			battler.SetWeapons(weaponEntities);	
			return battler;
		}
	}
}