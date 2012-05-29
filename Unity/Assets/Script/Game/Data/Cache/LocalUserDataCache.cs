using UnityEngine;
using JsonFx.Json;
using System.Collections.Generic;
using System.IO;
using TinyQuest.Data;
using Async;

namespace TinyQuest.Data.Cache {
	public class LocalUserDataCache
	{
		protected LocalUserData localUserData;
		
		public string Serialize() {
			// Need to implement
			return "";	
		}
		
		public void Set(string jsonText) {
			this.localUserData = JsonReader.Deserialize<LocalUserData>(jsonText);
		}
		
		public void SetZone(string jsonText) {
			this.localUserData.zone = JsonReader.Deserialize<UserZone>(jsonText);
		}
		
		public virtual void SetCombatProgress(CombatProgress combatProgress) {
			this.localUserData.combatProgress = combatProgress;
		}
			
		public virtual CombatProgress GetCombatProgress() {
			return this.localUserData.combatProgress;
		}
			
		public virtual UserStatus GetUserStatus() {
			return this.localUserData.status;
		}
			
		public virtual UserZone GetUserZone() {
			return this.localUserData.zone;
		}
		
		public virtual UserWeapon[] GetEquipWeapons() {
			return this.localUserData.equipWeapons;
		}
	
		public virtual UserWeapon[] GetStockWeapons() {
			return this.localUserData.stockWeapons;
		}
		
		public void Commit() {
			return;
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

