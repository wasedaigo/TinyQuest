using UnityEngine;
using TinyQuest.Data;
using TinyQuest.Entity;
using TinyQuest.Data.Cache;

namespace TinyQuest.Factory.Entity {
	public class UnitFactory {
		
		public static readonly UnitFactory Instance = new UnitFactory();
		private UnitFactory(){}
		
		
		public UnitEntity Build() {
			
			UnitEntity unit = new UnitEntity();
			return unit;
		}
	}
}