using UnityEngine;
using JsonFx.Json;
using System.Collections.Generic;
using System.IO;
using TinyQuest.Data;
using Async;

namespace TinyQuest.Data.Cache {
	public class LocalUserDataCache
	{
		private LocalUserData localUserData;
		protected UserZone userZone;
		protected UserWeapon[] equipWeapons;
		protected UserWeapon[] stockWeapons;
		protected Dictionary<int, UserZoneProgress> zoneProgressDictionary;
		
		public string Serialize() {
			// Need to implement
			return "";	
		}
		
		public void Set(string jsonText) {
			this.localUserData = JsonReader.Deserialize<LocalUserData>(jsonText);

			this.userZone = this.localUserData.zone;
			this.zoneProgressDictionary = this.GetAsDictionary<UserZoneProgress>(this.localUserData.zoneProgresses);
			this.equipWeapons = this.localUserData.equipWeapons;
			this.stockWeapons = this.localUserData.stockWeapons;
		}
		
		public void SetZone(string jsonText) {
			this.userZone = JsonReader.Deserialize<UserZone>(jsonText);
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

		public virtual UserZone GetUserZone() {
			return this.userZone;
		}
		
		public virtual UserWeapon[] GetEquipWeapons() {
			return this.equipWeapons;
		}
	
		public virtual UserWeapon[] GetStockWeapons() {
			return this.stockWeapons;
		}
		
		public virtual UserZoneProgress GetZoneProgressByID(int id) {
			return this.zoneProgressDictionary[id];
		}
		
		public void Commit() {
			string text = JsonWriter.Serialize(this.localUserData);
  			StreamWriter writer = new StreamWriter("Assets/Resources/Data/LocalUser.txt"); 
			writer.Write(text);
			writer.Close();
		}
	}
}

