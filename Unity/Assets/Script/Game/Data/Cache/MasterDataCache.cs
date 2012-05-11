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
		protected Dictionary<int, MasterEnemy> masterEnemyDictionary;
		
		public void Set(string jsonText) {
			MasterFile masterFile = JsonReader.Deserialize<MasterFile>(jsonText);
			MasterData masterData = masterFile.data;
			this.zone = masterData.zone;
			this.masterWeaponDictionary = this.GetAsDictionary<MasterWeapon>(masterData.weapons);
			this.masterSkillDictionary = this.GetAsDictionary<MasterSkill>(masterData.skills);
			this.masterEnemyDictionary = this.GetAsDictionary<MasterEnemy>(masterData.enemies);
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
			return this.masterSkillDictionary[id];
		}
		
		public virtual MasterEnemy GetEnemyByID(int id) {
			return this.masterEnemyDictionary[id];
		}
	}
}

