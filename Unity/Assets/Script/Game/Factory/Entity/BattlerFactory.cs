using UnityEngine;
using TinyQuest.Data.Cache;
using TinyQuest.Data;
using TinyQuest.Entity;

namespace TinyQuest.Factory.Entity {
	public class BattlerFactory  {
		public static readonly BattlerFactory Instance = new BattlerFactory();
		private BattlerFactory(){}

		public BattlerEntity BuildEnemy(int enemyId) {
			MasterEnemy masterEnemy = CacheFactory.Instance.GetMasterDataCache().GetEnemyByID(enemyId);
			CombatProgress combatProgress = CacheFactory.Instance.GetLocalUserDataCache().GetCombatProgress();
			BattlerEntity battler = null;
			if (combatProgress != null) {
				CombatBattler combatBattler = combatProgress.GetCombatBattler((int)BattlerEntity.NoType.Enemy, (int)BattlerEntity.NoType.Enemy);
				battler = new BattlerEntity(combatBattler.hp, masterEnemy.hp, (int)BattlerEntity.NoType.Enemy, (int)BattlerEntity.GroupType.Enemy);
			} else {
				battler = new BattlerEntity(masterEnemy.hp, masterEnemy.hp, (int)BattlerEntity.NoType.Enemy, (int)BattlerEntity.GroupType.Enemy);
			}
			return battler;
		}
	}
}