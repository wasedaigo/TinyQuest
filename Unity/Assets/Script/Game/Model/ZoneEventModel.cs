using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TinyQuest.Data;

namespace TinyQuest.Model {

	public class ZoneEventModel {
		private int currentStep;
		private Dictionary<int, BattlerModel> enemies =  new Dictionary<int, BattlerModel>();
		
		public ZoneEventModel() {
			
		}
	}
}
