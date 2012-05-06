namespace TinyQuest.Data{
	public class ZoneCommand {
		public enum ZoneCommandType {
			Battle,
			Message,
			Treasure
		}
		
		public int type;
		public object content;

	}
	
	public class ZoneCommandBattle {
		public int enemyId;
	}

	public class ZoneCommandTreasure {
		public int treasureId;
	}
	
	public class ZoneCommandMessage {
		public string key;
	}
	
	public class ZoneEvent {
		public int step;
		public ZoneCommand[] commands;
	}
}