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
			steps[0] = new StepData(1, 100, 250);
			steps[1] = new StepData(2, 100, 300);
			steps[2] = new StepData(3, 100, 350);
			steps[3] = new StepData(4, 100, 400);
			steps[4] = new StepData(5, 100, 450);
			
			return new Model.MapModel(steps);
				
		}
	}
}