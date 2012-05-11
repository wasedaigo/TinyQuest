using System.Collections.Generic;
namespace TinyQuest.Data{
	
	public class UserStatus {
		public int maxHP;	
	}
	
	public class UserWeapon : IDData{
		public int weaponId;
		public int exp;
		public int slot;
	}
	
	public class UserZone{
		public int zoneId;
		public int lastStepIndex;
		public Dictionary<string, ZoneEvent> events;

		public int playerHP;
    	public int stepIndex;
    	public int commandIndex;
		public object commandState;
		public int[] weaponDurabilities;
		public int currentAP;
	}

	public class CombatProgress {
		public int enemyHP;
		public bool playerTurn;
		public int turnCount;
		public int[] buffs;
		
		public CombatProgress() {
		}
		
		public CombatProgress(int enemyHP, bool playerTurn, int turnCount, int[] buffs) {
		  this.enemyHP = enemyHP;
		  this.playerTurn = playerTurn;
		  this.turnCount = turnCount;
		  this.buffs = buffs;
		}
	}

	public class LocalUserData {
		public readonly CombatProgress combatProgress;
		public readonly UserStatus status;
		public readonly UserZone zone;
		public readonly UserWeapon[] equipWeapons;
		public readonly UserWeapon[] stockWeapons;
	}
}