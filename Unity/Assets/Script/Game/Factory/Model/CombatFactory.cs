using UnityEngine;
using TinyQuest.Data.Cache;
using TinyQuest.Data;
using TinyQuest.Model;

namespace TinyQuest.Factory.Model {
	public class CombatFactory  {
		public static readonly CombatFactory Instance = new CombatFactory();
		private CombatFactory(){}

		public CombatModel Build(int unitID, BattlerModel playerBattlerModel) {
			BattlerModel enemyModel = BattlerFactory.Instance.BuildUnit(unitID, BattlerModel.GroupType.Player);
			CombatModel combatModel = new CombatModel();
			combatModel.SetBattler(playerBattlerModel, BattlerModel.GroupType.Player);
			combatModel.SetBattler(enemyModel, BattlerModel.GroupType.Enemy);
			
			return combatModel;
		}
	}
}