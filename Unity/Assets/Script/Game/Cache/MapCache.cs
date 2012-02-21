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
			return new Model.MapModel();	
		}
	}
}