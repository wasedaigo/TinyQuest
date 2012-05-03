using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TinyQuest.Data;

namespace TinyQuest.Entity {

	public class ZoneEntity {
		private int currentStep;
		private Dictionary<int, ZoneEventEntity> events =  new Dictionary<int, ZoneEventEntity>();
		
		public ZoneEntity() {
			
		}
		
		public void SetEnemy(int stepNo, ZoneEventEntity zoneEvent) {
			this.events.Add(stepNo, zoneEvent);
		}
	}
}
