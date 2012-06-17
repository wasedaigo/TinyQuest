using UnityEngine;
using TinyQuest.Data.Cache;
using TinyQuest.Data;
using TinyQuest.Entity;

namespace TinyQuest.Factory.Entity {
	public class BattlerFactory  {
		public static readonly BattlerFactory Instance = new BattlerFactory();
		private BattlerFactory(){}
		
		public BattlerEntity BuildUnit(int unitId, BattlerEntity.GroupType groupType) {
			MasterUnit masterUnit = CacheFactory.Instance.GetMasterDataCache().GetUnitByID(unitId);
			CombatProgress combatProgress = CacheFactory.Instance.GetLocalUserDataCache().GetCombatProgress();
			BattlerEntity battler = null;
			if (combatProgress != null) {
				CombatBattler combatBattler = combatProgress.GetCombatBattler(0, (int)groupType);
				//UnitInstance instance = CacheFactory.Instance.GetLocalUserDataCache().GetUnitInstanceByID(combatBattler.battlerID);
				battler = new BattlerEntity(masterUnit, combatBattler.hp, 0,  groupType);
			} else {
				battler = new BattlerEntity(masterUnit, masterUnit.hpTable.GetValue(1), 2, groupType);
			}
			return battler;
		}
	}
}