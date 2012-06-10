using UnityEngine;
using System.Collections.Generic;
using TinyQuest.Data.Cache;

namespace TinyQuest.Data{
	public enum ElementType {
		None,
		Fire,
		Water,
		Earth,
		Thunder
	};

	public enum AttributeType {
		None,
		Slash,
		Pierce,
		Smash,
		Magic
	};
	
	public enum BuffType {
		None,
		Poison,
		StrongPoison,
		Wet,
		Freeze,
		ArmorDown,
		ArmorUp,
		AttackDown,
		AttackUp,
		Regeneration,
		Paralyze,
		LastStand
	};
	
	public enum RarityType {
		Common,
		Uncommon,
		Heroic,
		Legend,
		Mythic
	};
	
	public enum WeaponCategory {
		None,
		Sword,
		BigSword,
		Katana,
		Knife,
		Bow,
		Spear,
		Rod,
		Book
	};
	
	public enum GrowthType {
		Slow,
		Normal,
		Fast
	};
	
	public enum ItemType {
		Core,
		Gear,
		Material,
		Recipe
	};

	public enum GearSlotType {
		Magic,
		Melee,
		Range
	};
	
	public struct MasterGearParameter {
		public enum Key {
			Exp = 0,
			Power = 1
		};
		public readonly int exp;
		public readonly int power;
		
		public MasterGearParameter(int[] rawData) {
			this.exp = rawData[(int)Key.Exp];
			this.power = rawData[(int)Key.Power];
		}
	}
	
	public class MasterMaterial : IDData{
		public string GetName() {
			return "";	
		}
		public string GetDescription() {
			return "";	
		}
	}

	public class MasterZoneMonster : IDData{
		int zoneID;
		float rate;
		int monster;
		bool isBoss;
	}
	
	public class MasterCoreBase : IDData {
		public readonly int hp;
		public readonly GrowthType growthType;
		
		public string GetName() {
			return "";	
		}
	}

	// Puppet core will define the basic feature of the puppet
	public class MasterCore : MasterCoreBase {
		public readonly RarityType rarity;
		
		public readonly GearSlotType[] activeSlotTypes;
		public readonly GearSlotType[] passiveSlotTypes;
		
		public string GetName() {
			return "";	
		}
	}

	public class MasterGearSetting : MasterCoreBase {
		public readonly int gear;
		public readonly int lv;
	}

	// Define monster data
	public class MasterMonster : MasterCoreBase {
		public readonly MasterGearSetting[] activeGears;
		public readonly MasterGearSetting[] passiveGears;
	}

	public class MasterSkill : IDData {
		public readonly int atk;
		public readonly string animation;
		public readonly BuffType buff;
		public readonly AttributeType attribute;
		public readonly ElementType element;
		public readonly WeaponCategory weaponCategory;

		public string GetName() {
			return CacheFactory.Instance.GetLocalizedTextCache().Get("Skill", this.id.ToString(), "name");
		}
	}
	
	public class MasterGear : IDData {
		public readonly int skill;
		public readonly int exp;
		public readonly GrowthType growthType;

		public string GetName() {
			return CacheFactory.Instance.GetMasterDataCache().GetSkillByID(this.skill).GetName();
		}
		
		public int GetLevel(int exp) {
			return 0;
		}
		
		public int GetMaxExp() {
			return 0;	
		}
	}
	
	public class MasterRecipe : IDData {
		public readonly int material1;
		public readonly int material2;
		public readonly int material3;
		public readonly int material1Count;
		public readonly int material2Count;
		public readonly int material3Count;
		public readonly ItemType itemType;
		public readonly int itemID;
	}
	
	// Define moster item drop table
	public class MasterMonsterDropItem : IDData {
		public readonly int monster;
		public readonly int rate;
		public readonly int drop;
		public readonly ItemType type;
	}

	public class MasterZone : IDData {
      	public readonly int stepCount;
		public readonly string path;
		public readonly ZoneEvent[] events;
		
		public string GetName() {
			return "";	
		}
	}
	
	public class MasterData {
		public readonly MasterZone Zone;
		public readonly MasterGear[] Gears;
		public readonly MasterSkill[] Skills;
		public readonly MasterCore[] Cores;
		public readonly MasterMonster[] Monsters;
		public readonly MasterZoneMonster[] ZoneMonsters;
		public readonly MasterMonsterDropItem[] MonsterDropItems;
		public readonly MasterRecipe[] Recipes;
		public readonly MasterMaterial[] Materials;
	}
	
	public class MasterFile {
		public readonly float version;
		public readonly MasterData data;
	}
}
