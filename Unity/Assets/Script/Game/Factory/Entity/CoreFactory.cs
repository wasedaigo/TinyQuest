using UnityEngine;
using TinyQuest.Data;
using TinyQuest.Entity;
using TinyQuest.Data.Cache;

namespace TinyQuest.Factory.Entity {
	public class CoreFactory {
		
		public static readonly CoreFactory Instance = new CoreFactory();
		private CoreFactory(){}
	
		public CoreEntity Build(UserGear[] activeUserGears, UserGear[] passiveUserGears) {
				
				GearEntity[] activeGears = new GearEntity[activeUserGears.Length];
				for (int i = 0; i < activeUserGears.Length; i++) {
					activeGears[i] = GearFactory.Instance.Build(activeUserGears[i].gear, activeUserGears[i].GetLevel());
				}
				
				GearEntity[] passiveGears = new GearEntity[passiveUserGears.Length];
				for (int i = 0; i < passiveUserGears.Length; i++) {
					passiveGears[i] = GearFactory.Instance.Build(passiveUserGears[i].gear, passiveUserGears[i].GetLevel());
				}
				
				return this.Build(activeGears, passiveGears);
		}	
		
		public CoreEntity Build(MasterGearSetting[] activeGearSettings, MasterGearSetting[] passiveGearSettings) {
				GearEntity[] activeGears = new GearEntity[activeGearSettings.Length];
				for (int i = 0; i < activeGearSettings.Length; i++) {
					activeGears[i] = GearFactory.Instance.Build(activeGearSettings[i].gear, activeGearSettings[i].lv);
				}
				
				GearEntity[] passiveGears = new GearEntity[passiveGearSettings.Length];
				for (int i = 0; i < passiveGearSettings.Length; i++) {
					passiveGears[i] = GearFactory.Instance.Build(passiveGearSettings[i].gear, passiveGearSettings[i].lv);
				}
				
				return this.Build(activeGears, passiveGears);
		}
		
		public CoreEntity Build(GearEntity[] activeGears, GearEntity[] passiveGears) {
			CoreEntity Core = new CoreEntity(activeGears, passiveGears);
			return Core;
		}
	}
}