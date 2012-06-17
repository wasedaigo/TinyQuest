using UnityEngine;
using TinyQuest.Data;
using TinyQuest.Data.Cache;
using TinyQuest.Model;

namespace TinyQuest.Factory.Model {
	public class ZoneFactory {
		public static readonly ZoneFactory Instance = new ZoneFactory();
		private ZoneFactory(){}
		
		public ZoneModel Build() {
			BattlerModel battlerModel = BattlerFactory.Instance.BuildUnit(1, BattlerModel.GroupType.Player); // TODO
			UserZone userZone = CacheFactory.Instance.GetLocalUserDataCache().GetUserZone();
			ZoneModel zoneModel = new ZoneModel(userZone);
			zoneModel.SetPlayerBattler(battlerModel);
			
			return zoneModel;
		}
	}
}