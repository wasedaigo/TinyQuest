using UnityEngine;
using TinyQuest.Entity;

namespace TinyQuest.Factory.Entity {
	public class ZoneFactory {
		public static readonly ZoneFactory Instance = new ZoneFactory();
		private ZoneFactory(){}

		public StageEntity Build(int id) {
			StageEntity stage = new StageEntity();
			return stage;
		}
	}
}