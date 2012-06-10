using UnityEngine;
using JsonFx.Json;
using System.Collections.Generic;
using TinyQuest.Data;

namespace TinyQuest.Data.Cache {
	public class MasterDataCache
	{
		protected MasterZone zone;
		protected Dictionary<int, MasterGear> masterGearDictionary;
		protected Dictionary<int, MasterSkill> masterSkillDictionary;
		protected Dictionary<int, MasterMonster> masterMonsterDictionary;
		protected Dictionary<int, MasterCore> masterCoreDictionary;
		
		public void Set(string jsonText) {
			MasterFile masterFile = JsonReader.Deserialize<MasterFile>(jsonText);
			MasterData masterData = masterFile.data;
			this.zone = masterData.Zone;
			this.masterGearDictionary = this.GetAsDictionary<MasterGear>(masterData.Gears);
			this.masterSkillDictionary = this.GetAsDictionary<MasterSkill>(masterData.Skills);
			this.masterMonsterDictionary = this.GetAsDictionary<MasterMonster>(masterData.Monsters);
			this.masterCoreDictionary = this.GetAsDictionary<MasterCore>(masterData.Cores);
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
		
		public virtual MasterGear GetGearByID(int id) {
			return this.masterGearDictionary[id];
		}
		
		public virtual MasterSkill GetSkillByID(int id) {
			if (this.masterSkillDictionary.ContainsKey(id)) {
				return this.masterSkillDictionary[id];
			} else {
				return null;	
			}
		}
		
		public virtual MasterCore GetCoreByID(int id) {
			if (this.masterCoreDictionary.ContainsKey(id)) {
				return this.masterCoreDictionary[id];
			} else {
				return null;	
			}
		}
		
		public virtual MasterMonster GetMonsterByID(int id) {
			return this.masterMonsterDictionary[id];
		}
	}
}

