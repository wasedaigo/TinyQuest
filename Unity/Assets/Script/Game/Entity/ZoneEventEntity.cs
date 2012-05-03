using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TinyQuest.Data;

namespace TinyQuest.Entity {

	public class ZoneEventEntity {
		private int currentStep;
		private Dictionary<int, BattlerEntity> enemies =  new Dictionary<int, BattlerEntity>();
		
		public ZoneEventEntity() {
			
		}
		
		public void SetEnemy(int stepNo, BattlerEntity enemy) {
			this.enemies.Add(stepNo, enemy);
		}
	}
}
