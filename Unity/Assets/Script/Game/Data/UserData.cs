using System.Collections.Generic;
namespace TinyQuest.Data{
	
	public class UserStatus {
		public int maxHP;	
	}
	
	public class UserWeapon : IDData{
		public int weaponId;
		public int exp;
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
		public int currentTP;
	}
	
	public class CombatBattler {
		public const int HandCount = 3;
		public int no;
		public int group;
		public int hp;
		public int[] buffs;
		public int[] handSkills;

		public CombatBattler(){
			this.handSkills = new int[HandCount]{-1, -1, -1};
		}
		public CombatBattler(int no, int group, int hp, int[] buffs) {
		  this.handSkills = new int[HandCount]{-1, -1, -1};
		  this.no = no;
		  this.group = group;
		  this.hp = hp;
		  this.buffs = buffs;
		}
	}
	
	public class CombatProgress {
		public int turnCount;
		public CombatBattler[][] battlers;
		
		public CombatProgress(){}
		public CombatProgress(int turnCount, CombatBattler[][] battlers) {
		  this.turnCount = turnCount;
		  this.battlers = battlers;
		}
		
		public CombatBattler GetCombatBattler(int no, int group) {
			return this.battlers[group][no];
		}
	}

	public class LocalUserData {
		public CombatProgress combatProgress;
		public UserZone zone;
		public readonly UserStatus status;
		public readonly UserWeapon[] equipWeapons;
		public readonly UserWeapon[] stockWeapons;
	}
}