using UnityEngine;
using System.Collections.Generic;
using TinyQuest.Data.Cache;

namespace TinyQuest.Data{
	public struct CompositeData {
		public readonly int Skill;
		public readonly int CommandIndex1;
		public readonly int CommandIndex2;
		public readonly int CommandIndex3;
		
		public CompositeData(int skill, int index1, int index2, int index3) {
			this.Skill = skill;
			this.CommandIndex1 = index1;
			this.CommandIndex2 = index2;
			this.CommandIndex3 = index3;
		}
	}

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
	
	public enum UserType {
		All,
		Player,
		Monster
	};
	
	public enum WeaponCategory {
		Monster,
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
		Weapon,
		Material
	};
	
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
	
	public class MasterMaterial : IDData{
		public string GetName() {
			return "";	
		}
		public string GetDescription() {
			return "";	
		}
	}
	
	public class MasterRecipe : IDData{
		int count;
		int material;
		int weapon;
	}

	public class MasterZoneMonster : IDData{
		int zoneID;
		float rate;
		int monster;
		bool isBoss;
	}

	public class MasterWeapon : IDData{
		public static readonly int MinLevel = 1;
		public int MaxLevel {
			get {return 50;}	
		}

		public readonly string name;
		public readonly int category;
		public readonly int skill1;
		public readonly int skill2;
		public readonly int skill3;
		public readonly int atk;
		public readonly UserType userType;
		public readonly GrowthType growthType;
		public readonly RarityType rarity;
		public readonly int durability;
		
		public string GetUIImagePath() {
			return "UI/Weapon/" + this.id;	
		}
		
		public string GetAnimationImagePath() {
			return "Weapon/" + this.id;	
		}
		
		public int GetLevel(int exp) {
			/*
			int level = this.parameters.Length;
			int expKey = (int)MasterWeaponParameter.Key.Exp;
			for (int i = 0; i < this.parameters.Length; i++) {
				if (this.parameters[i][expKey] > exp) {
					level = id + 1;	
				}
			}*/
			int level = 1;
			return level;
		}
		/*
		public MasterWeaponParameter GetParam(int level) {
			if (level < MinLevel || level > MaxLevel) {
				Debug.LogError(level + " is out of range");
			}
			
			return new MasterWeaponParameter(this.parameters[level - MinLevel]);
		}*/
		
		public int GetMaxExp() {
			/*
			if (this.parameters.Length == 0) {
				Debug.LogError(" No parameter is defined");
			}
			
			MasterWeaponParameter param = new MasterWeaponParameter(this.parameters[this.MaxLevel - MinLevel]);
			return param.exp;*/
			return 1000;
		}
		
		public string GetName() {
			return "";	
		}
	}

	public class MasterMonster : IDData {
		public readonly int weapon1;
		public readonly int weapon2;
		public readonly int weapon3;
		public readonly int tp;
		public readonly int hp;
		public readonly GrowthType growthType;
		
		public string GetName() {
			return "";	
		}
	}

	public class MasterSkill : IDData {
		public readonly int atk;
		public readonly int tp;
		public readonly string animation;
		public readonly BuffType buff;
		public readonly AttributeType attribute;
		public readonly ElementType element;
		
		public string GetName() {
			return CacheFactory.Instance.GetLocalizedTextCache().Get("Skill", this.id.ToString(), "name");
		}
	}
	
	public class MasterCompositeSkill : IDData {
		public readonly int targetSkill;
		public readonly int baseSkill1;
		public readonly int baseSkill2;
		public readonly int baseSkill3;
	}
	
	public class MasterMonsterDropItem : IDData {
		public readonly int rate;
		public readonly int monster;
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
		public readonly MasterWeapon[] Weapons;
		public readonly MasterSkill[] Skills;
		public readonly MasterCompositeSkill[] CompositeSkills;
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
