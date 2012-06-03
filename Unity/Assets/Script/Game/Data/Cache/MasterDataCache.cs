using UnityEngine;
using JsonFx.Json;
using System.Collections.Generic;
using TinyQuest.Data;

namespace TinyQuest.Data.Cache {
	public class MasterDataCache
	{
		public struct SkillTuple {
		    public readonly int Skill1;
		    public readonly int Skill2;
			public readonly int Skill3;
		    public SkillTuple(int skill1, int skill2, int skill3) { Skill1 = skill1; Skill2 = skill2; Skill3= skill3;} 
		}

		protected MasterZone zone;
		protected Dictionary<int, MasterWeapon> masterWeaponDictionary;
		protected Dictionary<int, MasterSkill> masterSkillDictionary;
		protected Dictionary<int, MasterMonster> masterMonsterDictionary;
		
		protected Dictionary<int, MasterCompositeSkill> masterCompositeSkillDictionary;
		protected Dictionary<SkillTuple, MasterCompositeSkill> masterCompositeSkillMatchDictionary;
		
		public void Set(string jsonText) {
			MasterFile masterFile = JsonReader.Deserialize<MasterFile>(jsonText);
			MasterData masterData = masterFile.data;
			this.zone = masterData.Zone;
			this.masterWeaponDictionary = this.GetAsDictionary<MasterWeapon>(masterData.Weapons);
			this.masterSkillDictionary = this.GetAsDictionary<MasterSkill>(masterData.Skills);
			this.masterMonsterDictionary = this.GetAsDictionary<MasterMonster>(masterData.Monsters);
			
			
			this.masterCompositeSkillDictionary = new Dictionary<int, MasterCompositeSkill>();
			foreach (MasterCompositeSkill skill in masterData.CompositeSkills) {
				masterCompositeSkillDictionary.Add(skill.targetSkill, skill);
			}
			
			this.masterCompositeSkillMatchDictionary = new Dictionary<SkillTuple, MasterCompositeSkill>();
			foreach (MasterCompositeSkill skill in masterData.CompositeSkills) {
				SkillTuple tuple = new SkillTuple(skill.baseSkill1, skill.baseSkill2, skill.baseSkill3);
				masterCompositeSkillMatchDictionary.Add(tuple, skill);
			}
		}
		
		public void SetZone(string jsonText) {
			this.zone = JsonReader.Deserialize<MasterZone>(jsonText);
			
		}
		
		private Dictionary<int, T> GetAsDictionary<T>(T[] data) 
			where T : IDData
		{
			Dictionary<int, T> dictionary = new Dictionary<int, T>();
			for (int i = 0; i < data.Length; i++) {
				dictionary.Add(data[i].id, data[i]);
			}
			return dictionary;
		}

		public virtual MasterZone GetLoadedZone() {
			return this.zone;
		}
		
		public virtual MasterWeapon GetWeaponByID(int id) {
			return this.masterWeaponDictionary[id];
		}
		
		public virtual MasterSkill GetSkillByID(int id) {
			if (this.masterSkillDictionary.ContainsKey(id)) {
				return this.masterSkillDictionary[id];
			} else {
				return null;	
			}
		}
		
		public MasterCompositeSkill GetCompositeSkillById(int id) {
			MasterCompositeSkill compositeSkill = null; 
			if (this.masterCompositeSkillDictionary.ContainsKey(id)) {
				compositeSkill = this.masterCompositeSkillDictionary[id];
			}
			
			return compositeSkill;
		}
		
		public SkillCompositeType GetCompositeType(int skillID) {
			SkillCompositeType compositeType = SkillCompositeType.Single;
			MasterCompositeSkill compositeSkill = this.GetCompositeSkillById(skillID);
			if (compositeSkill != null) {
				if ((compositeSkill.baseSkill1 > 0) && (compositeSkill.baseSkill2 > 0)) {
					if (compositeSkill.baseSkill3 > 0) {
						compositeType = SkillCompositeType.Triple;	
					} else {
						compositeType = SkillCompositeType.Double;	
					}
				}
			}
			
			return compositeType;
		}
		
		public virtual CompositeData GetCompositeData(int skill1, int skill2, int skill3) {
			int[] skills = new int[]{0, skill1, skill2, skill3};
			
			int[][] indexes = new int[15][];
			indexes[0] = new int[]{1, 2, 3};
			indexes[1] = new int[]{1, 3, 2};
			indexes[2] = new int[]{2, 1, 3};
			indexes[3] = new int[]{2, 3, 1};
			indexes[4] = new int[]{3, 1, 2};
			indexes[5] = new int[]{3, 2, 1};
			indexes[6] = new int[]{1, 2, 0};
			indexes[7] = new int[]{2, 1, 0};
			indexes[8] = new int[]{1, 3, 0};
			indexes[9] = new int[]{3, 1, 0};
			indexes[10] = new int[]{2, 3, 0};
			indexes[11] = new int[]{3, 2, 0};
			indexes[12] = new int[]{1, 0, 0};
			indexes[13] = new int[]{2, 0, 0};
			indexes[14] = new int[]{3, 0, 0};

			for (int i = 0; i < indexes.Length; i++) {
				int[] skillIndexes = indexes[i];
				SkillTuple tuple = new SkillTuple(skills[skillIndexes[0]], skills[skillIndexes[1]], skills[skillIndexes[2]]);
				if (this.masterCompositeSkillMatchDictionary.ContainsKey(tuple)) {
					int[] baseSkills = new int[3];
					for (int j = 0; j < skillIndexes.Length; j++) {
						int index = skillIndexes[j];
						if (index > 0) {
							baseSkills[index - 1] = skills[index];
						}
					}
					
					MasterCompositeSkill skill = this.masterCompositeSkillMatchDictionary[tuple];
					return new CompositeData(skill.targetSkill, baseSkills);
				}
			}
			
			return null;
		}
		
		public virtual MasterMonster GetMonsterByID(int id) {
			return this.masterMonsterDictionary[id];
		}
	}
}

