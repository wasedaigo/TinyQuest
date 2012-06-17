using UnityEngine;
using TinyQuest.Data;
using TinyQuest.Model;
using TinyQuest.Data.Cache;

namespace TinyQuest.Factory.Model {
	public class UnitFactory {
		
		public static readonly UnitFactory Instance = new UnitFactory();
		private UnitFactory(){}
		
		
		public UnitModel Build() {
			
			UnitModel unit = new UnitModel();
			return unit;
		}
	}
}