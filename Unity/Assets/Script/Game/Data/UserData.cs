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
	
	public class UserZone{
		public int zoneId;
		public int lastStepIndex;
		public Dictionary<string, ZoneEvent> events;

		public int playerHP;
    	public int stepIndex;
    	public int commandIndex;
		public object commandState;
	}
	
	public class CombatUnit {
		private readonly int id;
		public int hp;
		public int[] buffs;
		public int groupType;
		public int index;
		
		public CombatUnit(){
		}
		
		public UserUnit GetUserUnit() {
			LocalUserData data = Cache.CacheFactory.Instance.GetLocalUserDataCache().Data;
			if (this.groupType == 0) {
				return data.GetOwnUnitByID(this.id);
			} else {
				return data.GetZoneUnitByID(this.id);
			}
		}

		public CombatUnit(int id,  int groupType, int index, int hp, int[] buffs) {
			this.id = id;
			this.hp = hp;
			this.groupType = groupType;
			this.index = index;
			this.buffs = buffs;
		}
	}
	
	public class CombatProgress {
		public int turnCount;
		public List<CombatUnit>[] combatUnitGroups;
		
		public CombatProgress(List<CombatUnit>[] combatUnitGroups) {
			this.combatUnitGroups = combatUnitGroups;
			this.turnCount = 0;
		}
	}

	public class UserUnit : IDData{
		public int unit;
		public int exp;
		public int skillExp;

		public MasterUnit Unit{
			get{
				return Cache.CacheFactory.Instance.GetMasterDataCache().GetUnitByID(this.unit);
			}
		}

		public int Level {
			get {
				return this.Unit.lvTable.GetValue(this.exp);
			}
		}

		public int MaxHP {
			get {
				return this.Unit.hpTable.GetValue(this.Level);
			}
		}

		private int hp;
		public int HP {
			get {return this.hp;}
		}
		
		public bool IsDead {
			get {return this.hp <= 0;}
		}
		
		public UserUnit(){}
		public UserUnit(int id, int unit, int exp, int skillExp) {
			this.unit = unit;
			this.exp = exp;
			this.skillExp = skillExp;
			this.hp = this.MaxHP;
		}
	}

	public class LocalUserData {
		public CombatProgress combatProgress;
		public UserZone zone;
		public readonly UserStatus status;
		public int maxOwnUnitId;
		public List<UserUnit> ownUnits;
		public int maxZoneUnitId;
		public List<UserUnit> zoneUnits;
		
		private UserUnit AddUserUnit(int unit, int exp, int skillExp, ref int maxId, ref List<UserUnit> units) {
			maxId += 1;
			UserUnit userUnit = new UserUnit(maxId, unit, exp, skillExp);
			units.Add(userUnit);
			return  userUnit;
		}
		
		private void RemoveUserUnit(UserUnit userUnit, ref List<UserUnit> units) {
			units.Remove(userUnit);
		}
		
		private UserUnit GetUnitByID(int id, ref List<UserUnit> units) {
			foreach (UserUnit userUnit in units) {
				if (userUnit.id == id) { return userUnit; }
			}
			return null;
		}
		
		public UserUnit AddOwnUnit(int unit, int exp, int skillExp) {
			return this.AddUserUnit(unit, exp, skillExp, ref this.maxOwnUnitId, ref this.ownUnits);
		}
		
		public void RemoveOwnUnit(UserUnit userUnit) {
			this.RemoveUserUnit(userUnit, ref this.ownUnits);
		}
		
		public UserUnit GetOwnUnitByID(int id) {
			return this.GetUnitByID(id, ref this.ownUnits);
		}
		
		public UserUnit AddZoneUnit(int unit, int exp, int skillExp) {
			return this.AddUserUnit(unit, exp, skillExp, ref this.maxZoneUnitId, ref this.zoneUnits);
		}
		
		public void RemoveZoneUnit(UserUnit userUnit) {
			this.RemoveUserUnit(userUnit, ref this.zoneUnits);
		}
		
		public UserUnit GetZoneUnitByID(int id) {
			return this.GetUnitByID(id, ref this.zoneUnits);
		}
		//public readonly UnitInstance[] unitInstances;
		
	}

}