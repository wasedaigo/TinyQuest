using UnityEngine;
using TinyQuest.Data.Cache;
using TinyQuest.Data;
using TinyQuest.Entity;

namespace TinyQuest.Factory.Entity {
	public class CombatFactory  {
		public static readonly CombatFactory Instance = new CombatFactory();
		private CombatFactory(){}

		public CombatEntity Build(int puppetID, BattlerEntity playerBattlerEntity) {
			BattlerEntity enemyEntity = BattlerFactory.Instance.BuildPuppet(puppetID, BattlerEntity.GroupType.Player);
			CombatEntity combatEntity = new CombatEntity();
			combatEntity.SetBattler(playerBattlerEntity, BattlerEntity.GroupType.Player);
			combatEntity.SetBattler(enemyEntity, BattlerEntity.GroupType.Enemy);
			
			return combatEntity;
		}
	}
}