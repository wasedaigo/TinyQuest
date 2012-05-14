using UnityEngine;
using System.Collections.Generic;

namespace TinyQuest.Data{
	public struct MasterWeaponParameter {
		public enum Key {
			Exp = 0,
			Power = 1
		};
		public readonly int exp;
		public readonly int power;
		
		public MasterWeaponParameter(int[] rawData) {
			this.exp = rawData[(int)Key.Exp];
			this.power = rawData[(int)Key.Power];
		}
	}

	public class MasterWeapon : IDData{
		public static readonly int MinLevel = 1;
		public int MaxLevel {
			get {return this.parameters.Length;}	
		}

		public readonly string name;
		public readonly string path;
		public readonly string description;
		public readonly int power;
		public readonly int durability;
		public readonly int ap;
		public readonly int[][] parameters;
		public readonly int[] skills;
		
		public int GetLevel(int exp) {
			int level = this.parameters.Length;
			int expKey = (int)MasterWeaponParameter.Key.Exp;
			for (int i = 0; i < this.parameters.Length; i++) {
				if (this.parameters[i][expKey] > exp) {
					level = id + 1;	
				}
			}
			return level;
		}
		
		public MasterWeaponParameter GetParam(int level) {
			if (level < MinLevel || level > MaxLevel) {
				Debug.LogError(level + " is out of range");
			}
			
			return new MasterWeaponParameter(this.parameters[level - MinLevel]);
		}
		
		public int GetMaxExp() {
			if (this.parameters.Length == 0) {
				Debug.LogError(" No parameter is defined");
			}
			
			MasterWeaponParameter param = new MasterWeaponParameter(this.parameters[this.MaxLevel - MinLevel]);
			return param.exp;
		}
	}

	
	public class MasterEnemy : IDData {
		public readonly int hp;
		public readonly int power;
	}
		
	public class MasterSkill : IDData {
		public readonly string name;
		public readonly string path;
	}
	
	public class MasterZone : IDData {
      	public readonly int stepCount;
		public readonly string path;
		public readonly ZoneEvent[] events;
		//event
		//enemy
	}
	
	public class MasterData {
		public readonly MasterZone zone;
		public readonly MasterWeapon[] weapons;
		public readonly MasterSkill[] skills;
		public readonly MasterEnemy[] enemies;
	}
	
	public class MasterFile {
		public readonly float version;
		public readonly MasterData data;
	}
}
