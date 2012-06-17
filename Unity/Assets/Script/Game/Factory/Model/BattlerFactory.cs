using UnityEngine;
using TinyQuest.Data.Cache;
using TinyQuest.Data;
using TinyQuest.Model;

namespace TinyQuest.Factory.Model {
	public class BattlerFactory  {
		public static readonly BattlerFactory Instance = new BattlerFactory();
		private BattlerFactory(){}
		
		public BattlerModel BuildUnit(int unitId, BattlerModel.GroupType groupType) {
			MasterUnit masterUnit = CacheFactory.Instance.GetMasterDataCache().GetUnitByID(unitId);
			CombatProgress combatProgress = CacheFactory.Instance.GetLocalUserDataCache().GetCombatProgress();
			BattlerModel battler = null;
			if (combatProgress != null) {
				CombatBattler combatBattler = combatProgress.GetCombatBattler(0, (int)groupType);
				//UnitInstance instance = CacheFactory.Instance.GetLocalUserDataCache().GetUnitInstanceByID(combatBattler.battlerID);
				battler = new BattlerModel(masterUnit, combatBattler.hp, 0,  groupType);
			} else {
				battler = new BattlerModel(masterUnit, masterUnit.hpTable.GetValue(1), 2, groupType);
			}
			return battler;
		}
	}
}