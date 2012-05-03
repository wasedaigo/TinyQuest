using UnityEngine;
using TinyQuest.Data;
using TinyQuest.Data.Cache;
using TinyQuest.Entity;

namespace TinyQuest.Factory.Entity {
	public class ZoneFactory {
		public static readonly ZoneFactory Instance = new ZoneFactory();
		private ZoneFactory(){}
		
		public ZoneEntity Build(int id) {
			ZoneEntity zoneEntity = new ZoneEntity();
			return zoneEntity;
		}
	}
}