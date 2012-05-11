namespace TinyQuest.Data{
	public class ZoneCommand {
		public enum Type {
			Battle = 0,
			Message = 1,
			Treasure = 2
		}
		
		public int type;
		public object content;
	}
	
	public class ZoneCommandBattle {
		public int enemyID;
	}
	
	public class ZoneCommandTreasure {
		public int treasureID;
	}
	
	public class ZoneCommandMessage {
		public string text;
	}
	
	public class ZoneEvent {
		public ZoneCommand[] commands;
	}
}
