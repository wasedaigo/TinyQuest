using UnityEngine;
using JsonFx.Json;
using System.Collections.Generic;
using System.IO;
using TinyQuest.Data;
using Async;

namespace TinyQuest.Data.Cache {
	public class LocalUserDataCache
	{
		public static readonly LocalUserDataCache Instance = new LocalUserDataCache();
		private LocalUserDataCache(){}
		
		private UserZone userZone;
		private Dictionary<int, UserWeapon> weaponDictionary;
		private Dictionary<int, UserZoneProgress> zoneProgressDictionary;
		
		public string Serialize() {
			// Need to implement
			return "";	
		}
		
		public void Set(string jsonText) {
			LocalUserData localUserData = JsonReader.Deserialize<LocalUserData>(jsonText);

			this.userZone = localUserData.zone;
			this.zoneProgressDictionary = this.GetAsDictionary<UserZoneProgress>(localUserData.zoneProgresses);
			this.weaponDictionary = this.GetAsDictionary<UserWeapon>(localUserData.weapons);
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
		
		public UserWeapon GetWeaponByID(int id) {
			return this.weaponDictionary[id];
		}
		
		public UserZoneProgress GetZoneProgressByID(int id) {
			return this.zoneProgressDictionary[id];
		}
	}
}

