using UnityEngine;
using TinyQuest.Data.Cache;
using TinyQuest.Data;
using TinyQuest.Entity;

namespace TinyQuest.Factory.Entity {
	public class BattlerFactory  {
		public static readonly BattlerFactory Instance = new BattlerFactory();
		private BattlerFactory(){}
		
		public BattlerEntity BuildPuppet(int puppetId, BattlerEntity.GroupType groupType) {
			MasterPuppet masterPuppet = CacheFactory.Instance.GetMasterDataCache().GetPuppetByID(puppetId);
			CombatProgress combatProgress = CacheFactory.Instance.GetLocalUserDataCache().GetCombatProgress();
			BattlerEntity battler = null;
			if (combatProgress != null) {
				CombatBattler combatBattler = combatProgress.GetCombatBattler(0, (int)groupType);
				PuppetInstance instance = CacheFactory.Instance.GetLocalUserDataCache().GetPuppetInstanceByID(combatBattler.battlerID);
				PuppetEntity puppet = PuppetFactory.Instance.Build();
				battler = new BattlerEntity(puppet, combatBattler.hp, masterPuppet.hp, 0,  groupType);
			} else {
				PuppetEntity puppet = PuppetFactory.Instance.Build();
				battler = new BattlerEntity(puppet, masterPuppet.hp, masterPuppet.hp, 2, groupType);
			}
			return battler;
		}
	}
}