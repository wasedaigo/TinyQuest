using UnityEngine;
using TinyQuest.Entity;

namespace TinyQuest.Factory.Entity {
	public class BattlerFactory {
		public static readonly BattlerFactory Instance = new BattlerFactory();
		private BattlerFactory(){}

		public BattlerEntity Build(int no) {
			BattlerEntity battler = new BattlerEntity(100);
			WeaponEntity weapon = WeaponFactory.Instance.Build(0);
			battler.SetWeapon(0, weapon);
			return battler;
		}
	}
}