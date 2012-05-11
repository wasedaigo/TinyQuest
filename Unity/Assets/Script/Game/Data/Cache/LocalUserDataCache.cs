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
		protected UserStatus userStatus;
		protected UserZone userZone;
		protected CombatProgress combatProgress;
		protected UserWeapon[] equipWeapons;
		protected UserWeapon[] stockWeapons;
		
		public string Serialize() {
			// Need to implement
			return "";	
		}
		
		public void Set(string jsonText) {
			this.localUserData = JsonReader.Deserialize<LocalUserData>(jsonText);
			
			this.userStatus = this.localUserData.status;
			this.userZone = this.localUserData.zone;
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
		
		public virtual void SetCombatProgress(CombatProgress combatProgress) {
			this.combatProgress = combatProgress;
		}
			
		public virtual CombatProgress GetCombatProgress() {
			return this.combatProgress;
		}
			
		public virtual UserStatus GetUserStatus() {
			return this.userStatus;
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
		
		public void Commit() {
			string text = JsonWriter.Serialize(this.localUserData);

			// make a path
			string path = Path.Combine(Application.persistentDataPath, "Save/LocalUser.txt");
			Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "Save"));
			
			// write some data
			using (StreamWriter writer = new StreamWriter(File.Create(path)))
			{
			    writer.Write(text);
			}
			
			Debug.Log("LocalUser.txt Committed at " + path);
		}
	}
}

