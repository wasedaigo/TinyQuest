using UnityEngine;
using TinyQuest.Data;
using TinyQuest.Entity;
using TinyQuest.Data.Cache;

namespace TinyQuest.Factory.Entity {
	public class GearFactory {
		
		public static readonly GearFactory Instance = new GearFactory();
		private GearFactory(){}
	
		public GearEntity Build(int id, int lv) {
			MasterGear masterGear = CacheFactory.Instance.GetMasterDataCache().GetGearByID(id);
			
			GearEntity gear = new GearEntity(masterGear, lv);
			return gear;
		}
		
		public GearEntity Build(UserGear userGear) {
			GearEntity gear = new GearEntity(userGear.GetMasterGear(), userGear.GetLevel());
			return gear;
		}
	}
}