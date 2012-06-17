using UnityEngine;
using TinyQuest.Data.Cache;
using TinyQuest.Data;
using TinyQuest.Entity;

namespace TinyQuest.Factory.Entity {
	public class CombatFactory  {
		public static readonly CombatFactory Instance = new CombatFactory();
		private CombatFactory(){}

		public CombatEntity Build(int unitID, BattlerEntity playerBattlerEntity) {
			BattlerEntity enemyEntity = BattlerFactory.Instance.BuildUnit(unitID, BattlerEntity.GroupType.Player);
			CombatEntity combatEntity = new CombatEntity();
			combatEntity.SetBattler(playerBattlerEntity, BattlerEntity.GroupType.Player);
			combatEntity.SetBattler(enemyEntity, BattlerEntity.GroupType.Enemy);
			
			return combatEntity;
		}
	}
}