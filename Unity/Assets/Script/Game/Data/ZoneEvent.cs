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
		public int enemyId;
	}

	public class ZoneCommandBattleState {
		public int enemyHP;
		public bool playerTurn;
		public int turnCount;
		public int[] buffs;
		
		public ZoneCommandBattleState() {
		}
		
		public ZoneCommandBattleState(int enemyHP, bool playerTurn, int turnCount, int[] buffs) {
		  this.enemyHP = enemyHP;
		  this.playerTurn = playerTurn;
		  this.turnCount = turnCount;
		  this.buffs = buffs;
		}
	}
	
	public class ZoneCommandTreasure {
		public int treasureId;
	}
	
	public class ZoneCommandMessage {
		public string text;
	}
	
	public class ZoneEvent {
		public ZoneCommand[] commands;
	}
}
