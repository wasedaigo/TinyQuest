using System.Collections.Generic;
using TinyQuest.Data.Cache;
namespace TinyQuest.Data{
	
	public class UserStatus {
		public int maxHP;	
	}
	
	public class UserMaterial : IDData{
		public int userUnit;
		public int gear;
		public int exp;
	}
	
	public class UserUnit : IDData{
		public int unit;
		public int exp;
		public MasterUnit GetMasterUnit() {
			return 	Cache.CacheFactory.Instance.GetMasterDataCache().GetUnitByID(this.unit);
		}
	}
	
	public class UnitInstance : IDData{
		public int unit;
		public int lv;
		
		public MasterUnit GetMasterUnit() {
			return 	Cache.CacheFactory.Instance.GetMasterDataCache().GetUnitByID(this.unit);
		}
	}
	
	public class UserZone{
		public int zoneId;
		public int lastStepIndex;
		public Dictionary<string, ZoneEvent> events;

		public int playerHP;
    	public int stepIndex;
    	public int commandIndex;
		public object commandState;
	}
	
	public enum CombatBattlerType {
		Player,
		Monster
	}
	
	public class CombatBattler {
		public int hp;
		public int[] buffs;
		public CombatBattlerType battlerType;
		public int battlerID;
		
		public CombatBattler(){
		}

		public CombatBattler(CombatBattlerType battlerType, int battlerID, int hp, int[] buffs) {
			this.battlerID = battlerID;
			this.battlerType = battlerType;
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
		public CombatProgress CombatProgress;
		public UserZone Zone;
		public readonly UserStatus Status;

		public readonly UserUnit[] OwnUnits;
		//public readonly UnitInstance[] unitInstances;
		
	}
}