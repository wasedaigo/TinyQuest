using UnityEngine;
using TinyQuest.Entity;

namespace TinyQuest.Factory.Entity {
	public class StageFactory {
		public static readonly StageFactory Instance = new StageFactory();
		private StageFactory(){}

		public StageEntity Build(int no) {
			StageEntity stage = new StageEntity();
			return stage;
		}
	}
}