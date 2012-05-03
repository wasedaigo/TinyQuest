namespace TinyQuest.Data{
	public class ZoneCommand {
		public enum ZoneCommandType {
			Battle,
			Message,
			Treasure
		}
		
		public ZoneCommandType type;
	}
	
	public class ZoneCommandBattle : ZoneCommand {
		public int enemyId;
	}

	public class ZoneCommandTreasure : ZoneCommand {
		public int treasureId;
	}
	
	public class ZoneCommandMessage : ZoneCommand {
		public string key;
	}
	
	public struct ZoneEvent {
		public enum ZoneEventType {
			Story,
			Battle,
			Boss,
			Treasure
		}
		
		public ZoneEventType type;
		public ZoneCommand[] commands;
	}
}