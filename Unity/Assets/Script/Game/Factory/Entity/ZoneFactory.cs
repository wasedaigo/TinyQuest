using UnityEngine;
using TinyQuest.Data;
using TinyQuest.Data.Cache;
using TinyQuest.Entity;

namespace TinyQuest.Factory.Entity {
	public class ZoneFactory {
		public static readonly ZoneFactory Instance = new ZoneFactory();
		private ZoneFactory(){}
		
		public ZoneEntity Build(int id) {
			UserZone userZone = CacheFactory.Instance.GetLocalUserDataCache().GetUserZone();
			
			ZoneEntity zoneEntity = new ZoneEntity(userZone);
			return zoneEntity;
		}
	}
}