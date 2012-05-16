using UnityEngine;
using JsonFx.Json;
using System.Collections.Generic;
using TinyQuest.Data;

namespace TinyQuest.Data.Cache {
	public class MasterDataCache
	{
		protected MasterZone zone;
		protected Dictionary<int, MasterWeapon> masterWeaponDictionary;
		protected Dictionary<int, MasterSkill> masterSkillDictionary;
		protected Dictionary<int, MasterMonster> masterMonsterDictionary;
		
		public void Set(string jsonText) {
			MasterFile masterFile = JsonReader.Deserialize<MasterFile>(jsonText);
			MasterData masterData = masterFile.data;
			this.zone = masterData.Zone;
			this.masterWeaponDictionary = this.GetAsDictionary<MasterWeapon>(masterData.Weapons);
			this.masterSkillDictionary = this.GetAsDictionary<MasterSkill>(masterData.Skills);
			this.masterMonsterDictionary = this.GetAsDictionary<MasterMonster>(masterData.Monsters);
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
		
		public virtual MasterMonster GetMonsterByID(int id) {
			return this.masterMonsterDictionary[id];
		}
	}
}

