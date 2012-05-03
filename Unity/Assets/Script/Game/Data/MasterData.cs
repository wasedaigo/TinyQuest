using UnityEngine;
using System.Collections.Generic;

namespace TinyQuest.Data{
	public class BaseMasterData
	{
		public int id;
	}
	
	public class MasterDataCollection<T>
		where T : BaseMasterData
	{
		public T[] data;
	}
	
	public struct MasterWeaponParameter {
		public enum Key {
			Exp = 0,
			Power = 1,
			Chance1 = 2,
			Chance2 = 3,
			Chance3 = 4
		};
		public int exp;
		public int power;
		public int chance1;
		public int chance2;
		public int chance3;

		public MasterWeaponParameter(int[] rawData) {
			this.exp = rawData[(int)Key.Exp];
			this.power = rawData[(int)Key.Power];
			this.chance1 = rawData[(int)Key.Chance1];
			this.chance2 = rawData[(int)Key.Chance2];
			this.chance3 = rawData[(int)Key.Chance3];
		}
	}
	
	public class MasterWeapon : BaseMasterData {
		public static readonly int MinLevel = 1;
		public static readonly int MaxLevel = 99;
		
		public string name;
		public string path;
		public string description;
		public int power;
		public int[][] parameters;
		
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
		
		public System.Nullable<MasterWeaponParameter> GetParam(int level) {
			if (level >= MinLevel && level <= MaxLevel) {
				return new MasterWeaponParameter(this.parameters[level - MinLevel]);
			} else {
				Debug.LogError(level + " is out of range");
				return null;
			}
		}
	}
	
	public class MasterSkill : BaseMasterData {
		public string name;
		public string path;
	}
	
	public class MasterZone : BaseMasterData {
      	public int stepCount;
		public string path;
		ZoneEvent[] events;
		//event
		//enemy
	}
}
