using TinyQuest.Entity;
namespace TinyQuest.Cache {
	public class MapCache {	
		private static MapCache instance;
		public static MapCache GetInstance() {
			if (instance == null) {
				instance = new MapCache();
			}
			return instance;
		}
		
		private MapCache() {
			
		}

		public Model.MapModel GetModel() {
			StepData[] steps = new StepData[5];
			steps[0] = new StepData(1, 100, 100);
			steps[1] = new StepData(2, 150, 100);
			steps[2] = new StepData(3, 100, 150);
			steps[3] = new StepData(4, 200, 200);
			steps[4] = new StepData(5, 250, 250);
			
			return new Model.MapModel(steps);
				
		}
	}
}