using System.Collections.Generic;
using TinyQuest.Data.Cache;
namespace TinyQuest.Data{
	
	public class UserStatus {
		public int maxHP;	
	}
	
	public class UserMaterial : IDData{
		public int userPuppet;
		public int gear;
		public int exp;
	}
	
	public class UserGear : IDData{
		public int gear;
		public int exp;
		
		public MasterGear GetMasterGear() {
			return Cache.CacheFactory.Instance.GetMasterDataCache().GetGearByID(this.gear);	
		}
		
		public int GetLevel() {
			return this.GetMasterGear().GetLevel(this.exp);
		}
	}
	
	public class UserCore : IDData{
		public int core;
		public int[] activeGears;
		public int[] passiveGears;
		public int exp;
		
		public MasterCore GetMasterCore() {
			return Cache.CacheFactory.Instance.GetMasterDataCache().GetCoreByID(this.core);
		}
		
		public UserGear[] GetActiveUserGears() {
			UserGear[] userGears = new UserGear[this.activeGears.Length];
			for (int i = 0; i < this.activeGears.Length; i++) {
				userGears[i] = Cache.CacheFactory.Instance.GetLocalUserDataCache().GetUserGearByID(this.activeGears[i]);
			}
			return userGears;
		}
		
		public UserGear[] GetPassiveUserGears() {
			UserGear[] userGears = new UserGear[this.activeGears.Length];
			for (int i = 0; i < this.activeGears.Length; i++) {
				userGears[i] = Cache.CacheFactory.Instance.GetLocalUserDataCache().GetUserGearByID(this.activeGears[i]);
			}
			return userGears;
		}
	}

	public class UserPuppet : IDData{
		public int userCore;
		
		public UserCore GetUserCore() {
			return 	Cache.CacheFactory.Instance.GetLocalUserDataCache().GetUserCoreByID(this.userCore);
		}
	}
	
	public class MonsterInstance : IDData{
		public int monster;
		public int lv;
		
		public MasterMonster GetMasterMonster() {
			return 	Cache.CacheFactory.Instance.GetMasterDataCache().GetMonsterByID(this.monster);
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
		public CombatProgress combatProgress;
		public UserZone zone;
		public readonly UserStatus status;

		public readonly int[] party;
		public readonly UserPuppet[] ownPuppets;
		public readonly UserGear[] ownGears;
		public readonly UserCore[] ownCores;
		public readonly MonsterInstance[] monsterInstances;
		
	}
}