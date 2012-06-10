using UnityEngine;
using System.Collections;
using TinyQuest.Data;
using TinyQuest.Data.Cache;
using TinyQuest.Data.Request;
using TinyQuest.Factory.Entity;

namespace TinyQuest.Entity {

	public class CoreEntity {
		private GearEntity[] activeGears;
		private GearEntity[] passiveGears;
		public CoreEntity(GearEntity[] activeGears, GearEntity[] passiveGears) {
			this.activeGears = activeGears;
			this.passiveGears = passiveGears;
		}
		
		public GearEntity[] GetActiveGears() {
			return this.activeGears;
		}
		
		public GearEntity[] GetPassiveGears() {
			return this.passiveGears;
		}
	}
}