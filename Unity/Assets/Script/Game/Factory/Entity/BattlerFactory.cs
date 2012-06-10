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
				MonsterInstance instance = CacheFactory.Instance.GetLocalUserDataCache().GetMonsterInstanceByID(combatBattler.battlerID);
				CoreEntity core = CoreFactory.Instance.Build(instance.GetMasterMonster().activeGears, instance.GetMasterMonster().passiveGears);
				battler = new BattlerEntity(core, combatBattler.hp, masterEnemy.hp, 0,  BattlerEntity.GroupType.Enemy);
			} else {
				CoreEntity core = CoreFactory.Instance.Build(masterEnemy.activeGears, masterEnemy.passiveGears);
				battler = new BattlerEntity(core, masterEnemy.hp, masterEnemy.hp, 2, BattlerEntity.GroupType.Enemy);
			}
			return battler;
		}
		
		public BattlerEntity BuildPuppet(int puppetId) {
			UserStatus userStatus = CacheFactory.Instance.GetLocalUserDataCache().GetUserStatus();
			CombatProgress combatProgress = CacheFactory.Instance.GetLocalUserDataCache().GetCombatProgress();
			CombatBattler combatBattler = combatProgress.GetCombatBattler(0, (int)BattlerEntity.GroupType.Player);
			
			UserCore userCore = CacheFactory.Instance.GetLocalUserDataCache().GetPuppetByID(puppetId).GetUserCore();
			CoreEntity core = CoreFactory.Instance.Build(userCore.GetActiveUserGears(), userCore.GetPassiveUserGears());
			BattlerEntity battler = new BattlerEntity(core, userStatus.maxHP, userStatus.maxHP, 2, BattlerEntity.GroupType.Player);

			return battler;
		}
	}
}