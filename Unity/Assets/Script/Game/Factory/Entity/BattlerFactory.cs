using UnityEngine;
using TinyQuest.Data.Cache;
using TinyQuest.Data;
using TinyQuest.Entity;

namespace TinyQuest.Factory.Entity {
	public class BattlerFactory  {
		public static readonly BattlerFactory Instance = new BattlerFactory();
		private BattlerFactory(){}

		public BattlerEntity BuildUserBattler() {
			UserWeapon[] userWeapons = CacheFactory.Instance.GetLocalUserDataCache().GetEquipWeapons();
			BattlerEntity battler = new BattlerEntity(100);
			
			for (int i = 0; i < userWeapons.Length; i++) {
				WeaponEntity weapon = WeaponFactory.Instance.Build(userWeapons[i]);
				battler.SetWeapon(userWeapons[i].slot, weapon);
			}
			return battler;
		}
	}
}