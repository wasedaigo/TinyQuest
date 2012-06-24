using UnityEngine;
using System.Collections.Generic;
using TinyQuest.Data.Cache;

namespace TinyQuest.Data{

	public enum AttributeType {
		None,
		Fire,
		Water,
		Earth,
		Thunder,
		Dark,
		Light,
		Slash,
		Pierce,
		Smash
	};
	
	public enum SkillType {
		Active,
		Passive
	};
	

	public enum UnitType {
		None,
		Melee,
		Magic,
		Range
	};
	
	public enum RarityType {
		Common,
		Uncommon,
		Heroic,
		Legend,
		Mythic
	};
	
	public enum ItemType {
		Core,
		Gear,
		Material,
		Recipe
	};
	
	public enum UnitLookType {
		Puppet,
		Monster
	};
	
	public enum CastTiming {
		OnZoneStart,
		OnTurnStart,		

		OnReady,
		OnTarget,
		OnDamage,
		OnFinish,
		
		OnTargetted,
		OnDamaged,
		OnDead,
		
		OnTurnEnd,
		OnZoneEnd
	};

	public enum BuffType {
		FireResistUp,
		WaterResistUp,		
		WindResistUp,
		EarthResistUp,
		LightResistUp,
		DarkResistUp,
		FireResistDown,
		WaterResistDown,		
		WindResistDown,
		EarthResistDown,
		LightResistDown,
		DarkResistDown,

		PowerUp,
		PowerDown,
		SpeedUp,
		SpeedDown,
		DefUp,
		DefDown,
		RegenUp,
		RegenDown,
		
		Heal,
		Poison,
		Stun,
		Paralyze,
		Revive,
	};
	
	public enum BuffLifeTimeType {
		OneTurn,
		TwoTurn,
		ThreeTurn,
		Zone,
		Action
	};
	
	public enum GrowthType {
		Slow,
		Normal,
		Fast
	};

	public class GrowthTable {
		public readonly int startValue;
		public readonly int maxValue;
		public readonly GrowthType growthType;
		
		public int GetValue(int keyValue) {
			return startValue;	
		}
	};
	
	public class MasterUnit : IDData {
		public readonly int unitType;
		public readonly GrowthTable lvTable;
		public readonly GrowthTable hpTable;
		public readonly GrowthTable powerTable;
		public readonly GrowthTable regenTable;
		public readonly GrowthTable defTable;
		public readonly GrowthTable speedTable;

		public readonly AttributeType[] weakPoints;
		
		public readonly int weapon;
		public readonly UnitLookType lookType;
		public readonly RarityType rarity;
		public readonly int[] skills;
		
		public string GetName() {
			return "";	
		}
	}

	public class MasterSkill : IDData {
		public readonly CastTiming castTiming;
		public readonly SkillType type;

		public readonly string animation;
		public readonly int buff;
		public readonly int buffEffect;
		public readonly BuffLifeTimeType buffLifeTime;
		public readonly int buffRate;
		
		public readonly AttributeType attribute;
		public readonly int chance;

		public string GetName() {
			return CacheFactory.Instance.GetLocalizedTextCache().Get("Skill", this.id.ToString(), "name");
		}
	}
	
	public class MasterBuff : IDData {
		public readonly BuffType type;
		public readonly int probability;
	}

	public class MasterCore : IDData {
		public readonly Dictionary<string, Dictionary<string,int>[]> recipe;
		public readonly int masterUnit;
	}

	// Define moster item drop table
	/*
	public class MasterMonsterDropItem : IDData {
		public readonly int monster;
		public readonly int rate;
		public readonly int drop;
		public readonly ItemType type;
	}*/

	public class MasterZone : IDData {
      	public readonly int stepCount;
		public readonly string path;
		public readonly ZoneEvent[] events;
		
		public string GetName() {
			return "";	
		}
	}
	
	public class MasterData {
		public readonly MasterUnit[] Units;
		public readonly MasterBuff[] Buffs;
		public readonly MasterSkill[] Skills;
		public readonly MasterCore[] Cores;
	}
	
	public class MasterFile {
		public readonly float version;
		public readonly MasterData data;
	}
}
