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
		
		protected Dictionary<SkillTuple, MasterCompositeSkill> masterCompositeSkillDictionary;
		
		public void Set(string jsonText) {
			MasterFile masterFile = JsonReader.Deserialize<MasterFile>(jsonText);
			MasterData masterData = masterFile.data;
			this.zone = masterData.Zone;
			this.masterWeaponDictionary = this.GetAsDictionary<MasterWeapon>(masterData.Weapons);
			this.masterSkillDictionary = this.GetAsDictionary<MasterSkill>(masterData.Skills);
			this.masterMonsterDictionary = this.GetAsDictionary<MasterMonster>(masterData.Monsters);
			
			this.masterCompositeSkillDictionary = new Dictionary<SkillTuple, MasterCompositeSkill>();
			foreach (MasterCompositeSkill skill in masterData.CompositeSkills) {
				SkillTuple tuple = new SkillTuple(skill.baseSkill1, skill.baseSkill2, skill.baseSkill3);
				masterCompositeSkillDictionary.Add(tuple, skill);
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
		
		public virtual List<CompositeData> GetCompositeDataList(int skill1, int skill2, int skill3) {
			int[] skills = new int[]{0, skill1, skill2, skill3};
			List<CompositeData> compositeDataList = new List<CompositeData>();

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
				int index1 = indexes[i][0];
				int index2 = indexes[i][1];
				int index3 = indexes[i][2];
				SkillTuple tuple = new SkillTuple(skills[index1], skills[index2], skills[index3]);
				
				if (this.masterCompositeSkillDictionary.ContainsKey(tuple)) {
					MasterCompositeSkill skill = this.masterCompositeSkillDictionary[tuple];
					CompositeData data = new CompositeData(skill.targetSkill, index1, index2, index3);
					compositeDataList.Add(data);	
				}
			}
			
			return compositeDataList;
		}
		
		public virtual MasterMonster GetMonsterByID(int id) {
			return this.masterMonsterDictionary[id];
		}
	}
}

