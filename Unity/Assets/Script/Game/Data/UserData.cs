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
		public int no;
		public int group;
		public int hp;
		public int[] buffs;
		
		public CombatBattler(){}
		public CombatBattler(int no, int group, int hp, int[] buffs) {
		  this.no = no;
		  this.group = group;
		  this.hp = hp;
		  this.buffs = buffs;
		}
	}
	
	public class CombatProgress {
		public int turnCount;
		public CombatBattler[] battlers;

		public CombatProgress(){}
		public CombatProgress(int turnCount, CombatBattler[] battlers) {
		  this.turnCount = turnCount;
		  this.battlers = battlers;
		}
		
		public CombatBattler GetCombatBattler(int no, int group) {
			for (int i = 0; i < this.battlers.Length; i++) {
				CombatBattler battler = this.battlers[i];
				if (battler.no == no && battler.group == group) {
					return battler;
				}
			}
			return null;
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