using UnityEngine;
using JsonFx.Json;
using System.Collections.Generic;
using TinyQuest.Data;

namespace TinyQuest.Data.Cache {
	public class MasterDataCache
	{
		protected MasterData masterData;
		protected MasterZone zone;
		protected Dictionary<int, MasterWeapon> masterWeaponDictionary;
		protected Dictionary<int, MasterSkill> masterSkillDictionary;
		
		public void Set(string jsonText) {
			MasterFile masterFile = JsonReader.Deserialize<MasterFile>(jsonText);
			this.masterData = masterFile.data;
			this.zone = this.masterData.zone;
			this.masterWeaponDictionary = this.GetAsDictionary<MasterWeapon>(this.masterData.weapons);
			this.masterData.weapons = null;
			this.masterSkillDictionary = this.GetAsDictionary<MasterSkill>(this.masterData.skills);
			this.masterData.skills = null;
		}
		
		public void SetZone(string jsonText) {
			this.masterData.zone = JsonReader.Deserialize<MasterZone>(jsonText);
			
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
			return this.masterSkillDictionary[id];
		}
	}
}

